using System;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NovationController.Lib.DiscordIPC.Entities;
using NovationController.Lib.DiscordIPC.Event;
using NovationController.Lib.Integration.Discord;
using RestSharp;

namespace NovationController.Lib.DiscordIPC
{
public sealed class IpcClient : IDisposable
    {
        private const int VERSION = 1;
        private const int CORRUPT_READ = 2;  // 2 is the constant used in the official version so that's what we'll use for consistency.
        private readonly ulong _appId;

        private RestClient _restClient = new RestClient("https://discord.com/api");
        private DiscordConfig _discordConfig;
        private DiscordAuthConfig _discordAuthConfig;

        private IpcSocket _ipc;
        private Thread _readThread;
        private CancellationTokenSource _readThreadCancel;

        public IpcClient(ulong applicationId)
        {
            _appId = applicationId;
        }

        #region Events
        
        public event EventHandler<ReadyEvent> OnReady;
        
        public event EventHandler<PacketEventArgs> OnPacketSent;
        
        public event EventHandler<AuthorizeEvent> OnAuthorize;
        
        public event EventHandler<VoiceSettingsUpdateEvent> OnVoiceSettingsUpdate;
        
        public event EventHandler<PacketEventArgs> OnPacketRecieved;

        public event EventHandler<IpcErrorEventArgs> OnError;
        
        public event EventHandler<IpcErrorEventArgs> OnDisconnect;

        #endregion

        public enum State
        {
            /// <summary>
            /// The client has been created and has yet to connect.
            /// </summary>
            New,

            /// <summary>
            /// The client is in the process of connecting.
            /// </summary>
            Connecting,

            /// <summary>
            /// The client is connected.
            /// </summary>
            Connected,

            /// <summary>
            /// The client has been disconnected due to an error.
            /// </summary>
            Disconnected,

            /// <summary>
            /// The client has safely and normally shut down.
            /// </summary>
            Closed
        }
        
        public State ClientState { get; private set; }

        public void Init(DiscordConfig discordConfig, DiscordAuthConfig discordAuthConfig)
        {
            _ipc = new IpcSocket("\\\\?\\pipe\\discord-ipc-0");
            _readThreadCancel = new CancellationTokenSource();
            _discordConfig = discordConfig;

            StartReadThread();
            Handshake();
            Auth(discordAuthConfig);
        }

        public void Handshake()
        {
            try
            {
                var obj = new JObject
                {
                    {"v", VERSION},
                    {"client_id", _appId.ToString()}
                };


                _ipc.Write(OpCode.Handshake, obj);

                var packet = _ipc.Read();

                var data = packet.Payload["data"] as JObject;
                var config = data["config"] as JObject;

                var endpoint = config["api_endpoint"].ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Close()
        {
            if (ClientState != State.Connected)
            {
                throw new InvalidOperationException("Can only close while connected.");
            }

            // Notify the read thread to shut down.
            _readThreadCancel.Cancel();
            _readThreadCancel = null;

            // We can remove the only reference to the read thread as it won't be garbage collected until it's done running.
            _readThread = null;

            // Dispose the object used to communicate with the client.
            _ipc.Dispose();
            _ipc = null;

            ClientState = State.Disconnected;
        }

        public void ToggleMuteStatus(bool status)
        {
            var jObject = new JObject
            {
                {"cmd", "SET_VOICE_SETTINGS"}, 
                {"args", new JObject {{"mute", status}}}
            };

            _ipc.Write(OpCode.Frame, jObject);
        }
        
        public void ToggleDeafStatus(bool status)
        {
            var jObject = new JObject
            {
                {"cmd", "SET_VOICE_SETTINGS"}, 
                {"args", new JObject {{"deaf", status}}}
            };

            _ipc.Write(OpCode.Frame, jObject);
        }

        public void Subscribe(string name)
        {
            var jObject = new JObject
            {
                {"cmd", "SUBSCRIBE"},
                {"evt", name}
            };
            
            _ipc.Write(OpCode.Frame, jObject);
        }
        
        public void Auth(DiscordAuthConfig discordAuthConfig)
        {
            if (discordAuthConfig != null)
            {
                Authenticate(discordAuthConfig);
            }
            else
            {
                Authorize();
            }
        }
        
        public void Authorize()
        {
            var jObject = new JObject
            {
                {"cmd", "AUTHORIZE"},
                {"args", new JObject
                {
                    {"client_id", _appId.ToString()},
                    {"scopes", new JArray
                    {
                        "identify",
                        "rpc"
                    }}
                }}
            };
            _ipc.Write(OpCode.Frame, jObject);
        }

        public void Authenticate(DiscordAuthConfig discordAuthConfig)
        {
            _discordAuthConfig = discordAuthConfig;
            var jObject = new JObject
            {
                {"cmd", "AUTHENTICATE"},
                {"args", new JObject {{"access_token", discordAuthConfig.AccessToken}}}
            };
            
            _ipc.Write(OpCode.Frame, jObject);
        }

        public void Oauth(string token)
        {
            var req = new RestRequest("/oauth2/token", Method.POST);

            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            req.AddParameter("client_id", _discordConfig.ApplicationId);
            req.AddParameter("client_secret", _discordConfig.Secret);
            req.AddParameter("grant_type", "authorization_code");
            req.AddParameter("code", token);
            req.AddParameter("redirect_uri", "http://127.0.0.1");
            req.AddParameter("scope", "identify rpc");
                
            var res = _restClient.Post(req);

            var authConfig = JsonConvert.DeserializeObject<DiscordAuthConfig>(res.Content);
            
            OnAuthorize(this, new AuthorizeEvent(authConfig));
            
            Authenticate(authConfig);
        }

        public void OAuthRefresh(string refreshToken)
        {
            var req = new RestRequest("/oauth2/token", Method.POST);

            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            req.AddParameter("client_id", _discordConfig.ApplicationId);
            req.AddParameter("client_secret", _discordConfig.Secret);
            req.AddParameter("grant_type", "refresh_token");
            req.AddParameter("refresh_token", refreshToken);

            var res = _restClient.Post(req);

            var authConfig = JsonConvert.DeserializeObject<DiscordAuthConfig>(res.Content);
            
            Authenticate(authConfig);
        }

        private void StartReadThread()
        {
            _readThread = new Thread(token =>
            {
                // Unbox the cancellation token as ParameterizedThreadStart only takes an object.
                var cancellationToken = (CancellationToken)token;

                bool shouldBreak = false;
                while (!shouldBreak)
                {
                    try
                    {
                        // Throw if the read thread should shut down so that it can clean up.
                        cancellationToken.ThrowIfCancellationRequested();

                        // Only read if data is available because otherwise Read will block.
                        if (_ipc.IsDataAvailable)
                        {
                            // Read data and raise the packet recieved event.
                            IpcPacket packet = _ipc.Read();

                            OnPacketRecieved?.Invoke(this, new PacketEventArgs(packet));

                            switch (packet.OpCode)
                            {
                                case OpCode.Frame:
                                    // Frame is the opcode for normal data transfer.
                                    var evt = (string)packet.Payload["evt"];
                                    if (evt != null)
                                    {
                                        HandleEvent(evt, (JObject)packet.Payload["data"]);
                                        break;
                                    }
                                    
                                    var cmd = (string) packet.Payload["cmd"];
                                    if (cmd != null)
                                    {
                                        HandleCommand(cmd, (JObject) packet.Payload["data"]);
                                        break;
                                    }

                                    break;

                                case OpCode.Close:
                                    // Close means something has gone wrong and the discord client has severed the connection.
                                    Close();
                                    var closeData = packet.Payload["data"];
                                    OnDisconnect(this, new IpcErrorEventArgs((int)closeData["code"], (string)closeData["message"]));
                                    break;

                                case OpCode.Ping:
                                    // When we're pinged, pong with the same data.
                                    _ipc.Write(OpCode.Pong, packet.Payload, addNonce: false);
                                    break;

                                case OpCode.Pong:
                                    // The official version has a case for Pong but doesn't actually do anything so that's what we'll do.
                                    break;

                                default:
                                    // Bad data was recieved, so we safely close the connection and notify the user.
                                    Close();
                                    OnDisconnect(this, new IpcErrorEventArgs(CORRUPT_READ, "The IPC frame data is corrupt."));
                                    break;
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        shouldBreak = true;
                    }

                    // Wait 50ms before the next iteration.
                    Thread.Sleep(50);
                }
            });

            _readThread.Start(_readThreadCancel.Token);
        }

        private void HandleEvent(string evt, JObject data)
        {
            switch (evt)
            {
                case "ERROR":
                    if (int.Parse(data["code"].ToString()) == 4009)
                    {
                        OAuthRefresh(_discordAuthConfig.RefreshToken);
                    }
                    else
                    {
                        OnError?.Invoke(this, new IpcErrorEventArgs((int) data["code"], (string) data["message"]));
                    }
                    break;

                case "VOICE_SETTINGS_UPDATE":
                {
                    OnVoiceSettingsUpdate?.Invoke(this, new VoiceSettingsUpdateEvent((bool)data["mute"], (bool)data["deaf"]));
                    break;
                }

                default:
                    break;
            }
        }

        private void HandleCommand(string command, JObject data)
        {
            switch (command)
            {
                case "AUTHORIZE":
                {
                    var token = data["code"].ToString();
                    Oauth(token);
                    break;
                }
                
                case "AUTHENTICATE":
                {
                    SetReady();
                    break;
                }
            }
        }
        
        private void SetReady()
        {
            OnReady?.Invoke(this, new ReadyEvent());
            ClientState = State.Connected;
        }

        public void Dispose()
        {
            
        }
    }
}
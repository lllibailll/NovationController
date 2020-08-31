using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using NovationController.Lib.DiscordIPC.Entities;

namespace NovationController.Lib.DiscordIPC
{
internal sealed class IpcSocket : IDisposable
    {
        private Stream _ipc;
        private string _path;
        private bool _disposed;

        public IpcSocket(string path)
        {
            _path = path;

            var fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
            _ipc = Stream.Synchronized(fs);
        }

        /// <summary>
        /// Returns whether or not data is available to be read.
        /// </summary>
        public bool IsDataAvailable => _ipc.Length > 0;

        /// <summary>
        /// Reads data from the pipe.
        /// </summary>
        /// <returns>An <see cref="IpcPacket"/> created from the recieved data.</returns>
        public IpcPacket Read()
        {
            // Read the opcode and data length into byte buffers.
            byte[] opcodeBytes = new byte[4];
            _ipc.Read(opcodeBytes, 0, 4);

            byte[] lengthBytes = new byte[4];
            _ipc.Read(lengthBytes, 0, 4);

            // If the system is big endian, reverse the byte arrays.
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(opcodeBytes);
                Array.Reverse(lengthBytes);
            }

            OpCode opCode = (OpCode)BitConverter.ToInt32(opcodeBytes, 0);
            int length = BitConverter.ToInt32(lengthBytes, 0);

            byte[] data = new byte[length];
            _ipc.Read(data, 0, length);

            return new IpcPacket(opCode, JObject.Parse(Encoding.Default.GetString(data)));
        }

        /// <summary>
        /// Writes data to the pipe.
        /// </summary>
        /// <param name="opCode">The IPC opcode.</param>
        /// <param name="data">The data to write.</param>
        /// <returns>The <see cref="IpcPacket"/> that was written.</returns>
        public IpcPacket Write(OpCode opCode, JObject data, bool addNonce = true)
        {
            if (addNonce)
            {
                data.Add("nonce", new JValue(Guid.NewGuid()));
            }

            var p = new IpcPacket(opCode, data);
            var payload = p.ToBytes();
            _ipc.Write(payload, 0, payload.Length);

            return p;
        }

        public void Dispose()
        {
            // TODO: Flesh out disposal
            _ipc.Dispose();
            _ipc = null;
        }
    }
}
using System;
using Newtonsoft.Json.Linq;

namespace NovationController.Lib.DiscordIPC.Entities
{
public sealed class RichPresenceBuilder
    {
        private string _state;
        private string _details;
        private long _startTimestamp;
        private long _endTimestamp;
        private string _largeImageKey;
        private string _largeImageText;
        private string _smallImageKey;
        private string _smallImageText;
        private string _partyId;  
        private int _partySize;
        private int _partyMax;
        private string _matchSecret;
        private string _joinSecret;
        private string _spectateSecret;
        private bool _instance;

        /// <summary>
        /// Sets the user's current state.
        /// </summary>
        /// <param name="state">The user's current state</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithState(string state)
        {
            _state = state;
            return this;
        }

        /// <summary>
        /// Sets what the user is currently doing.
        /// </summary>
        /// <param name="details">What the user is currently doing.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithDetails(string details)
        {
            _details = details;
            return this;
        }

        /// <summary>
        /// Sets the start timestamp of the user's game.
        /// </summary>
        /// <param name="startTimestamp">The start timestamp of the user's game.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithStartTimestamp(DateTimeOffset startTimestamp)
        {
            _startTimestamp = startTimestamp.ToUnixTimeSeconds();
            return this;
        }

        /// <summary>
        /// Sets the end timestamp of the user's game.
        /// </summary>
        /// <param name="endTimestamp">The end timestamp of the user's game.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithEndTimestamp(DateTimeOffset endTimestamp)
        {
            _endTimestamp = endTimestamp.ToUnixTimeSeconds();
            return this;
        }

        /// <summary>
        /// Sets the large image and its mouseover text.
        /// </summary>
        /// <param name="largeImageKey">The name of the large image asset.</param>
        /// <param name="largeImageText">The text to display when mousing over the image.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithLargeImage(string largeImageKey, string largeImageText = null)
        {
            _largeImageKey = largeImageKey;
            _largeImageText = largeImageText;
            return this;
        }

        /// <summary>
        /// Sets the small image and its mouseover text.
        /// </summary>
        /// <param name="smallImageKey">The name of the small image asset.</param>
        /// <param name="smallImageText">The text to display when mousing over the image.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithSmallImage(string smallImageKey, string smallImageText = null)
        {
            _smallImageKey = smallImageKey;
            _smallImageText = smallImageText;
            return this;
        }

        /// <summary>
        /// Sets the user's party status.
        /// </summary>
        /// <param name="partyId">The ID of the party.</param>
        /// <param name="partySize">The current number of party members.</param>
        /// <param name="partyMax">The maximum number of party members.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithParty(string partyId, int partySize, int partyMax)
        {
            _partyId = partyId;
            _partySize = partySize;
            _partyMax = partyMax;
            return this;
        }

        /// <summary>
        /// Sets the match secret.
        /// </summary>
        /// <param name="matchSecret">A unique hashed string used for spectating and joining.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithMatchSecret(string matchSecret)
        {
            _matchSecret = matchSecret;
            return this;
        }

        /// <summary>
        /// Sets the join secret.
        /// </summary>
        /// <param name="joinSecret">A unique hashed string used for chat invitations and join requests.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithJoinSecret(string joinSecret)
        {
            _joinSecret = joinSecret;
            return this;
        }

        /// <summary>
        /// Sets the spectate secret.
        /// </summary>
        /// <param name="spectateSecret">A unique hashed string used for spectate requests.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithSpectateSecret(string spectateSecret)
        {
            _spectateSecret = spectateSecret;
            return this;
        }

        /// <summary>
        /// Sets whether or not the game is a session with a specific start and end.
        /// </summary>
        /// <param name="isInstance">Whether or not the game is a session with a specific start and end.</param>
        /// <returns>The builder.</returns>
        public RichPresenceBuilder WithIsInstance(bool isInstance)
        {
            _instance = isInstance;
            return this;
        }

        /// <summary>
        /// Creates a <see cref="JObject"/> from the builder's set values.
        /// </summary>
        /// <returns>A rich presence <see cref="JObject"/>.</returns>
        internal JObject Build()
        {
            var json =
                new JObject(
                    new JProperty("state", _state),
                    new JProperty("details", _details),
                    new JProperty("timestamps",
                        new JObject(
                            new JProperty("start", _startTimestamp == 0 ? null : (long?)_startTimestamp),
                            new JProperty("end", _endTimestamp == 0 ? null : (long?)_endTimestamp))),
                    new JProperty("assets",
                        new JObject(
                            new JProperty("large_image", _largeImageKey),
                            new JProperty("large_text", _largeImageText),
                            new JProperty("small_image", _smallImageKey),
                            new JProperty("small_text", _smallImageText))),
                    new JProperty("party",
                        new JObject(
                            new JProperty("id", _partyId),
                            new JProperty("size", new JArray(_partySize, _partyMax)))),
                    new JProperty("secrets", 
                        new JObject(
                            new JProperty("join", _joinSecret),
                            new JProperty("spectate", _spectateSecret),
                            new JProperty("match", _matchSecret))),
                    new JProperty("instance", _instance));

            return json;
        }
    }
}
namespace NovationController.Lib.DiscordIPC.Entities
{
    /// <summary>
    /// Represents a Discord user, used for join requests.
    /// </summary>
    public sealed class User
    {
        private readonly string[] DEFAULT_AVATARS = new string[]
        {
            "6debd47ed13483642cf09e832ed0bc1b", // Blurple
            "322c936a8c8be1b803cd94861bdfa868", // Grey
            "dd4dbc0016779df1378e7812eabaa04d", // Green
            "0e291f67c9274a1abdddeb3fd919cbaa", // Orange
            "1cbd08c76f8af6dddce02c5138971129"  // Red
        };

        /// <summary>
        /// The user's username.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The user's discriminator.
        /// </summary>
        public string Discriminator { get; private set; }

        /// <summary>
        /// The user's unique ID.
        /// </summary>
        public ulong Id { get; private set; }

        /// <summary>
        /// The user's avatar URL.
        /// </summary>
        public string AvatarUrl
        {
            get
            {
                if (_avatar != null)
                {
                    return $"https://cdn.discordapp.com/avatars/{Id}/{_avatar}.{(_avatar.StartsWith("a_") ? "gif" : "png")}";
                }
                else
                {
                    var defaultAvatarId = DEFAULT_AVATARS[int.Parse(Discriminator) % DEFAULT_AVATARS.Length];
                    return $"https://discordapp.com/assets/{defaultAvatarId}.png";
                }
            }
        }

        private string _avatar;

        internal User(string name, string discrim, ulong id, string avatar)
        {
            Name = name;
            Discriminator = discrim;
            Id = id;
            _avatar = avatar;
        }
    }
}
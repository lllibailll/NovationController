using System;

namespace NovationController.Lib.DiscordIPC.Event
{
    public sealed class VoiceSettingsUpdateEvent : EventArgs
    {
        public bool Mute { get; }
        
        public bool Deaf { get; }

        public VoiceSettingsUpdateEvent(bool mute, bool deaf)
        {
            Mute = mute;
            Deaf = deaf;
        }
    }
}
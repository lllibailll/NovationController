using System;

namespace Lib.DiscordIPC.Entities
{
    public sealed class PacketEventArgs : EventArgs
    {
        public IpcPacket Packet { get; private set; }

        public PacketEventArgs(IpcPacket packet)
        {
            Packet = packet;
        }
    }
}
using System;

namespace Lib.DiscordIPC.Entities
{
    public class IpcException : Exception
    {
        public IpcException()
        {
        }

        public IpcException(string message) : base(message)
        {
        }

        public IpcException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
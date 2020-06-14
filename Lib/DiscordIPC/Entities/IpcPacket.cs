using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Lib.DiscordIPC.Entities
{
public sealed class IpcPacket
    {
        /// <summary>
        /// The packet's <see cref="Entities.OpCode"/>.
        /// </summary>
        public OpCode OpCode { get; private set; }

        /// <summary>
        /// The packet's JSON data.
        /// </summary>
        public JObject Payload { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="IpcPacket"/>.
        /// </summary>
        /// <param name="opcode">The packet's opcode.</param>
        /// <param name="data">The packet's JSON payload.</param>
        public IpcPacket(OpCode opcode, JObject data)
        {
            OpCode = opcode;
            Payload = data;
        }

        /// <summary>
        /// Converts the <see cref="IpcPacket"/> to a byte array.
        /// </summary>
        /// <returns>A byte array containing the opcode, the payload length, and the payload itself.</returns>
        internal byte[] ToBytes()
        {
            // Convert the JSON payload to bytes using the default system encoding.
            byte[] payloadBytes = Encoding.Default.GetBytes(Payload.ToString());

            // Convert the opcode and payload length into bytes.
            byte[] opcodeBytes = BitConverter.GetBytes((int)OpCode);
            byte[] lengthBytes = BitConverter.GetBytes(payloadBytes.Length);
            // If the system is big endian, convert the byte arrays to little endian.
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(opcodeBytes);
                Array.Reverse(lengthBytes);
            }

            // Allocate space for the entire payload plus 2 32-bit integers.
            byte[] buffer = new byte[payloadBytes.Length + 8];
            // Copy the 3 arrays to the buffer.
            Array.Copy(opcodeBytes, 0, buffer, 0, 4);
            Array.Copy(lengthBytes, 0, buffer, 4, 4);
            Array.Copy(payloadBytes, 0, buffer, 8, payloadBytes.Length);

            return buffer;
        }
    }

    public enum OpCode
    {
        Handshake,
        Frame,
        Close,
        Ping,
        Pong
    }
}
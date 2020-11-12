using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace SSHAgentFramework
{
    // https://tools.ietf.org/id/draft-miller-ssh-agent-01.html
    // https://tools.ietf.org/html/rfc4251
    public class AgentMessage : IAgentMessage
    {
        public uint Length
        {
            get { return (uint)(Contents.Length + 1); }
        }

        public byte RawType;
        public AgentMessageType Type
        {
            get { return (AgentMessageType)RawType; }
            set { RawType = (byte)value; }
        }
        public byte[] Contents;

        public AgentMessage ToAgentMessage()
        {
            return this;
        }

        public byte[] Serialize()
        {
            var buff = new Span<byte>(new byte[sizeof(uint) + sizeof(byte) + Contents.Length]);
            int index = 0;
            if (!WireUtils.TryWriteUintToWire(buff[index..(index + sizeof(uint))], Length))
            {
                throw new Exception();
            }
            index += sizeof(uint);
            buff[index] = RawType;
            index++;
            Contents.CopyTo(buff[index..]);
            return buff.ToArray();
        }

        public static AgentMessage Deserialize(byte[] incomingBuff)
        {
            var message = new AgentMessage();
            int index = 0;
            var length = WireUtils.ReadUintFromWire(incomingBuff[index..(index + sizeof(uint))]);
            index += sizeof(uint);
            message.RawType = incomingBuff[index];
            index++;
            if (incomingBuff.Length != sizeof(uint) + 1 + length - 1)
            {
                throw new InvalidDataException();
            }
            message.Contents = incomingBuff[index..].ToArray();
            return message;
        }
    }
}

using SSHAgentFramework;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloSSH
{
    class EncodedSSHPublicKey
    {
        public const string KEY_TYPE_RSA = "ssh-rsa";
        public string KeyType;
        public byte[] ExponentOrECTypeName;
        public byte[] ModulusOrECPoint;

        public string Serialize()
        {
            var ret = new StringBuilder();//(KeyType);
            //ret.Append(" ");
            var buff = SerializeBuffer(Encoding.ASCII.GetBytes(KeyType))
                .Concat(SerializeBuffer(ExponentOrECTypeName))
                .Concat(SerializeBuffer(ModulusOrECPoint))
                .ToArray();
            ret.Append(Convert.ToBase64String(buff));
            return ret.ToString();
        }

        private static byte[] SerializeBuffer(byte[] buffer)
        {
            var ret = new Span<byte>(new byte[4 + buffer.Length]);
            WireUtils.TryWriteUintToWire(ret, (uint)buffer.Length);
            buffer.CopyTo(ret[4..]);
            return ret.ToArray();
        }
    }
}

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
        public const string KEY_TYPE_RSA_SHA256 = "rsa-sha2-256";
        public string KeyType;
        public byte[] ExponentOrECTypeName;
        public byte[] ModulusOrECPoint;

        public byte[] Serialize()
        {
            //this only works for RSA but that's ok for now 
            var buff = WireUtils.EncodeString(KeyType)
                .Concat(WireUtils.EncodeToMPInt(ExponentOrECTypeName))
                .Concat(WireUtils.EncodeToMPInt(ModulusOrECPoint))
                .ToArray();
            return WireUtils.EncodeString(buff);
        }
    }
}

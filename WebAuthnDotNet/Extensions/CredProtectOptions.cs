using System;
using System.Collections.Generic;
using System.Text;
using WebAuthnDotNet.Internal;

namespace WebAuthnDotNet.Extensions
{
    public class CredProtectOptions : IExtension<CredProtectType>
    {
        public CredProtectType CredProtectType { get; set; }
        public bool Required { get; set; }

        public string ExtensionIdentifier => ExtensionType.CredProtect.SerializedValue;

        public CredProtectType MarshalFromBytes(byte[] bytes)
        {
            if (bytes.Length != sizeof(uint))
            {
                throw new ArgumentException($"Expected {sizeof(uint)} bytes, got {bytes.Length}!");
            }
            return (CredProtectType)(int)(BitConverter.ToUInt32(bytes, 0));
        }

        public byte[] MarshalToBytes()
        {
            var outStruct = new Internal.RawCredProtectExtensionIn(this);
            return Util.MarshalToBytes(outStruct);
        }
    }
}

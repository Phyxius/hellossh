using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet.Extensions
{
    public class HmacSecretOptions : IExtension<bool>
    {
        
        public bool HmacSecretProtected { get; set; } = true;

        public string ExtensionIdentifier => ExtensionType.HmacSecret.SerializedValue;

        public bool MarshalFromBytes(byte[] bytes)
        {
            if (bytes.Length != sizeof(uint))
            {
                throw new ArgumentException($"Expected {sizeof(uint)} bytes, got {bytes.Length}!");
            }
            return BitConverter.ToInt32(bytes, 0) != 0;
        }

        public byte[] MarshalToBytes()
        {
            return BitConverter.GetBytes(HmacSecretProtected ? 1 : 0);
        }
    }
}

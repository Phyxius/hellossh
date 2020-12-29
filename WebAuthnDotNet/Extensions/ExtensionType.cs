using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet.Extensions
{
    public class ExtensionType
    {
        public string SerializedValue { get; private set; }
        private ExtensionType(string serializedValue)
        {
            SerializedValue = serializedValue;
        }

        public readonly static ExtensionType HmacSecret = new ExtensionType("hmac-secret");
        public readonly static ExtensionType CredProtect = new ExtensionType("credProtect");
    }
}

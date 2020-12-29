using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public class CredentialType
    {
        private const string PublicKeyType = "public-key";
        public string SerializedValue { get; private set; }
        private CredentialType(string serializedValue)
        {
            SerializedValue = serializedValue;
        }

        public readonly static CredentialType PublicKey = new CredentialType(PublicKeyType);

        public static CredentialType Deserialize(string serializedValue)
        {
            switch (serializedValue)
            {
                case PublicKeyType: return PublicKey;
                default: throw new ArgumentException($"Unknown credential type {serializedValue}");
            }
        }
    }
}

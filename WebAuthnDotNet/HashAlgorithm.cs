using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public class HashAlgorithm
    {
        public string SerializedValue { get; private set; }
        private HashAlgorithm(string serializedValue)
        {
            SerializedValue = serializedValue;
        }

        public readonly static HashAlgorithm Sha256 = new HashAlgorithm("SHA-256");
        public readonly static HashAlgorithm Sha384 = new HashAlgorithm("SHA-384");
        public readonly static HashAlgorithm Sha512 = new HashAlgorithm("SHA-512");
    }
}

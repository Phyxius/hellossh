using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public class CoseCredentialParameter
    {
        public const int CurrentVersion = 1;
        public int Version { get; set; } = CurrentVersion;
        public CredentialType CredentialType { get; set; } = CredentialType.PublicKey;
        public CoseAlgorithm CoseAlgorithm { get; set; }
    }
}

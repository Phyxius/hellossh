using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public class Credential
    {
        public const int CurrentVersion = 1;
        public int Version { get; set; } = CurrentVersion;
        public byte[] Id;
        public CredentialType CredentialType;
    }
}

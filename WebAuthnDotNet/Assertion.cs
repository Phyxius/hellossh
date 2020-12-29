using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public class Assertion
    {
        public const int CurrentVersion = 1;
        public int Version { get; set; }
        public byte[] AuthenticatorData { get; set; }
        public byte[] Signature { get; set; }
        public Credential Credential { get; set; }
        public byte[] UserId { get; set; }
    }
}

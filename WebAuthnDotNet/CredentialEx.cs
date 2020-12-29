using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public class CredentialEx : Credential
    {
        public CtapTransport Transport { get; set; }
    }
}

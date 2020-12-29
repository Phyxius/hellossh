using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public enum UserVerificationRequirement
    {
        Any = 0,
        Required = 1,
        Preferred = 2,
        Discouraged = 3
    }
}

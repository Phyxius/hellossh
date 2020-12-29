using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet.Extensions
{
    public enum CredProtectType : uint
    {
        UserVerificationAny = 0,
        UserVerificationOptional = 1,
        UserVerificationOptionalWithCredentialIDList = 2,
        UserVerificationRequired = 3
    }
}

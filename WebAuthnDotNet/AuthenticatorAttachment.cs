using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public enum AuthenticatorAttachment
    {
        Any = 0,
        Platform = 1,
        CrossPlatform = 2,
        CrossPlatformU2fV2 = 3
    }
}

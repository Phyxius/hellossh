using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public enum AttestationConveyancePreference
    {
        Any = 0,
        None = 1,
        Indirect = 2,
        Direct = 3
    }
}

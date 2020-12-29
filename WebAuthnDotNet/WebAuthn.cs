using System;
using System.Collections.Generic;
using System.Text;
using WebAuthnDotNet.Internal;

namespace WebAuthnDotNet
{
    public static class WebAuthn
    {
        public static int ApiVersion => (int)NativeMethods.WebAuthNGetApiVersionNumber();
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    internal static class NativeMethods
    {
        [DllImport("webauthn.dll")]
        public extern static uint WebAuthNGetApiVersionNumber();
    }
}

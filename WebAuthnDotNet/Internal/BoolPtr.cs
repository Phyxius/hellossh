using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BoolPtr
    {
        public bool BooleanValue { get; set; }
    }
}

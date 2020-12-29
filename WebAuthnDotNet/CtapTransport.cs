using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    [Flags]
    public enum CtapTransport : uint
    {
        Usb = 0x00000001,
        Nfc = 0x00000002,
        Ble = 0x00000004,
        Test = 0x00000008,
        Internal = 0x00000010,
        FlagsMask = 0x0000001F
    }
}

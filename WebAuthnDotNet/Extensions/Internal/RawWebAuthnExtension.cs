using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Extensions.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Information about Extensions.
    //-------------------------------------------------------------------------------------------
    typedef struct _WEBAUTHN_EXTENSION {
        LPCWSTR pwszExtensionIdentifier;
        DWORD cbExtension;
        PVOID pvExtension;
    } WEBAUTHN_EXTENSION, *PWEBAUTHN_EXTENSION;
    typedef const WEBAUTHN_EXTENSION *PCWEBAUTHN_EXTENSION;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawWebAuthnExtension
    {
        string pwszExtensionIdentifier { get; set; }
        uint cbExtension { get; set; }
        byte[] pvExtension { get; set; }

        public RawWebAuthnExtension(IExtension<object> template)
        {
            pwszExtensionIdentifier = template.ExtensionIdentifier;
            pvExtension = template.MarshalToBytes();
            cbExtension = (uint)pvExtension.Length;
        }
    }
}

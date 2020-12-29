using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebAuthnDotNet.Extensions.Internal
{
    /*
    typedef struct _WEBAUTHN_EXTENSIONS {
        DWORD cExtensions;
        _Field_size_(cExtensions)
        PWEBAUTHN_EXTENSION pExtensions;
    } WEBAUTHN_EXTENSIONS, *PWEBAUTHN_EXTENSIONS;
    typedef const WEBAUTHN_EXTENSIONS *PCWEBAUTHN_EXTENSIONS;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawWebAuthnExtensions
    {
        uint cExtenstions { get; set; }
        RawWebAuthnExtension[] pExtensions { get; set; }

        public RawWebAuthnExtensions(IEnumerable<RawWebAuthnExtension> extensions)
        {
            pExtensions = (RawWebAuthnExtension[])extensions.ToArray().Clone();
            cExtenstions = (uint)pExtensions.Length;
        }

        public RawWebAuthnExtensions(IEnumerable<IExtension<object>> extensions)
            : this(extensions.Select(e => new RawWebAuthnExtension(e)))
        {

        }
    }
}

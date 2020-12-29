using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    /*
    typedef struct _WEBAUTHN_X5C {
        // Length of X.509 encoded certificate
        DWORD cbData;
        // X.509 encoded certificate bytes
        _Field_size_bytes_(cbData)
        PBYTE pbData;
    } WEBAUTHN_X5C, *PWEBAUTHN_X5C;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawX5C
    {
        public uint cbData { get; set; }
        public byte[] pbData { get; set; }

        public X509Certificate ToX509Certificate()
        {
            return new X509Certificate(pbData);
        }
    }
}

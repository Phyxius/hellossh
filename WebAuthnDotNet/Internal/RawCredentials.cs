using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebAuthnDotNet.Internal
{
    /*
    typedef struct _WEBAUTHN_CREDENTIALS {
        DWORD cCredentials;
        _Field_size_(cCredentials)
        PWEBAUTHN_CREDENTIAL pCredentials;
    } WEBAUTHN_CREDENTIALS, *PWEBAUTHN_CREDENTIALS;
    typedef const WEBAUTHN_CREDENTIALS *PCWEBAUTHN_CREDENTIALS;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCredentials
    {
        uint cCredentials;
        RawCredential[] pCredentials;

        public RawCredentials(IEnumerable<RawCredential> rawCredentials)
        {
            pCredentials = (RawCredential[])rawCredentials.ToArray().Clone();
            cCredentials = (uint)pCredentials.Length;
        }

        public RawCredentials(IEnumerable<Credential> credentials)
            : this(credentials.Select((c) => new RawCredential(c)))
        {

        }
    }
}

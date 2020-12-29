using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Information about credential list with extra information
    //-------------------------------------------------------------------------------------------

    typedef struct _WEBAUTHN_CREDENTIAL_LIST {
        DWORD cCredentials;
        _Field_size_(cCredentials)
        PWEBAUTHN_CREDENTIAL_EX *ppCredentials;
    } WEBAUTHN_CREDENTIAL_LIST, *PWEBAUTHN_CREDENTIAL_LIST;
    typedef const WEBAUTHN_CREDENTIAL_LIST *PCWEBAUTHN_CREDENTIAL_LIST;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCredentialList
    {
        uint cCredentials { get; set; }
        CredentialEx[] ppCredentials { get; set; }

        public RawCredentialList(IEnumerable<RawCredentialEx> rawCredentials)
        {
            ppCredentials = (CredentialEx[])rawCredentials.ToArray().Clone();
            cCredentials = (uint)ppCredentials.Length;
        }

        public RawCredentialList(IEnumerable<CredentialEx> credentials)
            : this(credentials.Select(c => new RawCredentialEx(c)))
        {

        }
    }
}

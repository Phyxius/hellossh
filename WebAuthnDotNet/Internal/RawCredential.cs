using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Information about credential.
    //-------------------------------------------------------------------------------------------
    #define WEBAUTHN_CREDENTIAL_CURRENT_VERSION                         1

    typedef struct _WEBAUTHN_CREDENTIAL {
        // Version of this structure, to allow for modifications in the future.
        DWORD dwVersion;

        // Size of pbID.
        DWORD cbId;
        // Unique ID for this particular credential.
        _Field_size_bytes_(cbId)
        PBYTE pbId;

        // Well-known credential type specifying what this particular credential is.
        LPCWSTR pwszCredentialType;
    } WEBAUTHN_CREDENTIAL, *PWEBAUTHN_CREDENTIAL;
    typedef const WEBAUTHN_CREDENTIAL *PCWEBAUTHN_CREDENTIAL;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCredential
    {
        uint dwVersion { get; set; }
        uint cbId { get; set; }
        byte[] pbId { get; set; }
        string pwszCredentialType { get; set; }

        public RawCredential(Credential template)
        {
            dwVersion = (uint)template.Version;
            pbId = (byte[])template.Id.Clone();
            cbId = (uint)pbId.Length;
            pwszCredentialType = template.CredentialType.SerializedValue;
        }

        public Credential ToCredential()
        {
            return new Credential
            {
                Version = (int)dwVersion,
                Id = (byte[])pbId.Clone(),
                CredentialType = CredentialType.Deserialize(pwszCredentialType)
            };
        }
    }
}

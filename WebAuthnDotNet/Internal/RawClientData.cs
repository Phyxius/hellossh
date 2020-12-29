using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Information about client data.
    //-------------------------------------------------------------------------------------------

    #define WEBAUTHN_HASH_ALGORITHM_SHA_256                         L"SHA-256"
    #define WEBAUTHN_HASH_ALGORITHM_SHA_384                         L"SHA-384"
    #define WEBAUTHN_HASH_ALGORITHM_SHA_512                         L"SHA-512"

    #define WEBAUTHN_CLIENT_DATA_CURRENT_VERSION                    1

    typedef struct _WEBAUTHN_CLIENT_DATA {
        // Version of this structure, to allow for modifications in the future.
        // This field is required and should be set to CURRENT_VERSION above.
        DWORD dwVersion;

        // Size of the pbClientDataJSON field.
        DWORD cbClientDataJSON;
        // UTF-8 encoded JSON serialization of the client data.
        _Field_size_bytes_(cbClientDataJSON)
        PBYTE pbClientDataJSON;

        // Hash algorithm ID used to hash the pbClientDataJSON field.
        LPCWSTR pwszHashAlgId;
    } WEBAUTHN_CLIENT_DATA, *PWEBAUTHN_CLIENT_DATA;
    typedef const WEBAUTHN_CLIENT_DATA *PCWEBAUTHN_CLIENT_DATA;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawClientData
    {
        public uint dwVersion { get; set; }
        public uint cbClientDataJSON { get; set; }
        public byte[] pbClientDataJSON { get; set; }
        public string pwszHashAlgId { get; set; }
        public RawClientData(ClientData template)
        {
            dwVersion = (uint)template.Version;
            pbClientDataJSON = Encoding.UTF8.GetBytes(template.JSONClientData);
            cbClientDataJSON = (uint)pbClientDataJSON.Length;
            pwszHashAlgId = template.HashAlgorithm.SerializedValue;
        }
    }
}

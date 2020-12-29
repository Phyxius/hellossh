using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Information about credential with extra information, such as, dwTransports
    //-------------------------------------------------------------------------------------------

    #define WEBAUTHN_CTAP_TRANSPORT_USB         0x00000001
    #define WEBAUTHN_CTAP_TRANSPORT_NFC         0x00000002
    #define WEBAUTHN_CTAP_TRANSPORT_BLE         0x00000004
    #define WEBAUTHN_CTAP_TRANSPORT_TEST        0x00000008
    #define WEBAUTHN_CTAP_TRANSPORT_INTERNAL    0x00000010
    #define WEBAUTHN_CTAP_TRANSPORT_FLAGS_MASK  0x0000001F

    #define WEBAUTHN_CREDENTIAL_EX_CURRENT_VERSION                         1

    typedef struct _WEBAUTHN_CREDENTIAL_EX {
        // Version of this structure, to allow for modifications in the future.
        DWORD dwVersion;

        // Size of pbID.
        DWORD cbId;
        // Unique ID for this particular credential.
        _Field_size_bytes_(cbId)
        PBYTE pbId;

        // Well-known credential type specifying what this particular credential is.
        LPCWSTR pwszCredentialType;

        // Transports. 0 implies no transport restrictions.
        DWORD dwTransports;
    } WEBAUTHN_CREDENTIAL_EX, *PWEBAUTHN_CREDENTIAL_EX;
    typedef const WEBAUTHN_CREDENTIAL_EX *PCWEBAUTHN_CREDENTIAL_EX;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCredentialEx
    {
        uint dwVersion { get; set; }
        uint cbId { get; set; }
        byte[] pbId { get; set; }
        string pwszCredentialType { get; set; }
        uint dwTransports { get; set; }

        public RawCredentialEx(CredentialEx template)
        {
            dwVersion = (uint)template.Version;
            pbId = (byte[])template.Id.Clone();
            cbId = (uint)pbId.Length;
            pwszCredentialType = template.CredentialType.SerializedValue;
            dwTransports = (uint)template.Transport;
        }
    }
}

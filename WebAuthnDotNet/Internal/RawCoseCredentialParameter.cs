using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Information about credential parameters.
    //-------------------------------------------------------------------------------------------

    #define WEBAUTHN_CREDENTIAL_TYPE_PUBLIC_KEY                         L"public-key"

    #define WEBAUTHN_COSE_ALGORITHM_ECDSA_P256_WITH_SHA256             -7
    #define WEBAUTHN_COSE_ALGORITHM_ECDSA_P384_WITH_SHA384             -35
    #define WEBAUTHN_COSE_ALGORITHM_ECDSA_P521_WITH_SHA512             -36

    #define WEBAUTHN_COSE_ALGORITHM_RSASSA_PKCS1_V1_5_WITH_SHA256      -257
    #define WEBAUTHN_COSE_ALGORITHM_RSASSA_PKCS1_V1_5_WITH_SHA384      -258
    #define WEBAUTHN_COSE_ALGORITHM_RSASSA_PKCS1_V1_5_WITH_SHA512      -259

    #define WEBAUTHN_COSE_ALGORITHM_RSA_PSS_WITH_SHA256                -37
    #define WEBAUTHN_COSE_ALGORITHM_RSA_PSS_WITH_SHA384                -38
    #define WEBAUTHN_COSE_ALGORITHM_RSA_PSS_WITH_SHA512                -39

    #define WEBAUTHN_COSE_CREDENTIAL_PARAMETER_CURRENT_VERSION          1

    typedef struct _WEBAUTHN_COSE_CREDENTIAL_PARAMETER {
        // Version of this structure, to allow for modifications in the future.
        DWORD dwVersion;

        // Well-known credential type specifying a credential to create.
        LPCWSTR pwszCredentialType;

        // Well-known COSE algorithm specifying the algorithm to use for the credential.
        LONG lAlg;
    } WEBAUTHN_COSE_CREDENTIAL_PARAMETER, *PWEBAUTHN_COSE_CREDENTIAL_PARAMETER;
    typedef const WEBAUTHN_COSE_CREDENTIAL_PARAMETER *PCWEBAUTHN_COSE_CREDENTIAL_PARAMETER;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCoseCredentialParameter
    {
        uint dwVersion;
        string pwszCredentialType;
        int lAlg;
        
        public RawCoseCredentialParameter(CoseCredentialParameter template)
        {
            dwVersion = (uint)template.Version;
            pwszCredentialType = template.CredentialType.SerializedValue;
            lAlg = (int)template.CoseAlgorithm;
        }
    }
}

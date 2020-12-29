using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Attestation Info.
    //
    //-------------------------------------------------------------------------------------------
    #define WEBAUTHN_ATTESTATION_DECODE_NONE                                0
    #define WEBAUTHN_ATTESTATION_DECODE_COMMON                              1
    // WEBAUTHN_ATTESTATION_DECODE_COMMON supports format types
    //  L"packed"
    //  L"fido-u2f"

    #define WEBAUTHN_ATTESTATION_VER_TPM_2_0   L"2.0"

    typedef struct _WEBAUTHN_X5C {
        // Length of X.509 encoded certificate
        DWORD cbData;
        // X.509 encoded certificate bytes
        _Field_size_bytes_(cbData)
        PBYTE pbData;
    } WEBAUTHN_X5C, *PWEBAUTHN_X5C;

    // Supports either Self or Full Basic Attestation

    // Note, new fields will be added to the following data structure to
    // support additional attestation format types, such as, TPM.
    // When fields are added, the dwVersion will be incremented.
    //
    // Therefore, your code must make the following check:
    //  "if (dwVersion >= WEBAUTHN_COMMON_ATTESTATION_CURRENT_VERSION)"

    #define WEBAUTHN_COMMON_ATTESTATION_CURRENT_VERSION                     1

    typedef struct _WEBAUTHN_COMMON_ATTESTATION {
        // Version of this structure, to allow for modifications in the future.
        DWORD dwVersion;

        // Hash and Padding Algorithm
        //
        // The following won't be set for "fido-u2f" which assumes "ES256".
        PCWSTR pwszAlg;
        LONG lAlg;      // COSE algorithm

        // Signature that was generated for this attestation.
        DWORD cbSignature;
        _Field_size_bytes_(cbSignature)
        PBYTE pbSignature;

        // Following is set for Full Basic Attestation. If not, set then, this is Self Attestation.
        // Array of X.509 DER encoded certificates. The first certificate is the signer, leaf certificate.
        DWORD cX5c;
        _Field_size_(cX5c)
        PWEBAUTHN_X5C pX5c;

        // Following are also set for tpm
        PCWSTR pwszVer; // L"2.0"
        DWORD cbCertInfo;
        _Field_size_bytes_(cbCertInfo)
        PBYTE pbCertInfo;
        DWORD cbPubArea;
        _Field_size_bytes_(cbPubArea)
        PBYTE pbPubArea;
    } WEBAUTHN_COMMON_ATTESTATION, *PWEBAUTHN_COMMON_ATTESTATION;
    typedef const WEBAUTHN_COMMON_ATTESTATION *PCWEBAUTHN_COMMON_ATTESTATION;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCommonAttestation
    {
        public uint dwVersion;
        public string pwszAlg;
        public int lAlg;
        public uint cbSignature;
        public byte[] pbSignature;
        public uint cX5c;
        public RawX5C[] pX5c { get; set;}
        public string pwszVer;
        public uint cbCertInfo;
        public byte[] pbCertInfo;
        public uint cbPubArea;
        public byte[] pbPubArea;

        public CommonAttestation ToCommonAttestation()
        {
            return new CommonAttestation
            {
                Version = (int)dwVersion,
                AlgorithmName = pwszAlg,
                Algorithm = (CoseAlgorithm)lAlg,
                Signature = (byte[])pbSignature.Clone(),
                AttestationCertificates = pX5c.Select(c => c.ToX509Certificate()).ToArray(),
                TpmAttestationVersion = pwszVer,
                TpmCertificateInfo = (byte[])pbCertInfo.Clone(),
                TpmPublicArea = (byte[])pbPubArea.Clone()
            };
        }
    }
}

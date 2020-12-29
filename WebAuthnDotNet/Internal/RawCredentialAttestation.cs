using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WebAuthnDotNet.Extensions.Internal;

namespace WebAuthnDotNet.Internal
{
    /*
    #define WEBAUTHN_ATTESTATION_TYPE_PACKED                                L"packed"
    #define WEBAUTHN_ATTESTATION_TYPE_U2F                                   L"fido-u2f"
    #define WEBAUTHN_ATTESTATION_TYPE_TPM                                   L"tpm"
    #define WEBAUTHN_ATTESTATION_TYPE_NONE                                  L"none"

    #define WEBAUTHN_CREDENTIAL_ATTESTATION_VERSION_1               1
    #define WEBAUTHN_CREDENTIAL_ATTESTATION_VERSION_2               2
    #define WEBAUTHN_CREDENTIAL_ATTESTATION_VERSION_3               3
    #define WEBAUTHN_CREDENTIAL_ATTESTATION_CURRENT_VERSION         WEBAUTHN_CREDENTIAL_ATTESTATION_VERSION_3

    typedef struct _WEBAUTHN_CREDENTIAL_ATTESTATION {
        // Version of this structure, to allow for modifications in the future.
        DWORD dwVersion;

        // Attestation format type
        PCWSTR pwszFormatType;

        // Size of cbAuthenticatorData.
        DWORD cbAuthenticatorData;
        // Authenticator data that was created for this credential.
        _Field_size_bytes_(cbAuthenticatorData)
        PBYTE pbAuthenticatorData;

        // Size of CBOR encoded attestation information
        //0 => encoded as CBOR null value.
        DWORD cbAttestation;
        //Encoded CBOR attestation information
        _Field_size_bytes_(cbAttestation)
        PBYTE pbAttestation;

        DWORD dwAttestationDecodeType;
        // Following depends on the dwAttestationDecodeType
        //  WEBAUTHN_ATTESTATION_DECODE_NONE
        //      NULL - not able to decode the CBOR attestation information
        //  WEBAUTHN_ATTESTATION_DECODE_COMMON
        //      PWEBAUTHN_COMMON_ATTESTATION;
        PVOID pvAttestationDecode;

        // The CBOR encoded Attestation Object to be returned to the RP.
        DWORD cbAttestationObject;
        _Field_size_bytes_(cbAttestationObject)
        PBYTE pbAttestationObject;

        // The CredentialId bytes extracted from the Authenticator Data.
        // Used by Edge to return to the RP.
        DWORD cbCredentialId;
        _Field_size_bytes_(cbCredentialId)
        PBYTE pbCredentialId;

        //
        // Following fields have been added in WEBAUTHN_CREDENTIAL_ATTESTATION_VERSION_2
        //

        WEBAUTHN_EXTENSIONS Extensions;

        //
        // Following fields have been added in WEBAUTHN_CREDENTIAL_ATTESTATION_VERSION_3
        //

        // One of the WEBAUTHN_CTAP_TRANSPORT_* bits will be set corresponding to
        // the transport that was used.
        DWORD dwUsedTransport;

    } WEBAUTHN_CREDENTIAL_ATTESTATION, *PWEBAUTHN_CREDENTIAL_ATTESTATION;
    typedef const WEBAUTHN_CREDENTIAL_ATTESTATION *PCWEBAUTHN_CREDENTIAL_ATTESTATION;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCredentialAttestation
    {
        public uint dwVersion;
        public string pwszFormatType;
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ArrayMarshaler<byte>))]
        public byte[] pbAuthenticatorData;
        public uint cbAttestation;
        public byte[] pbAttestation;
        public uint dwAttestationDecodeType;
        public RawCommonAttestation? pvAttestationDecode;
        public uint cbAttestationObject;
        public byte[] pbAttestationObject;
        public uint cbCredentialId;
        public byte[] pbCredentialId;
        public RawWebAuthnExtensions Extensions;
        public uint dwUsedTransport;

        public CredentialAttestation ToCredentialAttestation()
        {
            return new CredentialAttestation
            {
                Version = (int)dwVersion,
                AttestationType = AttestationType.Deserialize(pwszFormatType),
                AuthenticatorData = pbAuthenticatorData,
                Attestation = pbAttestation,
                AttestationDecodeType = (AttestationDecodeType)dwAttestationDecodeType,
                AttestationDecode = pvAttestationDecode?.ToCommonAttestation(),
                CredentialId = (byte[])pbCredentialId.Clone(),
                Extensions = null, //todo
                CtapTransport = (CtapTransport)dwUsedTransport
            };
        }
    }
}

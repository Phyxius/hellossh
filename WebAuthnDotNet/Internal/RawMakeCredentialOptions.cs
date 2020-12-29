using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WebAuthnDotNet.Extensions.Internal;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Options.
    //-------------------------------------------------------------------------------------------

    #define WEBAUTHN_AUTHENTICATOR_ATTACHMENT_ANY                               0
    #define WEBAUTHN_AUTHENTICATOR_ATTACHMENT_PLATFORM                          1
    #define WEBAUTHN_AUTHENTICATOR_ATTACHMENT_CROSS_PLATFORM                    2
    #define WEBAUTHN_AUTHENTICATOR_ATTACHMENT_CROSS_PLATFORM_U2F_V2             3

    #define WEBAUTHN_USER_VERIFICATION_REQUIREMENT_ANY                          0
    #define WEBAUTHN_USER_VERIFICATION_REQUIREMENT_REQUIRED                     1
    #define WEBAUTHN_USER_VERIFICATION_REQUIREMENT_PREFERRED                    2
    #define WEBAUTHN_USER_VERIFICATION_REQUIREMENT_DISCOURAGED                  3

    #define WEBAUTHN_ATTESTATION_CONVEYANCE_PREFERENCE_ANY                      0
    #define WEBAUTHN_ATTESTATION_CONVEYANCE_PREFERENCE_NONE                     1
    #define WEBAUTHN_ATTESTATION_CONVEYANCE_PREFERENCE_INDIRECT                 2
    #define WEBAUTHN_ATTESTATION_CONVEYANCE_PREFERENCE_DIRECT                   3

    #define WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS_VERSION_1            1
    #define WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS_VERSION_2            2
    #define WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS_VERSION_3            3
    #define WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS_CURRENT_VERSION      WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS_VERSION_3

    typedef struct _WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS {
        // Version of this structure, to allow for modifications in the future.
        DWORD dwVersion;

        // Time that the operation is expected to complete within.
        // This is used as guidance, and can be overridden by the platform.
        DWORD dwTimeoutMilliseconds;

        // Credentials used for exclusion.
        WEBAUTHN_CREDENTIALS CredentialList;

        // Optional extensions to parse when performing the operation.
        WEBAUTHN_EXTENSIONS Extensions;

        // Optional. Platform vs Cross-Platform Authenticators.
        DWORD dwAuthenticatorAttachment;

        // Optional. Require key to be resident or not. Defaulting to FALSE;
        BOOL bRequireResidentKey;

        // User Verification Requirement.
        DWORD dwUserVerificationRequirement;

        // Attestation Conveyance Preference.
        DWORD dwAttestationConveyancePreference;

        // Reserved for future Use
        DWORD dwFlags;

        //
        // The following fields have been added in WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS_VERSION_2
        //

        // Cancellation Id - Optional - See WebAuthNGetCancellationId
        GUID *pCancellationId;

        //
        // The following fields have been added in WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS_VERSION_3
        //

        // Exclude Credential List. If present, "CredentialList" will be ignored.
        PWEBAUTHN_CREDENTIAL_LIST pExcludeCredentialList;

    } WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS, *PWEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS;
    typedef const WEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS *PCWEBAUTHN_AUTHENTICATOR_MAKE_CREDENTIAL_OPTIONS;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawMakeCredentialOptions
    {
        uint dwVersion { get; set; }
        uint dwTimeoutMilliseconds { get; set; }
        IntPtr CredentialList { get; set; }
        RawWebAuthnExtensions Extensions { get; set; }
        uint dwAuthenticatorAttachment { get; set; }
        uint bRequireResidentKey { get; set; }
        uint dwUserVerificationRequirement { get; set; }
        uint dwAttestationConveyancePreference { get; set; }
        uint dwFlags { get; set; }
        Guid? pCancelationId { get; set; }
        RawCredentialList pExcludeCredentialList { get; set; }

        public RawMakeCredentialOptions(AuthenticatorMakeCredentialOptions template)
        {
            dwVersion = (uint)template.Version;
            dwTimeoutMilliseconds = (uint)template.TimeoutMillilseconds;
            CredentialList = IntPtr.Zero;
            Extensions = new RawWebAuthnExtensions(template.Extensions);
            dwAuthenticatorAttachment = (uint)template.AuthenticatorAttachment;
            bRequireResidentKey = template.RequireResidentKey ? 1u : 0u;
            dwUserVerificationRequirement = (uint)template.UserVerificationRequirement;
            dwAttestationConveyancePreference = (uint)template.AttestationConveyancePreference;
            dwFlags = (uint)template.Flags;
            pCancelationId = template.CancelationId;
            pExcludeCredentialList = new RawCredentialList(template.ExcludedCredentials);
        }
    }
}

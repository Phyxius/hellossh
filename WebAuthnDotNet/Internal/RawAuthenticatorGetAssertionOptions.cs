using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WebAuthnDotNet.Extensions.Internal;

namespace WebAuthnDotNet.Internal
{
    /*
    #define WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_VERSION_1          1
    #define WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_VERSION_2          2
    #define WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_VERSION_3          3
    #define WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_VERSION_4          4
    #define WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_CURRENT_VERSION    WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_VERSION_4

    typedef struct _WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS {
        // Version of this structure, to allow for modifications in the future.
        DWORD dwVersion;

        // Time that the operation is expected to complete within.
        // This is used as guidance, and can be overridden by the platform.
        DWORD dwTimeoutMilliseconds;

        // Allowed Credentials List.
        WEBAUTHN_CREDENTIALS CredentialList;

        // Optional extensions to parse when performing the operation.
        WEBAUTHN_EXTENSIONS Extensions;

        // Optional. Platform vs Cross-Platform Authenticators.
        DWORD dwAuthenticatorAttachment;

        // User Verification Requirement.
        DWORD dwUserVerificationRequirement;

        // Reserved for future Use
        DWORD dwFlags;

        //
        // The following fields have been added in WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_VERSION_2
        //

        // Optional identifier for the U2F AppId. Converted to UTF8 before being hashed. Not lower cased.
        PCWSTR pwszU2fAppId;

        // If the following is non-NULL, then, set to TRUE if the above pwszU2fAppid was used instead of
        // PCWSTR pwszRpId;
        BOOL *pbU2fAppId;

        //
        // The following fields have been added in WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_VERSION_3
        //

        // Cancellation Id - Optional - See WebAuthNGetCancellationId
        GUID *pCancellationId;

        //
        // The following fields have been added in WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS_VERSION_4
        //

        // Allow Credential List. If present, "CredentialList" will be ignored.
        PWEBAUTHN_CREDENTIAL_LIST pAllowCredentialList;

    } WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS,  *PWEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS;
    typedef const WEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS  *PCWEBAUTHN_AUTHENTICATOR_GET_ASSERTION_OPTIONS;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawAuthenticatorGetAssertionOptions
    {
        uint dwVersion;
        uint dwTimeoutMilliseconds;
        IntPtr CredentialList;
        RawWebAuthnExtensions Extensions;
        uint dwAuthenticatorAttachment;
        uint dwUserVerificationRequirement;
        uint dwFlags;
        string pwszU2fAppId;
        BoolPtr pbU2fAppId;
        Guid? pCancelationId;
        RawCredentialList pAllowCredentialsList;

        public RawAuthenticatorGetAssertionOptions(AuthenticatorGetAssertionOptions template)
        {
            dwVersion = (uint)template.Version;
            dwTimeoutMilliseconds = (uint)template.TimeoutMilliseconds;
            CredentialList = IntPtr.Zero;
            Extensions = new RawWebAuthnExtensions(template.Extensions);
            dwAuthenticatorAttachment = (uint)template.AuthenticatorAttachment;
            dwUserVerificationRequirement = (uint)template.UserVerificationRequirement;
            dwFlags = (uint)template.Flags;
            pwszU2fAppId = template.U2fAppId;
            pbU2fAppId = new BoolPtr { BooleanValue = template.U2fAppId != null };
            pCancelationId = template.CancellationId;
            pAllowCredentialsList = new RawCredentialList(template.AllowedCredentialList);
        }
    }
}

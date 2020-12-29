using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Extensions.Internal
{
    /*
    #define WEBAUTHN_USER_VERIFICATION_ANY                                          0
    #define WEBAUTHN_USER_VERIFICATION_OPTIONAL                                     1
    #define WEBAUTHN_USER_VERIFICATION_OPTIONAL_WITH_CREDENTIAL_ID_LIST             2
    #define WEBAUTHN_USER_VERIFICATION_REQUIRED                                     3

    typedef struct _WEBAUTHN_CRED_PROTECT_EXTENSION_IN {
        // One of the above WEBAUTHN_USER_VERIFICATION_* values
        DWORD dwCredProtect;
        // Set the following to TRUE to require authenticator support for the credProtect extension
        BOOL bRequireCredProtect;
    } WEBAUTHN_CRED_PROTECT_EXTENSION_IN, *PWEBAUTHN_CRED_PROTECT_EXTENSION_IN;
    typedef const WEBAUTHN_CRED_PROTECT_EXTENSION_IN *PCWEBAUTHN_CRED_PROTECT_EXTENSION_IN;


    #define WEBAUTHN_EXTENSIONS_IDENTIFIER_CRED_PROTECT                 L"credProtect"
    // Below type definitions is for WEBAUTHN_EXTENSIONS_IDENTIFIER_CRED_PROTECT
    // MakeCredential Input Type:   WEBAUTHN_CRED_PROTECT_EXTENSION_IN.
    //      - pvExtension must point to a WEBAUTHN_CRED_PROTECT_EXTENSION_IN struct
    //      - cbExtension will contain the sizeof(WEBAUTHN_CRED_PROTECT_EXTENSION_IN).
    // MakeCredential Output Type:  DWORD.
    //      - pvExtension will point to a DWORD with one of the above WEBAUTHN_USER_VERIFICATION_* values
    //        if credential was successfully created with CRED_PROTECT.
    //      - cbExtension will contain the sizeof(DWORD).
    // GetAssertion Input Type:     Not Supported
    // GetAssertion Output Type:    Not Supported
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCredProtectExtensionIn
    {
        public uint dwCredProtect { get; set; }
        public uint bRequireCredProtect { get; set; }

        public RawCredProtectExtensionIn(CredProtectOptions template)
        {
            dwCredProtect = (uint)template.CredProtectType;
            bRequireCredProtect = template.Required ? 1u : 0u;
        }
    }
}

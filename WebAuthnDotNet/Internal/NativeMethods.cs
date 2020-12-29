using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    internal static class NativeMethods
    {
        private const string WebAuthnDll = "webauthn.dll";
        [DllImport(WebAuthnDll)]
        public static extern uint WebAuthNGetApiVersionNumber();
        [DllImport(WebAuthnDll, PreserveSig = false)]
        public static extern void WebAuthNIsUserVerifyingPlatformAuthenticatorAvailable(out bool pbIsUserVerifyingPlatformAuthenticatorAvailable);
        [DllImport(WebAuthnDll, PreserveSig = false)]
        public static extern void WebAuthNAuthenticatorMakeCredential(IntPtr hWnd,
                                                                      RawRPEntityInformation pRpInformation,
                                                                      RawUserEntityInformation pUserInformation,
                                                                      RawCoseCredentialParameters pPubKeyCredParams,
                                                                      RawClientData pWebAuthNClientData,
                                                                      RawMakeCredentialOptions pWebAuthNMakeCredentialOptions,
                                                                      out RawCredentialAttestation ppWebAuthNCredentialAttestation);
        [DllImport(WebAuthnDll, PreserveSig = false)]
        public static extern void WebAuthNAuthenticatorMakeCredential(IntPtr hWnd,
                                                                      RawRPEntityInformation pRpInformation,
                                                                      RawUserEntityInformation pUserInformation,
                                                                      RawCoseCredentialParameters pPubKeyCredParams,
                                                                      RawClientData pWebAuthNClientData,
                                                                      IntPtr ppWebAuthNCredentialAttestation,
                                                                      out RawCredentialAttestation ppWebAuthNCredentialAttestation2);
    }
}

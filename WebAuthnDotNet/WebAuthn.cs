using System;
using System.Collections.Generic;
using System.Text;
using WebAuthnDotNet.Internal;
using System.Linq;

namespace WebAuthnDotNet
{
    public static class WebAuthn
    {
        public static int ApiVersion => (int)NativeMethods.WebAuthNGetApiVersionNumber();
        public static bool UserVerifyingPlatformAuthenticatorAvailable
        {
            get
            {
                NativeMethods.WebAuthNIsUserVerifyingPlatformAuthenticatorAvailable(out bool b);
                return b;
            }
        }

        public static CredentialAttestation MakeCredential(IntPtr hWnd,
                                                           RPEntityInformation rpEntityInformation,
                                                           UserEntityInformation userEntityInformation,
                                                           CoseCredentialParameter[] coseCredentialParameters,
                                                           ClientData clientData,
                                                           AuthenticatorMakeCredentialOptions authenticatorMakeCredentialOptions)
        {
            var rawRpEntityInfo = new RawRPEntityInformation(rpEntityInformation);
            var rawUserEntityInfo = new RawUserEntityInformation(userEntityInformation);
            var rawCoseCredentialParam = new RawCoseCredentialParameters(coseCredentialParameters.Select(c => new RawCoseCredentialParameter(c)));
            var rawClientData = new RawClientData(clientData);
            RawCredentialAttestation attestation;
            if (authenticatorMakeCredentialOptions != null)
            {
                var rawMakeCredOptions = new RawMakeCredentialOptions(authenticatorMakeCredentialOptions);
                NativeMethods.WebAuthNAuthenticatorMakeCredential(hWnd,
                                                                  rawRpEntityInfo,
                                                                  rawUserEntityInfo,
                                                                  rawCoseCredentialParam,
                                                                  rawClientData,
                                                                  rawMakeCredOptions,
                                                                  out attestation);
            }
            else
            {
                NativeMethods.WebAuthNAuthenticatorMakeCredential(hWnd,
                                                                  rawRpEntityInfo,
                                                                  rawUserEntityInfo,
                                                                  rawCoseCredentialParam,
                                                                  rawClientData,
                                                                  IntPtr.Zero,
                                                                  out attestation);
            }
            return attestation.ToCredentialAttestation();
        }
    }
}

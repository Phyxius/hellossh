using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawAssertion
    {
        uint dwVersion;
        uint cbAuthenticatorData;
        byte[] pbAuthenticatorData;
        uint cbSignature;
        byte[] pbSignature;
        RawCredential Credential;
        uint cbUserid;
        byte[] pbUserId;

        public Assertion ToAssertion()
        {
            return new Assertion
            {
                Version = (int)dwVersion,
                AuthenticatorData = (byte[])pbAuthenticatorData.Clone(),
                Signature = (byte[])pbSignature.Clone(),
                Credential = Credential.ToCredential(),
                UserId = (byte[])pbUserId.Clone()
            };
        }
    }
}

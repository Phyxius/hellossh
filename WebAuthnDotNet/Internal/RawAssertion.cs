using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawAssertion
    {
        uint dwVersion { get; set; }
        uint cbAuthenticatorData { get; set; }
        byte[] pbAuthenticatorData { get; set; }
        uint cbSignature { get; set; }
        byte[] pbSignature { get; set; }
        RawCredential Credential { get; set; }
        uint cbUserid { get; set; }
        byte[] pbUserId { get; set; }

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

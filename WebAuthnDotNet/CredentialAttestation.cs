using System;
using System.Collections.Generic;
using System.Text;
using WebAuthnDotNet.Extensions;

namespace WebAuthnDotNet
{
    public class CredentialAttestation
    {
        public const int VersionOne = 1,
            VersionTwo = 2,
            VersionThree = 3,
            CurrenVersion = VersionThree;
        public int Version { get; set; }
        public AttestationType AttestationType { get; set; }
        public byte[] AuthenticatorData { get; set; }
        public byte[] Attestation { get; set; }
        public AttestationDecodeType AttestationDecodeType { get; set; }
        public CommonAttestation AttestationDecode { get; set; }
        public byte[] CredentialId { get; set; }
        public IExtension<object>[] Extensions { get; set; }
        public CtapTransport CtapTransport { get; set; }
    }
}

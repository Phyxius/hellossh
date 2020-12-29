using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebAuthnDotNet
{
    public class CommonAttestation
    {
        public const int CurrentVersion = 1;
        public const string AttestationVersionTpmTwoPointZero = "2.0";
        public int Version { get; set; } = CurrentVersion;
        public string AlgorithmName { get; set; }
        public CoseAlgorithm Algorithm { get; set; }
        public byte[] Signature { get; set; }
        public X509Certificate[] AttestationCertificates { get; set; }
        public string TpmAttestationVersion { get; set; }
        public byte[] TpmCertificateInfo { get; set; }
        public byte[] TpmPublicArea { get; set; }
    }
}

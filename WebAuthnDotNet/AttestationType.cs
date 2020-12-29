using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public class AttestationType
    {
        public string SerializedValue { get; private set; }
        private const string PackedType = "packed",
            U2fType = "fido-u2f",
            TpmType = "tpm",
            NoneType = "none";
        public AttestationType(string serializedValue)
        {
            SerializedValue = serializedValue;
        }

        public static readonly AttestationType Packed = new AttestationType(PackedType);
        public static readonly AttestationType U2f = new AttestationType(U2fType);
        public static readonly AttestationType Tpm = new AttestationType(TpmType);
        public static readonly AttestationType None = new AttestationType(NoneType);

        public static AttestationType Deserialize(string serializedValue)
        {
            switch (serializedValue)
            {
                case PackedType: return Packed;
                case U2fType: return U2f;
                case TpmType: return Tpm;
                case NoneType: return None;
                default: throw new ArgumentException($"Unknown AttestationType {serializedValue}!");
            }
        }
    }
}

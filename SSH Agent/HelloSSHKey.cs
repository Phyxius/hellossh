﻿using HelloSSH.Agent;
using SSHAgentFramework;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Credentials;
using Windows.Security.Cryptography.Core;
using System.Security.Cryptography;
using Windows.Security.Cryptography;
using System.Text.Json;

namespace HelloSSH
{
    class HelloSSHKey
    {
        public readonly KeyCredential Credential;
        public string Comment { get; private set; }
        public string PublicKeyFingerprint { get; private set; }
        public string PublicKeyHash { get; private set; }
        public readonly byte[] KeyIdentifier;
        public readonly SSHPublicKey PublicKey;

        public HelloSSHKey(KeyCredential credential, string comment)
        {
            Credential = credential;
            Comment = comment;
            PublicKey = GetPublicKeyFromCredential(credential);
            KeyIdentifier = PublicKey.Serialize(false);
            PublicKeyFingerprint = GetKeyFingerprint(PublicKey, Comment);
            PublicKeyHash = GetKeyHash(PublicKey);
        }

        //https://coolaj86.com/articles/the-ssh-public-key-format/
        private static string GetKeyFingerprint(SSHPublicKey publicKey, string comment)
        {
            byte[] fpBytes = GetKeyFingerprintBytes(publicKey);
            return $"{publicKey.KeyType} {Convert.ToBase64String(fpBytes)} {comment}";
        }

        //https://coolaj86.com/articles/ssh-pubilc-key-fingerprints/
        private static string GetKeyHash(SSHPublicKey publicKey)
        {
            var hashedFpBytes = SHA256.HashData(GetKeyFingerprintBytes(publicKey));
            return $"SHA256:{Convert.ToBase64String(hashedFpBytes).Trim('=')}";
        }
        private static byte[] GetKeyFingerprintBytes(SSHPublicKey publicKey)
        {
            return WireUtils.EncodeString(publicKey.KeyType)
                .Concat(WireUtils.EncodeToMPInt(publicKey.ExponentOrECTypeName))
                .Concat(WireUtils.EncodeToMPInt(publicKey.ModulusOrECPoint))
                .ToArray();
        }

        private static SSHPublicKey GetPublicKeyFromCredential(KeyCredential cred)
        {
            var publicKeyStream = cred.RetrievePublicKey(CryptographicPublicKeyBlobType.BCryptPublicKey).AsStream();
            var header = BCryptKeyBlob.FromStream(publicKeyStream);
            var keyData = new SSHPublicKey
            {
                KeyType = SSHPublicKey.KEY_TYPE_RSA,
                ExponentOrECTypeName = new byte[header.cbPublicExp],
                ModulusOrECPoint = new byte[header.cbModulus]
            };
            publicKeyStream.Read(keyData.ExponentOrECTypeName, 0, keyData.ExponentOrECTypeName.Length);
            publicKeyStream.Read(keyData.ModulusOrECPoint, 0, keyData.ModulusOrECPoint.Length);

            return keyData;
        }
        public byte[] SignChallenge(byte[] challenge)
        {
            var task = Credential.RequestSignAsync(CryptographicBuffer.CreateFromByteArray(challenge)).AsTask();
            task.Wait();
            var result = task.Result;
            return result.Result?.ToArray();
        }

        public KeyCredentialAttestationStatus GetAttestation(out AttestationResult result)
        {
            var task = Credential.GetAttestationAsync().AsTask();
            task.Wait();
            var taskResult = task.Result;
            result = taskResult.Status == KeyCredentialAttestationStatus.Success ?
                new AttestationResult(taskResult) : null;
            return taskResult.Status;
        }

        public class AttestationResult
        {
            public byte[] CertificateChain { get; private set; }
            public byte[] Attestation { get; private set; }
            public AttestationResult(KeyCredentialAttestationResult result)
            {
                CertificateChain = Util.GetIBufferBytes(result.CertificateChainBuffer);
                Attestation = Util.GetIBufferBytes(result.AttestationBuffer);
            }
            public string Serialize()
            {
                return JsonSerializer.Serialize(this);
            }
        }
    }
}

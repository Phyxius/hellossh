using HelloSSH.Agent;
using SSHAgentFramework;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Credentials;
using Windows.Security.Cryptography.Core;
using System.Security.Cryptography;

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
    }
}

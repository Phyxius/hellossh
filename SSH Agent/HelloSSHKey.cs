using HelloSSH.Agent;
using SSHAgentFramework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Credentials;
using Windows.Security.Cryptography.Core;
using System.Linq;

namespace HelloSSH
{
    class HelloSSHKey
    {
        public readonly KeyCredential Credential;
        public readonly string Comment;
        public readonly string PublicKeyFingerprint;
        public readonly byte[] KeyIdentifier;
        public readonly SSHPublicKey PublicKey;

        public HelloSSHKey(KeyCredential credential, string comment)
        {
            Credential = credential;
            Comment = comment;
            PublicKey = GetPublicKeyFromCredential(credential);
            KeyIdentifier = PublicKey.Serialize(false);
            PublicKeyFingerprint = GetKeyFingerprint(PublicKey, Comment); 
        }

        //https://coolaj86.com/articles/the-ssh-public-key-format/
        private static string GetKeyFingerprint(SSHPublicKey publicKey, string comment)
        {
            var fpBytes = WireUtils.EncodeString(publicKey.KeyType)
                .Concat(WireUtils.EncodeToMPInt(publicKey.ExponentOrECTypeName))
                .Concat(WireUtils.EncodeToMPInt(publicKey.ModulusOrECPoint))
                .ToArray();
            return publicKey.KeyType + " " + Convert.ToBase64String(fpBytes) + " " + comment;
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

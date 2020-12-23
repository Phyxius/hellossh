using SSHAgentFramework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Security.Credentials;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Security.Cryptography;

namespace HelloSSH
{
    class HelloSSHAgent : AbstractSSHAgent
    {
        KeyCredential credential;
        public HelloSSHAgent()
        {
            LoadOrCreateCredential();
            Console.WriteLine(PublicKeyToInterchangeFormat(credential));
        }

        public override IAgentMessage ProcessMessage(AgentMessage message)
        {
            switch (message.Type)
            {
                case AgentMessageType.SSH_AGENTC_REQUEST_IDENTITIES:
                    return new AgentIdentitiesAnswerMessage
                    {
                        Keys = new List<(byte[], string)>()
                        {
                            (PublicKeyToWireFormat(credential), credential.Name)
                        }
                    };
                case AgentMessageType.SSH_AGENTC_SIGN_REQUEST:
                    var request = ClientSignRequestMessage.Deserialize(message.Contents);
                    return new AgentSignResponseMessage
                    {
                        Type = "rsa-sha2-256",
                        Blob = SignChallenge(credential, request.Challenge, SignatureType.RSA_SHA2_256)
                    };
                default:
                    return base.ProcessMessage(message);
            }
        }

        private void LoadOrCreateCredential()
        {
            var resultTask = KeyCredentialManager.RequestCreateAsync("hellossh", KeyCredentialCreationOption.FailIfExists).AsTask();
            resultTask.Wait();
            var result = resultTask.Result;
            switch(result.Status)
            {
                case KeyCredentialStatus.Success:
                    credential = result.Credential;
                    return;
                case KeyCredentialStatus.CredentialAlreadyExists:
                    var openTask = KeyCredentialManager.OpenAsync("hellossh").AsTask();
                    openTask.Wait();
                    credential = openTask.Result.Credential;
                    return;
                default:
                    throw new Exception(result.Status.ToString());
            }
        }

        private enum SignatureType
        {
            RSA_SHA2_256, RSA_SHA2_512
        }
        private byte[] SignChallenge(KeyCredential credential, byte[] challenge, SignatureType type)
        {
            //switch(type)
            //{
            //    case SignatureType.RSA_SHA2_256:
            //        challenge = SHA256.Create().ComputeHash(challenge);
            //        break;
            //    case SignatureType.RSA_SHA2_512:
            //        challenge = SHA512.Create().ComputeHash(challenge);
            //        break;
            //}
            var task = credential.RequestSignAsync(CryptographicBuffer.CreateFromByteArray(challenge)).AsTask();
            task.Wait();
            var result = task.Result;
            return result.Result.ToArray();
        }

        //https://coolaj86.com/articles/the-ssh-public-key-format/
        private static string PublicKeyToInterchangeFormat(KeyCredential cred)
        {
            //TODO: THIS IS CURRENTLY NOT CORRECT
            return "ssh-rsa " + Convert.ToBase64String(PublicKeyToWireFormat(cred));
        }
        private static byte[] PublicKeyToWireFormat(KeyCredential cred)
        {
            var publicKeyStream = cred.RetrievePublicKey(CryptographicPublicKeyBlobType.BCryptPublicKey).AsStream();
            var header = BCryptKeyBlob.FromStream(publicKeyStream);
            var keyData = new EncodedSSHPublicKey
            {
                KeyType = EncodedSSHPublicKey.KEY_TYPE_RSA_SHA256,
                ExponentOrECTypeName = new byte[header.cbPublicExp],
                ModulusOrECPoint = new byte[header.cbModulus]
            };
            publicKeyStream.Read(keyData.ExponentOrECTypeName, 0, keyData.ExponentOrECTypeName.Length);
            publicKeyStream.Read(keyData.ModulusOrECPoint, 0, keyData.ModulusOrECPoint.Length);

            return keyData.Serialize();
        }

    }
}

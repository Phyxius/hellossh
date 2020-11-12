using SSHAgentFramework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Security.Credentials;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace HelloSSH
{
    class HelloSSHAgent : AbstractSSHAgent
    {
        KeyCredential credential;
        public HelloSSHAgent()
        {
            LoadOrCreateCredential();
            Console.WriteLine(PublicKeyToSSHFormat(credential));
        }

        public override IAgentMessage ProcessMessage(AgentMessage message)
        {
            switch (message.Type)
            {
                case AgentMessageType.SSH_AGENTC_REQUEST_IDENTITIES:
                    return new AgentIdentitiesAnswerMessage
                    {
                        Keys = new List<(string, string)>()
                        {
                            (PublicKeyToSSHFormat(credential), credential.Name)
                        }
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

        //https://coolaj86.com/articles/the-ssh-public-key-format/
        private static string PublicKeyToSSHFormat(KeyCredential cred)
        {
            var publicKeyStream = cred.RetrievePublicKey(CryptographicPublicKeyBlobType.BCryptPublicKey).AsStream();
            var header = BCryptKeyBlob.FromStream(publicKeyStream);
            var keyData = new EncodedSSHPublicKey
            {
                KeyType = EncodedSSHPublicKey.KEY_TYPE_RSA,
                ExponentOrECTypeName = new byte[header.cbPublicExp],
                ModulusOrECPoint = new byte[header.cbModulus]
            };
            publicKeyStream.Read(keyData.ExponentOrECTypeName, 0, keyData.ExponentOrECTypeName.Length);
            publicKeyStream.Read(keyData.ModulusOrECPoint, 0, keyData.ModulusOrECPoint.Length);

            return keyData.Serialize();
        }
    }
}

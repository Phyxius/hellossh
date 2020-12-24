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
using System.Linq;

namespace HelloSSH
{
    class HelloSSHAgent : AbstractSSHAgent
    {
        readonly ConfigurationProvider configuration;
        List<KeyCredential> credentials = new List<KeyCredential>();
        public HelloSSHAgent(ConfigurationProvider configurationProvider)
        {
            this.configuration = configurationProvider;
            LoadOrCreateCredentials();
        }

        public override IAgentMessage ProcessMessage(AgentMessage message)
        {
            switch (message.Type)
            {
                case AgentMessageType.SSH_AGENTC_REQUEST_IDENTITIES:
                    return new AgentIdentitiesAnswerMessage
                    {
                        Keys = credentials.Select(cred => (PublicKeyToWireFormat(cred), cred.Name)).ToList()
                    };
                case AgentMessageType.SSH_AGENTC_SIGN_REQUEST:
                    var request = ClientSignRequestMessage.Deserialize(message.Contents);
                    if ((request.Flags & (uint)AgentSignatureFlags.SSH_AGENT_RSA_SHA2_256) == 0)
                    {
                        return new AgentFailureMessage();
                    }
                    // the request's key blob length will get stripped out by the parser, so we tell our serializer not to include it
                    var cred = credentials.Find(cred => PublicKeyToWireFormat(cred, false).SequenceEqual(request.KeyBlob));
                    if (cred == null)
                    {
                        return new AgentFailureMessage();
                    }
                    var blob = SignChallenge(cred, request.Challenge);
                    if (blob == null) return new AgentFailureMessage();
                    return new AgentSignResponseMessage
                    {
                        Type = "rsa-sha2-256",
                        Blob = blob
                    };
                default:
                    return base.ProcessMessage(message);
            }
        }
        private void LoadOrCreateCredentials()
        {
            foreach (string handle in configuration.Configuration.KeyHandles)
            {
                var credential = LoadOrCreateCredential(handle);
                if (credential != null)
                {
                    credentials.Add(credential);
                }
            }
        }
        private KeyCredential LoadOrCreateCredential(string name)
        {
            var resultTask = KeyCredentialManager.RequestCreateAsync(name, KeyCredentialCreationOption.FailIfExists).AsTask();
            resultTask.Wait();
            var result = resultTask.Result;
            switch(result.Status)
            {
                case KeyCredentialStatus.Success:
                    return result.Credential;
                case KeyCredentialStatus.CredentialAlreadyExists:
                    var openTask = KeyCredentialManager.OpenAsync(name).AsTask();
                    openTask.Wait();
                    return openTask.Result.Credential;
                case KeyCredentialStatus.UserCanceled:
                case KeyCredentialStatus.UserPrefersPassword:
                    Console.WriteLine("You canceled creating a key! Continuing...");
                    return null;
                default:
                    throw new Exception(result.Status.ToString());
            }
        }

        private byte[] SignChallenge(KeyCredential credential, byte[] challenge)
        {
            var task = credential.RequestSignAsync(CryptographicBuffer.CreateFromByteArray(challenge)).AsTask();
            task.Wait();
            var result = task.Result;
            return result.Result?.ToArray();
        }

        public void ListenOnNamedPipe()
        {
            ListenOnNamedPipe(configuration.Configuration.NamedPipeLocation);
        }

        //https://coolaj86.com/articles/the-ssh-public-key-format/
        private static string PublicKeyToInterchangeFormat(KeyCredential cred)
        {
            //TODO: THIS IS CURRENTLY NOT CORRECT
            return "ssh-rsa " + Convert.ToBase64String(PublicKeyToWireFormat(cred));
        }
        private static byte[] PublicKeyToWireFormat(KeyCredential cred, bool includeOverallLength = true)
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

            return keyData.Serialize(includeOverallLength);
        }

    }
}

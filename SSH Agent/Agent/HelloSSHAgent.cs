using SSHAgentFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;

namespace HelloSSH.Agent
{
    class HelloSSHAgent : AbstractSSHAgent
    {
        readonly ConfigurationProvider configuration;
        readonly List<HelloSSHKey> credentials = new List<HelloSSHKey>();
        public HelloSSHAgent(ConfigurationProvider configurationProvider)
        {
            configuration = configurationProvider;
            LoadOrCreateCredentials();
        }

        public override IAgentMessage ProcessMessage(AgentMessage message, uint clientProcessId)
        {
            switch (message.Type)
            {
                case AgentMessageType.SSH_AGENTC_REQUEST_IDENTITIES:
                    return new AgentIdentitiesAnswerMessage
                    {
                        Keys = credentials.Select(cred => (cred.PublicKey, cred.Comment)).ToList()
                    };
                case AgentMessageType.SSH_AGENTC_SIGN_REQUEST:
                    var request = ClientSignRequestMessage.Deserialize(message.Contents);
                    if ((request.Flags & (uint)AgentSignatureFlags.SSH_AGENT_RSA_SHA2_256) == 0)
                    {
                        return new AgentFailureMessage();
                    }
                    // the request's key blob length will get stripped out by the parser, so we tell our serializer not to include it
                    var cred = credentials.Find(cred => cred.KeyIdentifier.SequenceEqual(request.KeyBlob));
                    if (cred == null)
                    {
                        return new AgentFailureMessage();
                    }
                    var blob = SignChallenge(cred.Credential, request.Challenge);
                    if (blob == null) return new AgentFailureMessage();
                    return new AgentSignResponseMessage
                    {
                        Type = "rsa-sha2-256",
                        Blob = blob
                    };
                default:
                    return base.ProcessMessage(message, clientProcessId);
            }
        }
        private void LoadOrCreateCredentials()
        {
            foreach (string handle in configuration.Configuration.KeyHandles)
            {
                var credential = LoadOrCreateCredential(handle);
                if (credential != null)
                {
                    credentials.Add(new HelloSSHKey(credential, handle));
                }
            }
        }
        private KeyCredential LoadOrCreateCredential(string name)
        {
            var resultTask = KeyCredentialManager.RequestCreateAsync(name, KeyCredentialCreationOption.FailIfExists).AsTask();
            resultTask.Wait();
            var result = resultTask.Result;
            switch (result.Status)
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
    }
}

using HelloSSH.DataStore;
using SSHAgentFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;

namespace HelloSSH.Agent
{
    delegate void PrivateKeyNotification(HelloSSHKey key, uint clientProcessId);
    class HelloSSHAgent : AbstractSSHAgent
    {
        readonly SynchronizedDataStore dataStore;
        readonly Configuration configuration;
        readonly ObservableCollection<HelloSSHKey> credentials;
        readonly ReaderWriterLockSlim lockSlim;

        public event PrivateKeyNotification PrivateKeyRequested;
        public HelloSSHAgent(SynchronizedDataStore synchronizedDataStore)
        {
            dataStore = synchronizedDataStore;
            configuration = synchronizedDataStore.ConfigurationProvider.Configuration;
            credentials = synchronizedDataStore.Keys;
            lockSlim = synchronizedDataStore.Lock;
            dataStore.LoadOrCreateCredentials();
        }

        public override IAgentMessage ProcessMessage(AgentMessage message, uint clientProcessId)
        {
            lockSlim.EnterReadLock();
            var ret = ProcessMessageInternal(message, clientProcessId);
            lockSlim.ExitReadLock();
            return ret;
        }
        private IAgentMessage ProcessMessageInternal(AgentMessage message, uint clientProcessId)
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
                    var cred = credentials.ToList().Find(cred => cred.KeyIdentifier.SequenceEqual(request.KeyBlob));
                    if (cred == null)
                    {
                        return new AgentFailureMessage();
                    }
                    PrivateKeyRequested?.Invoke(cred, clientProcessId);
                    var blob = cred.SignChallenge(request.Challenge);
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

        public void ListenOnNamedPipe()
        {
            lockSlim.EnterReadLock();
            var location = configuration.NamedPipeLocation;
            lockSlim.ExitReadLock();

            ListenOnNamedPipe(location);
        }
    }
}

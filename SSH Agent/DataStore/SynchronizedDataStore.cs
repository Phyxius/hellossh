using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Windows.Security.Credentials;

namespace HelloSSH.DataStore
{
    internal class SynchronizedDataStore
    {
        public readonly ConfigurationProvider ConfigurationProvider;
        public readonly List<HelloSSHKey> Keys;
        public readonly ReaderWriterLockSlim Lock;

        public SynchronizedDataStore(ConfigurationProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
            Keys = new List<HelloSSHKey>();
            Lock = new ReaderWriterLockSlim();
        }

        public void LoadOrCreateCredentials()
        {
            Lock.EnterWriteLock();
            foreach (string handle in ConfigurationProvider.Configuration.KeyHandles)
            {
                var credential = LoadOrCreateCredential(handle);
                if (credential != null)
                {
                    Keys.Add(new HelloSSHKey(credential, handle));
                }
            }
            Lock.ExitWriteLock();
        }

        public bool AddKey(string name)
        {
            var cred = LoadOrCreateCredential(name);
            if (cred == null)
            {
                return false;
            }
            Lock.EnterWriteLock();
            Keys.Add(new HelloSSHKey(cred, name));
            ConfigurationProvider.Configuration.KeyHandles.Add(name);
            ConfigurationProvider.Save();
            Lock.ExitWriteLock();
            return true;
        }
        private static KeyCredential LoadOrCreateCredential(string name)
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
        ~SynchronizedDataStore()
        {
            Lock.Dispose();
        }
    }
}

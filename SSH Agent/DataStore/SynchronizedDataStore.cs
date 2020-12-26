using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        ~SynchronizedDataStore()
        {
            Lock.Dispose();
        }
    }
}

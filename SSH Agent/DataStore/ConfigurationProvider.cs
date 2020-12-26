using System.IO;

namespace HelloSSH.DataStore
{
    class ConfigurationProvider
    {
        public Configuration Configuration
        {
            get;
            private set;
        }

        private readonly string configurationFilePath;

        public ConfigurationProvider(string configurationFilePath)
        {
            this.configurationFilePath = configurationFilePath;
            if (File.Exists(configurationFilePath))
            {
                Load();
            }
            else
            {
                Configuration = Configuration.DefaultSettings;
                Save();
            }
        }

        public void Load()
        {
            Configuration = Configuration.Deserialize(File.ReadAllText(configurationFilePath));
        }

        public void Save()
        {
            File.WriteAllText(configurationFilePath, Configuration.Serialize());
        }
    }
}

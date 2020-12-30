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

        public ConfigurationProvider(string configurationFilePath, out bool defaultSettingsCreated)
        {
            this.configurationFilePath = configurationFilePath;
            if (File.Exists(configurationFilePath))
            {
                Load();
                defaultSettingsCreated = false;
            }
            else
            {
                Configuration = Configuration.DefaultSettings;
                var folder = Directory.GetParent(configurationFilePath);
                folder.Create();
                Save();
                defaultSettingsCreated = true;
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

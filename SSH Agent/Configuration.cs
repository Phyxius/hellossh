using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelloSSH
{
    class Configuration
    {
        const decimal VERSION_ONE = 1.0M;
        const decimal MIN_COMPATIBLE_VERSION = VERSION_ONE, MAX_COMPATIBLE_VERSION = VERSION_ONE;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public decimal Version { get; set; }
        public List<string> KeyHandles { get; set; }
        public string NamedPipeLocation { get; set; }

        public string Serialize() => JsonSerializer.Serialize(this, options);

        public static Configuration Deserialize(string json)
        {
            var config = JsonSerializer.Deserialize<Configuration>(json);
            if (config.Version < MIN_COMPATIBLE_VERSION || config.Version > MAX_COMPATIBLE_VERSION)
            {
                throw new FormatException($"Incompatible configuration version! Expected version in [{MIN_COMPATIBLE_VERSION}, {MAX_COMPATIBLE_VERSION}], got {config.Version}.");
            }
            if (config.KeyHandles == null)
            {
                config.KeyHandles = new List<string>();
            }
            return config;
        }

        public static Configuration DefaultSettings => new Configuration
        {
            Version = MAX_COMPATIBLE_VERSION,
            KeyHandles = new List<string>(new string[] { Environment.MachineName.ToLower() + "-hellossh" }),
            NamedPipeLocation = "hellossh"
        };
    }
}

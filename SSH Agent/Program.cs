using System;

namespace HelloSSH
{
    class Program
    {
        const string DefaultConfigLocation = "helossh.json";
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                PrintUsageAndExit();
            }
            var configLocation = DefaultConfigLocation;
            if (args.Length > 0)
            {
                configLocation = args[1];
            }
            new HelloSSHAgent(new ConfigurationProvider(configLocation)).ListenOnNamedPipe();
        }

        private static void PrintUsageAndExit()
        {
            Console.WriteLine(@"Usage: heloossh.exe [C:\path\to\config.json]");
            Environment.Exit(-1);
        }
    }
}

using System;
using Windows.Security.Credentials;

namespace HelloSSH
{
    class Program
    {
        static void Main(string[] args)
        {
            //GenerateKey();
            new HelloSSHAgent().ListenOnNamedPipe("hellossh");
        }
        
        static async void GenerateKey()
        {
            var result = await KeyCredentialManager.RequestCreateAsync("test", KeyCredentialCreationOption.ReplaceExisting);
            Console.WriteLine(result.Status);
        }
    }
}

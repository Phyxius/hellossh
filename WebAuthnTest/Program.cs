using System;
using System.Text;
using WebAuthnDotNet;

namespace WebAuthnTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine($"WebAuthn API Version: {WebAuthn.ApiVersion}");
            Console.WriteLine($"WebAuthn Platform Authenticator Available: {WebAuthn.UserVerifyingPlatformAuthenticatorAvailable}");
            /*var result = WebAuthn.MakeCredential(
                IntPtr.Zero, 
                new RPEntityInformation { Id = "example.com" },
                new UserEntityInformation { Id = Encoding.UTF8.GetBytes("FooBar") },
                new CoseCredentialParameter[] { 
                    new CoseCredentialParameter { 
                        CoseAlgorithm = CoseAlgorithm.RsaSsaPkcs1V1Dot5WithSha256, 
                        CredentialType = CredentialType.PublicKey 
                    }
                },
                new ClientData { HashAlgorithm.Sha256, }
                )*/
        }
    }
}

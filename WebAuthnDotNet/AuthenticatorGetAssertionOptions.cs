using System;
using System.Collections.Generic;
using System.Text;
using WebAuthnDotNet.Extensions;

namespace WebAuthnDotNet
{
    public class AuthenticatorGetAssertionOptions
    {
        public const int VersionOne = 1,
            VersionTwo = 2,
            VersionThree = 3,
            VersionFour = 4,
            CurrentVersion = VersionFour;
        public int Version { get; set; } = CurrentVersion;
        public int TimeoutMilliseconds { get; set; }
        public IExtension<object>[] Extensions { get; set; }
        public AuthenticatorAttachment AuthenticatorAttachment { get; set; }
        public UserVerificationRequirement UserVerificationRequirement { get; set; }
        public int Flags { get; set; }
        public string U2fAppId { get; set; }
        public Guid? CancellationId { get; set; }
        public CredentialEx[] AllowedCredentialList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WebAuthnDotNet.Extensions;

namespace WebAuthnDotNet
{
    public class AuthenticatorMakeCredentialOptions
    {
        public const int VersionOne = 1,
            VersionTwo = 2,
            VersionThree = 3,
            CurrentVersion = VersionThree;
        public int Version { get; set; } = CurrentVersion;
        public int TimeoutMillilseconds { get; set; }
        public CredentialEx[] ExcludedCredentials { get; set; } 
        public IExtension<object>[] Extensions { get; set; }
        public AuthenticatorAttachment AuthenticatorAttachment { get; set; } = AuthenticatorAttachment.Any;
        public bool RequireResidentKey { get; set; }
        public UserVerificationRequirement UserVerificationRequirement { get; set; } = UserVerificationRequirement.Any;
        public AttestationConveyancePreference AttestationConveyancePreference { get; set; } = AttestationConveyancePreference.Any;
        public int Flags { get; set; }
        public Guid? CancelationId { get; set; }

    }
}

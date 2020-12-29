using System;

namespace WebAuthnDotNet
{
    public class RPEntityInformation
    {
        public const int CurrentVersion = 1;
        public int Version { get; set; } = CurrentVersion;
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}

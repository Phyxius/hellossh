using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public class UserEntityInformation
    {
        public const int MaxUserIDLength = 64, CurrentVersion = 1;
        public int Version { get; set; } = CurrentVersion;
        public byte[] Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string DisplayName { get; set; }
    }
}

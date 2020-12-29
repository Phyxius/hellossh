using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    class ClientData
    {
        public const int CurrentVersion = 1;
        public int Version { get; set; } = CurrentVersion;
        public string JSONClientData { get; set; }
        public HashAlgorithm HashAlgorithm { get; set; }
    }
}

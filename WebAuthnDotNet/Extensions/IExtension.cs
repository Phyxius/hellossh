using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet.Extensions
{
    public interface IExtension<T>
    {
        byte[] MarshalToBytes();
        T MarshalFromBytes(byte[] bytes);
        string ExtensionIdentifier { get; }
    }
}

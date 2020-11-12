using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSHAgentFramework
{
    public static class WireUtils
    {
        public static bool TryWriteUintToWire(Span<byte> dest, uint n)
        {
            return BitConverter.TryWriteBytes(dest, n) && NativeToWireEndianness(dest);
        }
        public static uint ReadUintFromWire(Span<byte> src)
        {
            return BitConverter.ToUInt32(WireToNativeEndianness(src));
        }
        public static ulong ReadUlongFromWire(Span<byte> src)
        {
            return BitConverter.ToUInt64(WireToNativeEndianness(src));
        }

        // SSH uses network byte order (AKA big endian)
        public static bool NativeToWireEndianness(Span<byte> nativeSpan)
        {
            if (BitConverter.IsLittleEndian)
            {
                var wireBytes = nativeSpan.ToArray().Reverse().ToArray();
                return new Span<byte>(wireBytes).TryCopyTo(nativeSpan);
            }
            // else, no need to do anything
            return true;
        }

        public static byte[] WireToNativeEndianness(ReadOnlySpan<byte> wireSpan)
        {
            var buff = wireSpan.ToArray();
            if (BitConverter.IsLittleEndian)
            {
                return buff.Reverse().ToArray();
            }
            return buff;
        }

        public static byte[] NullTerminate(string str)
        {
            return Encoding.ASCII.GetBytes(str)
                .Concat(new byte[] { 0 })
                .ToArray();
        }
    }
}

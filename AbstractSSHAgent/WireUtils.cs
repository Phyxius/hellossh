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
        public static byte[] EncodeUint(uint n)
        {
            var buff = new Span<byte>(BitConverter.GetBytes(n));
            NativeToWireEndianness(buff);
            return buff.ToArray();
        }

        public static byte[] EncodeToMPInt(uint n)
        {
            return EncodeToMPInt(BitConverter.GetBytes(n), true);
        }

        public static byte[] EncodeToMPInt(byte[] buff, bool handleEndianness = false)
        {
            if (handleEndianness && !BitConverter.IsLittleEndian) 
                buff = buff.Reverse().ToArray();
            buff = buff.SkipWhile(b => b == 0)
                .ToArray();
            if (buff.Length > 0 && (buff[0] & 0b1000_0000) != 0)
                buff = (new byte[] { 0 }).Concat(buff).ToArray();
            return EncodeString(buff);
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

        public static byte[] EncodeString(string str)
        {
            return EncodeString(Encoding.ASCII.GetBytes(str));
        }

        public static byte[] EncodeString(byte[] str)
        {
            return EncodeUint((uint)str.Length)
                .Concat(str)
                .ToArray();
        }
    }
}

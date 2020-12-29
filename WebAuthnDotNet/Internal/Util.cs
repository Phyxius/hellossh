using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    public static class Util
    {
        //https://stackoverflow.com/a/3278908
        public static byte[] MarshalToBytes(object obj)
        {
            var size = Marshal.SizeOf(obj);
            var ret = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, ret, 0, size);
            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        //https://stackoverflow.com/a/3278908
        public static void MarshalFromBytes(byte[] bytes, ref object obj)
        {
            int len = Marshal.SizeOf(obj);
            IntPtr i = Marshal.AllocHGlobal(len);
            Marshal.Copy(bytes, 0, i, len);
            obj = Marshal.PtrToStructure(i, obj.GetType());
            Marshal.FreeHGlobal(i);
        }
    }
}

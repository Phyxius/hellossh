using System.IO;
using System.Runtime.InteropServices;

namespace HelloSSH.Agent
{
    struct BCryptKeyBlob
    {
        // from: https://github.com/tpn/winsdk-10/blob/master/Include/10.0.14393.0/shared/bcrypt.h
        // https://docs.microsoft.com/en-us/windows/win32/api/bcrypt/ns-bcrypt-bcrypt_rsakey_blob
        const uint BCRYPT_RSAPUBLIC_MAGIC = 0x31415352; //"RSA1"
        const uint BCRYPT_RSAPRIVATE_MAGIC = 0x32415352;  // "RSA2"
        const uint BCRYPT_RSAFULLPRIVATE_MAGIC = 0x33415352;  // "RSA3";

        //these fields are never directly assigned to directly, but the marshaler will assign to them
#pragma warning disable 0649
        public uint Magic;
        public uint BitLength;
        public uint cbPublicExp;
        public uint cbModulus;
        public uint cbPrime1;
        public uint cbPrime2;
#pragma warning restore 0649

        public static BCryptKeyBlob FromStream(Stream stream)
        {
            var blob = new BCryptKeyBlob();
            var size = Marshal.SizeOf(blob);
            var ptr = Marshal.AllocHGlobal(size);
            var buffer = new byte[size];
            stream.Read(buffer, 0, size);
            Marshal.Copy(buffer, 0, ptr, size);
            blob = (BCryptKeyBlob)Marshal.PtrToStructure(ptr, blob.GetType());
            Marshal.FreeHGlobal(ptr);
            return blob;
        }
    }
}

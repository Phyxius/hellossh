namespace SSHAgentFramework
{
    public class ClientSignRequestMessage
    {
        public byte[] KeyBlob, Challenge;
        public uint Flags;
        public static ClientSignRequestMessage Deserialize(byte[] buff)
        {
            var ret = new ClientSignRequestMessage();
            int index = 0;
            int length = (int)WireUtils.ReadUintFromWire(buff[index..(index + sizeof(uint))]);
            index += sizeof(uint);
            ret.KeyBlob = new byte[length];
            buff[index..(index + length)].CopyTo(ret.KeyBlob, 0);
            index += length;
            length = (int)WireUtils.ReadUintFromWire(buff[index..(index + sizeof(uint))]);
            index += sizeof(uint);
            ret.Challenge = new byte[length];
            buff[index..(index + length)].CopyTo(ret.Challenge, 0);
            index += length;
            ret.Flags = WireUtils.ReadUintFromWire(buff[index..(index + sizeof(uint))]);
            return ret;
        }
    }

}

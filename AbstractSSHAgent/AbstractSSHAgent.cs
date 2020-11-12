using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SSHAgentFramework
{
    public class AbstractSSHAgent
    {
        public virtual IAgentMessage ProcessMessage(AgentMessage message)
        {
            return new AgentFailureMessage();
        }

        public void ListenOnNamedPipe(string pipeName)
        {
            while (true)
            {
                using var pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut);
                pipeServer.WaitForConnection();
                HandleClient(pipeServer);
            }
        }

        private void HandleClient(NamedPipeServerStream pipeServer)
        {
            Console.WriteLine("Connected!");
            var lengthBytes = new byte[sizeof(uint)];
            int readBytes = pipeServer.Read(lengthBytes, 0, lengthBytes.Length);
            if (readBytes != lengthBytes.Length)
            {
                Console.WriteLine($"Malformed message! Expecting {lengthBytes.Length}, got {readBytes}.");
                return;
            }
            var length = WireUtils.ReadUintFromWire(lengthBytes[..]);
            var fullMessage = new byte[lengthBytes.Length + length];
            lengthBytes.CopyTo(fullMessage, 0);
            readBytes = pipeServer.Read(fullMessage, lengthBytes.Length, (int)length);
            if (readBytes != length)
            {
                Console.WriteLine($"Malformed message! Expecting {length}, got {readBytes}.");
                return;
            }
            var response = ProcessMessage(AgentMessage.Deserialize(fullMessage));
            if (response != null)
            {
                pipeServer.Write(response.ToAgentMessage().Serialize());
            }
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;

namespace SSHAgentFramework
{
    public class AbstractSSHAgent
    {
        public bool IsCanceled { get; private set; }
        public virtual IAgentMessage ProcessMessage(AgentMessage message)
        {
            return new AgentFailureMessage();
        }

        public void ListenOnNamedPipe(string pipeName)
        {
            while (!IsCanceled)
            {
                var pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
                pipeServer.WaitForConnection();
                new Thread(HandleClientThread).Start(pipeServer);
            }
        }

        public void Cancel() => IsCanceled = true;

        private void HandleClientThread(object o)
        {
            using var pipeServer = (NamedPipeServerStream)o;
            while (pipeServer.IsConnected && !IsCanceled)
            {
                HandleClient(pipeServer);
            }
        }
        private void HandleClient(NamedPipeServerStream pipeServer)
        {
            Console.WriteLine("Connected!");
            var lengthBytes = new byte[sizeof(uint)];
            int readBytes = pipeServer.Read(lengthBytes, 0, lengthBytes.Length);
            if (readBytes == 0)
            {
                // empty/closed connection
                return;
            }
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
                byte[] buffer = response.ToAgentMessage().Serialize();
                Console.WriteLine("> " + BitConverter.ToString(buffer).Replace("-", ""));
                pipeServer.Write(buffer);
            }
        }
    }
}

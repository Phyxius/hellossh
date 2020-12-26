using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;

namespace SSHAgentFramework
{
    public class AbstractSSHAgent
    {
        public bool IsCanceled { get; private set; }
        public virtual IAgentMessage ProcessMessage(AgentMessage message, UInt32 clientProcessId)
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

        // adapted from https://stackoverflow.com/questions/15896315/get-process-id-of-a-client-that-connected-to-a-named-pipe-server-with-c-sharp
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetNamedPipeClientProcessId(IntPtr Pipe, out uint ClientProcessId);
        private static uint getNamedPipeClientProcID(NamedPipeServerStream pipeServer)
        {
            var hPipe = pipeServer.SafePipeHandle.DangerousGetHandle();
            if (GetNamedPipeClientProcessId(hPipe, out uint nProcID))
                return nProcID;
            return 0;
        }

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
            var clientProcId = getNamedPipeClientProcID(pipeServer);
            var response = ProcessMessage(AgentMessage.Deserialize(fullMessage), clientProcId);
            if (response != null)
            {
                byte[] buffer = response.ToAgentMessage().Serialize();
                Console.WriteLine("> " + BitConverter.ToString(buffer).Replace("-", ""));
                pipeServer.Write(buffer);
            }
        }
    }
}

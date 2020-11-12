using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSHAgentFramework
{
    public class AgentIdentitiesAnswerMessage : IAgentMessage
    {
        public List<(string, string)> Keys;

        public AgentMessage ToAgentMessage()
        {
            var numKeys = new Span<byte>(new byte[sizeof(uint)]);
            WireUtils.TryWriteUintToWire(numKeys, (uint)Keys.Count);
            var encodedKeys = Keys.SelectMany(KeyAndCommentToWireEncoding).ToArray();

            return new AgentMessage
            {
                Type = AgentMessageType.SSH_AGENT_IDENTITIES_ANSWER,
                Contents = numKeys.ToArray().Concat(encodedKeys).ToArray()
            };
        }

        private static byte[] KeyAndCommentToWireEncoding((string, string) tuple)
        {
            return WireUtils.NullTerminate(tuple.Item1)
                .Concat(WireUtils.NullTerminate(tuple.Item2))
                .ToArray();
        }
    }
}

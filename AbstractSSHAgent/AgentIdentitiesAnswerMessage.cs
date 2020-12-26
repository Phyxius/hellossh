using System;
using System.Collections.Generic;
using System.Linq;

namespace SSHAgentFramework
{
    public class AgentIdentitiesAnswerMessage : IAgentMessage
    {
        public List<(SSHPublicKey, string)> Keys;

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

        private static byte[] KeyAndCommentToWireEncoding((SSHPublicKey, string) tuple)
        {
            return tuple.Item1.Serialize()
                .Concat(WireUtils.EncodeString(tuple.Item2))
                .ToArray();
        }
    }
}

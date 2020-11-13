using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SSHAgentFramework
{
    public class AgentSignResponseMessage : IAgentMessage
    {
        public string Type;
        public byte[] Blob;
        public AgentMessage ToAgentMessage()
        {
            return new AgentMessage()
            {
                Type = AgentMessageType.SSH_AGENT_SIGN_RESPONSE,
                Contents = WireUtils.EncodeString(
                    WireUtils.EncodeString(Type)
                        .Concat(WireUtils.EncodeString(Blob))
                        .ToArray()
                    )
            };
        }
    }
}

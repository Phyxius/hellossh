using System;
using System.Collections.Generic;
using System.Text;

namespace SSHAgentFramework
{
    public class AgentFailureMessage : IAgentMessage
    {
        public AgentMessage ToAgentMessage()
        {
            return new AgentMessage
            {
                Type = AgentMessageType.SSH_AGENT_FAILURE,
                Contents = new byte[0]
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SSHAgentFramework
{
    class AgentFailureMessage : IAgentMessage
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

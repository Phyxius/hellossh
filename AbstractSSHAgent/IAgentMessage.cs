using System;
using System.Collections.Generic;
using System.Text;

namespace SSHAgentFramework
{
    public interface IAgentMessage
    {
        AgentMessage ToAgentMessage();
    }
}

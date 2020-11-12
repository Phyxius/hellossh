using System;
using System.Collections.Generic;
using System.Text;

namespace SSHAgentFramework
{
    enum AgentKeyConstraintIdentifier : byte
    {
        SSH_AGENT_CONSTRAIN_LIFETIME = 1,
        SSH_AGENT_CONSTRAIN_CONFIRM = 2,
        SSH_AGENT_CONSTRAIN_EXTENSION = 3
    }
}

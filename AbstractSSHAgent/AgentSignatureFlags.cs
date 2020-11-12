using System;
using System.Collections.Generic;
using System.Text;

namespace SSHAgentFramework
{
    [Flags]
    enum AgentSignatureFlags : byte
    {
        SSH_AGENT_RSA_SHA2_256 = 2,
        SSH_AGENT_RSA_SHA2_512 = 4
    }
}

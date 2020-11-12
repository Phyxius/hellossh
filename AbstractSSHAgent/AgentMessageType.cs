﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SSHAgentFramework
{
    public enum AgentMessageType : byte
    {
        SSH_AGENTC_REQUEST_IDENTITIES = 11,
        SSH_AGENTC_SIGN_REQUEST = 13,
        SSH_AGENTC_ADD_IDENTITY = 17,
        SSH_AGENTC_REMOVE_IDENTITY = 18,
        SSH_AGENTC_REMOVE_ALL_IDENTITIES = 19,
        SSH_AGENTC_ADD_ID_CONSTRAINED = 25,
        SSH_AGENTC_ADD_SMARTCARD_KEY = 20,
        SSH_AGENTC_REMOVE_SMARTCARD_KEY = 21,
        SSH_AGENTC_LOCK = 22,
        SSH_AGENTC_UNLOCK = 23,
        SSH_AGENTC_ADD_SMARTCARD_KEY_CONSTRAINED = 26,
        SSH_AGENTC_EXTENSION = 27,
        SSH_AGENT_FAILURE = 5,
        SSH_AGENT_SUCCESS = 6,
        SSH_AGENT_EXTENSION_FAILURE = 28,
        SSH_AGENT_IDENTITIES_ANSWER = 12,
        SSH_AGENT_SIGN_RESPONSE = 14,
    }
}

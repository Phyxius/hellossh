namespace SSHAgentFramework
{
    class ClientRequestIdentitiesMessage : IAgentMessage
    {
        public AgentMessage ToAgentMessage()
        {
            return new AgentMessage
            {
                Contents = new byte[0],
                Type = AgentMessageType.SSH_AGENTC_REQUEST_IDENTITIES
            };
        }
    }
}

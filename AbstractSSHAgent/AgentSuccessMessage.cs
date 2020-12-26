namespace SSHAgentFramework
{
    class AgentSuccessMessage : IAgentMessage
    {
        public AgentMessage ToAgentMessage()
        {
            return new AgentMessage
            {
                Type = AgentMessageType.SSH_AGENT_SUCCESS,
                Contents = new byte[0]
            };
        }
    }
}

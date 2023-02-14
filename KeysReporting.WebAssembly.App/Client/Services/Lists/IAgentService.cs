using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Client.Services.Lists
{
    public interface IAgentService
    {
        Task<List<AgentListDto>> GetAgentAsync();
    }
}

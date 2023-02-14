using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.Lists
{
    public class AgentService : IAgentService
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public AgentService(CallDispositionContext callDispositionContext, IMapper mapper)
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        public async Task<List<AgentListDto>> GetAgentAsync()
        {
            var dbModel = await _callDispositionContext.Agents
                 .Where(x => x.CallDispositions.Any(i => i.FkFtpfileNavigation.LastWriteTime > DateTime.Today.AddMonths(-1)))
                 .OrderBy(x => x.AgentId)
                 .ToListAsync();

            return _mapper.Map<List<AgentListDto>>(dbModel);
        }
    }
}

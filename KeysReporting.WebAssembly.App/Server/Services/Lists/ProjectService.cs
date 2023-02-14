using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.Lists
{
    public class ProjectService : IProjectService
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public ProjectService(CallDispositionContext callDispositionContext, IMapper mapper)
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        public async Task<List<ProjectListDto>> GetProjectAsync()
        {
            var dbModel = await _callDispositionContext.ProjectCodes
                .Where(x => x.CallDispositions.Any(i => i.FkFtpfileNavigation.LastWriteTime > DateTime.Today.AddMonths(-1)))
                .OrderBy(x => x.ProjectCode1)
                .ToListAsync();

            return _mapper.Map<List<ProjectListDto>>(dbModel);
        }
    }
}

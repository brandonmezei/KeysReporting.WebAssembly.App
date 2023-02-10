using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.Lists
{
    public class SourceTableService : ISourceTableService
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public SourceTableService(CallDispositionContext callDispositionContext, IMapper mapper) 
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        public async Task<List<SourceTableListDto>> GetSourceTableAsync()
        {
            var dbModel = await _callDispositionContext.SourceTables
                .Where(x => x.CallDispositions.Any(i => i.FkFtpfileNavigation.LastWriteTime > DateTime.Today.AddMonths(-1)))
                .OrderBy(x => x.SourceTable1)
                .ToListAsync();

            return _mapper.Map<List<SourceTableListDto>>(dbModel);
        }
    }
}

using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.TermCodes;

namespace KeysReporting.WebAssembly.App.Server.Services.Reports.TermCodes
{
    public class TermCodeReportService : ITermCodeReportService
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public TermCodeReportService(CallDispositionContext callDispositionContext, IMapper mapper)
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        public Task<List<TermCodeReportDto>> GetReportAsync(SearchDto searchDto)
        {
            
        }
    }
}

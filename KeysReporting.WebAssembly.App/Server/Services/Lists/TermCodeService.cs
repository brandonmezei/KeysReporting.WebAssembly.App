using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.TermCodes;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.Lists
{
    public class TermCodeService : ITermCodeService
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public TermCodeService(CallDispositionContext callDispositionContext, IMapper mapper)
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        public async Task<List<TermCodeDto>> GetTermCodesAsync()
        {
            var termCodes = await _callDispositionContext.TermCodes
                .Include(x => x.FkTermCodeCategoryNavigation)
                .OrderBy(x => x.Alias)
                .ToListAsync();

            return _mapper.Map<List<TermCodeDto>>(termCodes);
        }
    }
}

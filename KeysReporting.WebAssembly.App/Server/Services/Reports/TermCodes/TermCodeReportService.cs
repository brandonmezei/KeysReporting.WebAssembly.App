using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.Lists;
using KeysReporting.WebAssembly.App.Shared.TermCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<List<TermCodeReportDto>> GetReportAsync(TermCodeSearchDto searchDto)
        {
            if (string.IsNullOrEmpty(searchDto.Account))
                return new List<TermCodeReportDto>();

            return _mapper.Map<List<TermCodeReportDto>>(
                await _callDispositionContext.CallDispositions
                .Include(x => x.FkProjectCodeNavigation)
                .Include(x => x.FkFtpfileNavigation)
                .Include(x => x.FkTermCodeNavigation)
                    .ThenInclude(x => x.FkTermCodeCategoryNavigation)
                .Where(x => x.Account.ToLower() == searchDto.Account.ToLower())
                .ToListAsync()
                );
        }

        public async Task<List<TermCodeReportDto>> UpdateReport(TermCodeEditDto editDto)
        {
            var editLine = await _callDispositionContext.CallDispositions
                .Where(x => x.Id == editDto.Id)
                .FirstOrDefaultAsync();

            if(editLine == null)
                return new List<TermCodeReportDto>();

            _mapper.Map(editDto, editLine);

            await _callDispositionContext.SaveChangesAsync();

            return await GetReportAsync(new TermCodeSearchDto { Account = editLine.Account });
        }
    }
}

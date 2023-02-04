using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.CPH;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.CPHReport
{
    public class CPHReport : ICPHReport
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public CPHReport(CallDispositionContext callDispositionContext, IMapper mapper) 
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        public async Task<CPHReportDto> GetReportAsync(SearchDto searchDto)
        {

            var returnDto = new CPHReportDto();

            if (!searchDto.SearchDate.HasValue)
                return returnDto;

            //Create if Doesn't Exist
            if (!_callDispositionContext.Cphheaders.Where(x => x.ReportDate == searchDto.SearchDate).Any())
            {
                await _callDispositionContext.Cphheaders.AddAsync(new Cphheader
                {
                    ReportDate = searchDto.SearchDate.Value.Date
                });

                await _callDispositionContext.SaveChangesAsync();
            }

            var dbModel = await _callDispositionContext.Cphheaders
                .Where(x => x.ReportDate == searchDto.SearchDate)
                .FirstOrDefaultAsync();

            returnDto = _mapper.Map<CPHReportDto>(dbModel);
            
            return returnDto;

        }
    }
}

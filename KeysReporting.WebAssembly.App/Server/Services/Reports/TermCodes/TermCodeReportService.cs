using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.Lists;
using KeysReporting.WebAssembly.App.Shared.TermCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.SqlServer.Server;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<List<TermCodeReportDto>> CreateTermAsync(TermCodeAddDto termCodeAddDto)
        {
            var ftpFile = await _callDispositionContext.Ftpcontrols
                .Where(x => x.FileName.ToLower() == "correction" && x.LastWriteTime == termCodeAddDto.FileDate)
                .FirstOrDefaultAsync();

            if (ftpFile == null)
            {
                ftpFile = new Ftpcontrol
                {
                    FileName = "Correction",
                    LastWriteTime = termCodeAddDto.FileDate.Value,
                    Result = "OK",
                    RecordCount = 0
                };

                _callDispositionContext.Ftpcontrols.Add(ftpFile);
            }

            var sourceTable = await _callDispositionContext.CallDispositions
                .Include(x => x.FkSourceTableNavigation)
                .Where(x => x.FkProjectCode == termCodeAddDto.ProjectID && x.FkSourceTable.HasValue)
                .FirstOrDefaultAsync();

            var newCallFile = new CallDisposition() {
                FkClientNavigation = await _callDispositionContext.Clients.Where(x => x.ClientName.ToLower() == "keys360").FirstOrDefaultAsync(),
                FkSourceTableNavigation = sourceTable.FkSourceTableNavigation,
                UpdateDate = DateTime.Now,
                UpdateId = 1
            };

            ftpFile.CallDispositions.Add(_mapper.Map(termCodeAddDto, newCallFile));

            await _callDispositionContext.SaveChangesAsync();

            return await GetReportAsync(new TermCodeSearchDto { Account = termCodeAddDto.Account });
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

        public async Task<List<TermCodeReportDto>> UpdateReportAsync(TermCodeEditDto editDto)
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

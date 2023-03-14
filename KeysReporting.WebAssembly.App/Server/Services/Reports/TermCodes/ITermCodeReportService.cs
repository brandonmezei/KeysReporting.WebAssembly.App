using KeysReporting.WebAssembly.App.Shared.TermCodes;

namespace KeysReporting.WebAssembly.App.Server.Services.Reports.TermCodes
{
    public interface ITermCodeReportService
    {
        Task<List<TermCodeReportDto>> GetReportAsync(TermCodeSearchDto searchDto);

        Task<List<TermCodeReportDto>> UpdateReportAsync(TermCodeEditDto editDto);

        Task<List<TermCodeReportDto>> CreateTermAsync(TermCodeAddDto termCodeAddDto);
    }
}

using KeysReporting.WebAssembly.App.Shared.TermCodes;

namespace KeysReporting.WebAssembly.App.Server.Services.Reports.TermCodes
{
    public interface ITermCodeReportService
    {
        Task<List<TermCodeReportDto>> GetReportAsync(SearchDto searchDto);

        Task<List<TermCodeReportDto>> UpdateReport(TermCodeEditDto editDto);
    }
}

using KeysReporting.WebAssembly.App.Shared.TermCodes;

namespace KeysReporting.WebAssembly.App.Client.Services.TermCodes
{
    public interface ITermCodeService
    {
        Task<List<TermCodeReportDto>> GetTermCodeReportAsync(SearchDto searchDto);
    }
}

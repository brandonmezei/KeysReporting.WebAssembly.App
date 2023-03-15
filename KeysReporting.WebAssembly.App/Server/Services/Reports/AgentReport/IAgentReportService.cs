using KeysReporting.WebAssembly.App.Shared.Agent;

namespace KeysReporting.WebAssembly.App.Server.Services.Reports.AgentReport
{
    public interface IAgentReportService
    {
        Task<AgentReportDto> GetReportAsync(SearchDto searchDto);
        Task<byte[]> GetReportDownloadAsync(SearchDto searchDto);
        Task<byte[]> GetReportDownloadTotalsAsync(SearchDto searchDto);
        Task<byte[]> GetReportDownloadCompareTotalsAsync(SearchDto searchDto);
    }
}

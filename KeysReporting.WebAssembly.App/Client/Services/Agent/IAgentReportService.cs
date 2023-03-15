using KeysReporting.WebAssembly.App.Shared.Agent;
using KeysReporting.WebAssembly.App.Shared.File;
using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Client.Services.Agent
{
    public interface IAgentReportService
    {
        Task<AgentReportDto> GetAgentReportAsync(SearchDto searchDto);
        Task<ServiceFileDto> DownloadReportFileAsync(SearchDto searchDto);
        Task<ServiceFileDto> DownloadTotalReportFileAsync(SearchDto searchDto);
        Task<ServiceFileDto> DownloadCompareTotalReportFileAsync(SearchDto searchDto);
    }
}

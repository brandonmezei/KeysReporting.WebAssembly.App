using KeysReporting.WebAssembly.App.Shared.CPH;

namespace KeysReporting.WebAssembly.App.Server.Services.CPHReport
{
    public interface ICPHReport
    {
        Task<CPHReportDto> GetReportAsync(SearchDto searchDto);
    }
}

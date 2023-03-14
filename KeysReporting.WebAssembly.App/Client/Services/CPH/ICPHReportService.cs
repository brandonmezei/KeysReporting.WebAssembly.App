using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.File;
using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Client.Services.CPH
{
    public interface ICPHReportService
    {
        Task<CPHReportDto> GetCPHReportAsync(SearchDto searchDto);
        Task<List<ProjectListDto>> GetProjectListAsync(DateTime reportDate);

        Task<ProjectListDto> CreateProjectAsync(AddProjectDto addProjectDto);
        Task<CPHReportDto> EditCPHReportTimeAsync(EditTimeDto editTimeDto);
        Task<CPHReportDto> DeleteProjectAsync(DeleteProjectDto deleteProjectDto);
        Task<CPHReportDto> EditCPH(EditCPHDto editCPHDto);
        Task<ServiceFileDto> DownloadFile(SearchDto searchDto);
    }
}

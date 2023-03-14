using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Server.Services.CPHReport
{
    public interface ICPHReportService
    {
        Task<List<ProjectListDto>> GetProjectListAsync(DateTime reportTime);
        Task<CPHReportDto> GetReportAsync(SearchDto searchDto);
        Task<ProjectListDto> CreateNewProjectAsync(AddProjectDto addProjectDto);

        Task<CPHReportDto> EditTimeLineAsync(EditTimeDto editTimeDto);

        Task<CPHReportDto> DeleteProjectAsync(DeleteProjectDto deleteProjectDto);
        Task<CPHReportDto> EditCPH(EditCPHDto editCPHDto);

        Task<byte[]> GetAllCPHAsync(SearchDto searchDto);

    }
}

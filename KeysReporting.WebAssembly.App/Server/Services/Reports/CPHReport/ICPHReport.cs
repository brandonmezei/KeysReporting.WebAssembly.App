using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Server.Services.CPHReport
{
    public interface ICPHReport
    {
        Task<List<ProjectListDto>> GetProjectListAsync(DateTime reportTime);
        Task<CPHReportDto> GetReportAsync(SearchDto searchDto);
        Task<ProjectListDto> CreateNewProject(AddProjectDto addProjectDto);
    }
}

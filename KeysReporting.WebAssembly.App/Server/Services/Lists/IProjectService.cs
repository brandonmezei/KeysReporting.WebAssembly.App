using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Server.Services.Lists
{
    public interface IProjectService
    {
        Task<List<ProjectListDto>> GetProjectAsync();
    }
}

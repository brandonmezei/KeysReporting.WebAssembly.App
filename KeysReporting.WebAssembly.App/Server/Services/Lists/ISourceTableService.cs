using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Server.Services.Lists
{
    public interface ISourceTableService
    {
        Task<List<SourceTableListDto>> GetSourceTableAsync();
    }
}

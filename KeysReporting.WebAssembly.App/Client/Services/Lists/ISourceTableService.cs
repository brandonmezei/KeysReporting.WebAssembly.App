using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Client.Services.Lists
{
    public interface ISourceTableService
    {
        Task<List<SourceTableListDto>> GetSourceTableAsync();
    }
}

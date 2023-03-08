using KeysReporting.WebAssembly.App.Shared.Lists;
using KeysReporting.WebAssembly.App.Shared.TermCodes;

namespace KeysReporting.WebAssembly.App.Client.Services.Lists
{
    public interface ITermCodeListService
    {
        Task<List<TermCodeDto>> GetTermCodesAsync();
    }
}

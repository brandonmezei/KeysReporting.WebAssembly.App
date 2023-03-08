using KeysReporting.WebAssembly.App.Shared.Lists;
using KeysReporting.WebAssembly.App.Shared.TermCodes;

namespace KeysReporting.WebAssembly.App.Server.Services.Lists
{
    public interface ITermCodeService
    {
        public Task<List<TermCodeDto>> GetTermCodesAsync();
    }
}

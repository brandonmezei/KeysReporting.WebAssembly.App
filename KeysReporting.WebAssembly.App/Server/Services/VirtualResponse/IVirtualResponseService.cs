using KeysReporting.WebAssembly.App.Shared.VirtualResponse;

namespace KeysReporting.WebAssembly.App.Server.Services.VirtualResponse
{
    public interface IVirtualResponseService<T> where T : class
    {
        Task<VirtualResponseDto<TResult>> GetAllAsync<TResult>(QueryParamDto queryParm) where TResult : class;
    }


}

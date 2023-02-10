using KeysReporting.WebAssembly.App.Shared.Auth;

namespace KeysReporting.WebAssembly.App.Server.Services.LiveVoxAPI
{
    public interface ILiveVoxAPIService
    {
        Task<UserLoginResponseDto> LoginAsync(UserLoginDto userLoginDto);
    }
}

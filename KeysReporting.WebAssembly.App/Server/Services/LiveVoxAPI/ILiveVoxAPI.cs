using KeysReporting.WebAssembly.App.Shared.Auth;

namespace KeysReporting.WebAssembly.App.Server.Services.LiveVoxAPI
{
    public interface ILiveVoxAPI
    {
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    }
}

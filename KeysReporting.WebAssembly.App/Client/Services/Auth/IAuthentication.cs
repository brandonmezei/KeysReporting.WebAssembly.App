using KeysReporting.WebAssembly.App.Shared.Auth;

namespace KeysReporting.WebAssembly.App.Client.Services.Auth
{
    public interface IAuthentication
    {
        Task<bool> LogIn(UserLoginDto userLoginDto);
    }
}

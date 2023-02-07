using KeysReporting.WebAssembly.App.Shared.Auth;

namespace KeysReporting.WebAssembly.App.Client.Services.Auth
{
    public interface IAuthenticationService
    {
        Task<bool> LogIn(UserLoginDto userLoginDto);
        Task Logout();
        Task<bool> CheckLogin();
    }
}

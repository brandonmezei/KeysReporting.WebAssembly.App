using KeysReporting.WebAssembly.App.Client.Services.Auth;
using Microsoft.AspNetCore.Components;

namespace KeysReporting.WebAssembly.App.Client.Shared
{
    public partial class NavMenu
    {

        [Inject]
        private IAuthentication _authService { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }
        private void SignOut()
        {
            _authService.Logout();
            _navManager.NavigateTo("/");
        }
    }
}

using KeysReporting.WebAssembly.App.Client.Services.Auth;
using Microsoft.AspNetCore.Components;

namespace KeysReporting.WebAssembly.App.Client.Shared
{
    public partial class NavMenu
    {

        [Inject]
        private IAuthentication AuthService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }
        private void SignOut()
        {
            AuthService.Logout();
            NavManager.NavigateTo("/");
        }
    }
}

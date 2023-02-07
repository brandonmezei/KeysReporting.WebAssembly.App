using KeysReporting.WebAssembly.App.Client.Services.Auth;
using Microsoft.AspNetCore.Components;

namespace KeysReporting.WebAssembly.App.Client.Pages
{
    public partial class Dashboard
    {
        [Inject]
        private IAuthenticationService AuthService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!await AuthService.CheckLogin())
                NavManager.NavigateTo("/");
        }
    }
}

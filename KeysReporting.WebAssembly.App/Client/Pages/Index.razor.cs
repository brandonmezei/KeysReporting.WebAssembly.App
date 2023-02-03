using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Shared.Auth;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace KeysReporting.WebAssembly.App.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private IAuthentication AuthService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        private UserLoginDto _userLogin = new();
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            if(await AuthService.CheckLogin())
                NavManager.NavigateTo("Dashboard");
        }

        private async Task HandleLogin()
        {
            try
            {
                if (!await AuthService.LogIn(_userLogin))
                    _errorMessage = Static.Messages.LoginError;
                else
                    NavManager.NavigateTo("Dashboard");
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
        }
    }
}

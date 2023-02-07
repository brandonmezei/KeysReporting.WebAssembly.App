using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Shared.Auth;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace KeysReporting.WebAssembly.App.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private IAuthenticationService AuthService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        private UserLoginDto _userLogin = new();
        private string? _errorMessage;
        private string? _buttonClass;

        protected override async Task OnInitializedAsync()
        {
            if(await AuthService.CheckLogin())
                NavManager.NavigateTo("Dashboard");
        }

        private async Task HandleLogin()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            
            try
            {
                if (!await AuthService.LogIn(_userLogin))
                    _errorMessage = Messages.LoginError;
                else
                    NavManager.NavigateTo("Dashboard");
            }
            catch
            {
                _errorMessage = Messages.LoginError;
            }

            _buttonClass = string.Empty;
        }
    }
}

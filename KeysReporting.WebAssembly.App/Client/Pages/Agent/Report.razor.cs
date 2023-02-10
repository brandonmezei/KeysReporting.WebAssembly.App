using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.CPH;
using KeysReporting.WebAssembly.App.Client.Services.Lists;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Shared.Agent;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.AspNetCore.Components;

namespace KeysReporting.WebAssembly.App.Client.Pages.Agent
{
    public partial class Report
    {
        [Inject]
        private IAuthenticationService AuthService { get; set; }

        [Inject]
        private ISourceTableService SourceTableService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        private SearchDto _search = new();
        private List<SourceTableListDto> _sourceTables = new();


        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;

        protected override async Task OnInitializedAsync()
        {
            if (!await AuthService.CheckLogin())
                NavManager.NavigateTo("/");

            var responseSourceTable = await SourceTableService.GetSourceTableAsync();

            if (responseSourceTable != null)
                _sourceTables = responseSourceTable;
        }

        private async Task HandleSearch()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.LoadingReport;
            _errorMessage = string.Empty;

            try
            {

            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

    }
}

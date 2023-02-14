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
        private IProjectService ProjectService { get; set; }

        [Inject]
        private IAgentService AgentService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        private SearchDto _search = new();
        private List<SourceTableListDto> _sourceTables = new();
        private List<ProjectListDto> _projectTable = new();
        private List<AgentListDto> _agentTable = new();


        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;

        protected override async Task OnInitializedAsync()
        {
            if (!await AuthService.CheckLogin())
                NavManager.NavigateTo("/");

            var responseSourceTable = await SourceTableService.GetSourceAsync();

            if (responseSourceTable != null)
                _sourceTables = responseSourceTable;

            var responseProject = await ProjectService.GetProjectAsync();

            if (responseProject != null)
                _projectTable = responseProject;

            var responseAgent = await AgentService.GetAgentAsync();

            if (responseAgent != null)
                _agentTable = responseAgent;
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

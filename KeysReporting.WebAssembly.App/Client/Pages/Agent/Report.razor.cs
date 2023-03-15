using KeysReporting.WebAssembly.App.Client.Services.Agent;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.CPH;
using KeysReporting.WebAssembly.App.Client.Services.Lists;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Shared.Agent;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        private IAgentReportService AgentReportService { get; set; }


        [Inject]
        private NavigationManager NavManager { get; set; }

        [Inject]
        private IJSRuntime JS { get; set; }

        private SearchDto _search = new();
        private AgentReportDto _agentReport;
        private List<SourceTableListDto> _sourceTables = new();
        private List<ProjectListDto> _projectTable = new();
        private List<AgentListDto> _agentTable = new();


        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;
        public string? _warnMessage;

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
            _warnMessage = string.Empty;

            try
            {
                _agentReport = await AgentReportService.GetAgentReportAsync(_search);

                _warnMessage = _agentReport.AgentLines.Any()
                    ? string.Empty
                    : Messages.NothingFound;
                    
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleDownloadReport()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.Downloading;
            _errorMessage = string.Empty;

            try
            {
                var response = await AgentReportService.DownloadReportFileAsync(_search);

                if (response != null)
                    await JS.InvokeVoidAsync("SaveByteArray", response.Name, "application/octet-stream", response.Content);
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleDownloadTotalReport()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.Downloading;
            _errorMessage = string.Empty;

            try
            {
                var response = await AgentReportService.DownloadTotalReportFileAsync(_search);

                if (response != null)
                    await JS.InvokeVoidAsync("SaveByteArray", response.Name, "application/octet-stream", response.Content);
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleDownloadCompareTotalReport()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.Downloading;
            _errorMessage = string.Empty;

            try
            {
                var response = await AgentReportService.DownloadCompareTotalReportFileAsync(_search);

                if (response != null)
                    await JS.InvokeVoidAsync("SaveByteArray", response.Name, "application/octet-stream", response.Content);
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private void HandleSourceChange(ChangeEventArgs e)
        {
            try
            {
                _search.SourceTable = string.IsNullOrEmpty(e.Value.ToString())
                    ? null
                    : long.Parse(e.Value.ToString());
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }
        }

        private void HandleProjectChange(ChangeEventArgs e)
        {
            try
            {
                _search.Project = string.IsNullOrEmpty(e.Value.ToString())
                    ? null
                    : new List<long>() { long.Parse(e.Value.ToString()) };
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }
        }

        private void HandleAgentChange(ChangeEventArgs e)
        {
            try
            {
                _search.Agent = string.IsNullOrEmpty(e.Value.ToString())
                    ? null
                    : long.Parse(e.Value.ToString());
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }
        }

    }
}

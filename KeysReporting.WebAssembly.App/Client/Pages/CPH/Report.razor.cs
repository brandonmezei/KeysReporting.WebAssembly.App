using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.CPH;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.AspNetCore.Components;

namespace KeysReporting.WebAssembly.App.Client.Pages.CPH
{
    public partial class Report
    {
        [Inject]
        private IAuthenticationService AuthService { get; set; }

        [Inject]
        private ICPHReportService ReportService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        private SearchDto _search = new() { SearchDate = DateTime.Today };
        private CPHReportDto _reportResponse;
        private List<ProjectListDto> _projectList = new();
        private AddProjectDto _addNewProject = new();
        private CPHReportLineDto _editTimeDto;

        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;

        public bool _addNewCampaign;
        public bool _editTime;
        public bool _firstLoad = true;

        protected override async Task OnInitializedAsync()
        {
            if (!await AuthService.CheckLogin())
                NavManager.NavigateTo("/");

            if (_firstLoad)
            {
                _firstLoad = false;
                await HandleSearch();
            }
        }

        private async Task HandleSearch()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.LoadingProjects;

            _reportResponse = null;
            _projectList = new();
            _search.ProjectID = null;

            try
            {
                //Load Project Dropdown
                var projectReponse = await ReportService.GetProjectListAsync(_search.SearchDate);

                if(projectReponse != null)
                    _projectList = projectReponse;

            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleAddProject()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.AddingProject;

            try
            {
                var response = await ReportService.CreateProjectAsync(_addNewProject);

                if (response != null)
                {
                    _search.ProjectID = response.Id;
                    _projectList.Add(response);
                    ToggleAddCampaign();
                    await BindReport();
                }
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleProjectChange(ChangeEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value.ToString()))
            {
                _reportResponse = null;  
            }
            else
            {
                _search.ProjectID = long.Parse(e.Value.ToString());
                await BindReport();
            }
        }

        private async Task HandleEditTime()
        {

        }

        private void ToggleEditTime(long? id)
        {
            _editTime = !_editTime;

            if (id.HasValue && _reportResponse.CPHLines?.Count > 0)
                _editTimeDto = _reportResponse.CPHLines
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
        }

        private void ToggleAddCampaign()
        {
            _addNewCampaign = !_addNewCampaign;
            _addNewProject.ReportDate = _search.SearchDate;
        }

        private async Task BindReport()
        {
            _loadingMessage = Messages.LoadingReport;

            try
            {
                _reportResponse = await ReportService.GetCPHReportAsync(_search);

                if(_reportResponse != null)
                {
                    _reportResponse.CPHLines = _reportResponse.CPHLines.OrderBy(x => x.Series).ToList();
                }
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
        }
    }
}

using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.CPH;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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

        [Inject]
        private IJSRuntime JS { get; set; }

        private SearchDto _search = new() { SearchDate = DateTime.Today };
        private CPHReportDto _reportResponse;
        private List<ProjectListDto> _projectList = new();
        private AddProjectDto _addNewProject = new();
        private CPHReportLineDto _editTimeDto;
        private EditCPHDto _editCPH = new();

        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;

        public bool _addNewCampaign;
        public bool _editTime;
        public bool _removeProjectWarn;
        public bool _resetWarn;

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

        private async Task BindReport()
        {
            _loadingMessage = Messages.LoadingReport;

            try
            {
                _reportResponse = await ReportService.GetCPHReportAsync(_search);

                if (_reportResponse.CPHLines.Any())
                    _reportResponse.CPHLines = _reportResponse.CPHLines.OrderBy(x => x.Series).ToList();

                _editCPH.SearchDate = _search.SearchDate;
                _editCPH.ProjectID = _reportResponse.Project?.Id;
                _editCPH.CPH = _reportResponse.CPH;
            }
            catch (Exception ex)
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
        }

        #region ButtonHandler
        private async Task HandleSearch()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.LoadingProjects;
            _errorMessage = string.Empty;

            _reportResponse = null;
            _projectList = new();
            _search.ProjectID = null;

            try
            {
                //Load Project Dropdown
                var projectReponse = await ReportService.GetProjectListAsync(_search.SearchDate);

                if (projectReponse != null)
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
            _errorMessage = string.Empty;

            try
            {
                var response = await ReportService.CreateProjectAsync(_addNewProject);

                if (response != null)
                {
                    _search.ProjectID = response.Id;

                    if (!_projectList.Any(x => x.Id == response.Id))
                        _projectList.Add(response);

                    ToggleAddroject();
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
            try
            {
                if (string.IsNullOrEmpty(e.Value.ToString()))
                {
                    _reportResponse = null;
                    _search.ProjectID = null;
                }
                else
                {
                    _search.ProjectID = long.Parse(e.Value.ToString());
                    await BindReport();
                }
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

        }

        private async Task HandleEditTime()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.SavingLine;
            _errorMessage = string.Empty;

            try
            {

                if (_editTimeDto.Agent >= 0)
                {
                    _reportResponse = await ReportService.EditCPHReportTimeAsync(new EditTimeDto { Id = _editTimeDto.Id, Agent = _editTimeDto.Agent });

                    if (_reportResponse.CPHLines.Any())
                        _reportResponse.CPHLines = _reportResponse.CPHLines.OrderBy(x => x.Series).ToList();

                    ToggleEditTime(null);
                }
                else
                    _errorMessage = Messages.NoNegatives;
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleDeleteProject()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.DeleteProject;
            _errorMessage = string.Empty;

            try
            {
                _reportResponse = await ReportService.DeleteProjectAsync(
                    new DeleteProjectDto { SearchDate = _search.SearchDate, ProjectID = _search.ProjectID });

                await HandleSearch();
                ToggleRemoveProject();
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleReset()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.SavingLine;
            _errorMessage = string.Empty;

            try
            {
                if (_reportResponse.CPHLines.Any())
                {
                    _editCPH.CPH = 10;

                    _reportResponse = await ReportService.EditCPH(_editCPH);
                    _reportResponse = await ReportService.EditCPHReportTimeAsync(new EditTimeDto { Id = _reportResponse.CPHLines.First().Id, Agent = 0 });

                    if (_reportResponse.CPHLines.Any())
                        _reportResponse.CPHLines = _reportResponse.CPHLines.OrderBy(x => x.Series).ToList();
                }

                ToggleReset();
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleEditCPH()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.SavingCPH;
            _errorMessage = string.Empty;

            try
            {
                _reportResponse = await ReportService.EditCPH(_editCPH);
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task JumpToNow()
        {
            await JS.InvokeVoidAsync("scrollIntoView", "currentRow");
        }
        #endregion


        #region Toggles
        private void ToggleEditTime(long? id)
        {
            _editTime = !_editTime;

            if (id.HasValue && _reportResponse.CPHLines?.Count > 0)
                _editTimeDto = _reportResponse.CPHLines
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
        }

        private void ToggleAddroject()
        {
            _addNewCampaign = !_addNewCampaign;
            _addNewProject = new();
            _addNewProject.ReportDate = _search.SearchDate;
        }

        private void ToggleRemoveProject()
        {
            _removeProjectWarn = !_removeProjectWarn;
        }

        private void ToggleReset()
        {
            _resetWarn = !_resetWarn;
        }
        #endregion

    }
}

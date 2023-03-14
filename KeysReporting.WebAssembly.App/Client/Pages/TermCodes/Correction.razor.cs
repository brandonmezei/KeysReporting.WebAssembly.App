using Microsoft.AspNetCore.Components;
using KeysReporting.WebAssembly.App.Shared.TermCodes;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Client.Services.TermCodes;
using System.Runtime;
using Microsoft.JSInterop;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.Lists;
using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Client.Pages.TermCodes
{
    public partial class Correction
    {
        [Inject]
        private IAuthenticationService AuthService { get; set; }

        [Inject]
        private ITermCodeService TermCodeService { get; set; }

        [Inject]
        private ITermCodeListService TermCodeListService { get; set; }

        [Inject]
        private IAgentService AgentService { get; set; }

        [Inject]
        private IProjectService ProjectService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        [Inject]
        private IJSRuntime JS { get; set; }

        private TermCodeSearchDto _search = new();
        private List<TermCodeReportDto> _termcodeReport = new();
        
        private TermCodeEditDto _termCodeEdit;
        private TermCodeAddDto _termCodeAdd;

        private List<TermCodeDto> _termCodeList = new();
        private List<AgentListDto> _agentList = new();
        private List<ProjectListDto> _projectList = new();

        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;
        public string? _warnMessage;
        public string? _successMessage;

        public bool _editTermCode;
        public bool _newTermCode;

        protected override async Task OnInitializedAsync()
        {
            if (!await AuthService.CheckLogin())
                NavManager.NavigateTo("/");

            var responseTermCodes = await TermCodeListService.GetTermCodesAsync();

            if (responseTermCodes != null)
                _termCodeList = responseTermCodes;

            var responseAgent = await AgentService.GetAgentAsync();

            if (responseAgent != null)
                _agentList = responseAgent;

            var responseProject = await ProjectService.GetProjectAsync();

            if (responseProject != null)
                _projectList = responseProject;
        }

        private async Task HandleSearch()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.LoadingReport;
            _errorMessage = string.Empty;

            try
            {
                _termcodeReport = await TermCodeService.GetTermCodeReportAsync(_search);

                _warnMessage = _termcodeReport?.Count > 0
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

        public async Task HandleEditTerm()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.SavingLine;
            _errorMessage = string.Empty;

            try
            {
                _termcodeReport = await TermCodeService.UpdateTermCodeReportAsync(_termCodeEdit);
                ToggleEdit(null);
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task HandleNewTerm()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _loadingMessage = Messages.SavingLine;
            _errorMessage = string.Empty;

            try
            {
                _termcodeReport = await TermCodeService.CreateTermCodeAsync(_termCodeAdd);
                ToggleNew();
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        public async Task HandleTermCodeChange(ChangeEventArgs e)
        {
            try
            {
                _termCodeEdit.TermCodeID = string.IsNullOrEmpty(e.Value.ToString())
                    ? null
                    : long.Parse(e.Value.ToString());
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }
        }

        public async Task HandleTermCodeNewChange(ChangeEventArgs e)
        {
            try
            {
                _termCodeAdd.TermCodeID = string.IsNullOrEmpty(e.Value.ToString())
                    ? null
                    : long.Parse(e.Value.ToString());
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }
        }

        public async Task AgentChange(ChangeEventArgs e)
        {
            try
            {
                _termCodeAdd.AgentID = string.IsNullOrEmpty(e.Value.ToString())
                    ? null
                    : long.Parse(e.Value.ToString());
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }
        }

        public async Task ProjectChange(ChangeEventArgs e)
        {
            try
            {
                _termCodeAdd.ProjectID = string.IsNullOrEmpty(e.Value.ToString())
                    ? null
                    : long.Parse(e.Value.ToString());
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }
        }

        private void ToggleEdit(long? id)
        {
            _editTermCode = !_editTermCode;

            if (id.HasValue && _termcodeReport.Any())
            {
                var editRecord = _termcodeReport
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (editRecord != null)
                {
                    _termCodeEdit = new()
                    {
                        Id = editRecord.Id,
                        Account = editRecord.Account,
                        TermCodeID = editRecord.TermCode.Id,
                        TotalPtp = editRecord.TotalPtp,
                        DblDip = editRecord.DblDip
                    };
                }
            }
        }

        private void ToggleNew()
        {
            _newTermCode = !_newTermCode;
            _termCodeAdd = new();
        }
    }
}

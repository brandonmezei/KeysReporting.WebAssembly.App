using Microsoft.AspNetCore.Components;
using KeysReporting.WebAssembly.App.Shared.TermCodes;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Client.Services.TermCodes;
using System.Runtime;
using Microsoft.JSInterop;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.Lists;

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
        private NavigationManager NavManager { get; set; }

        [Inject]
        private IJSRuntime JS { get; set; }

        private TermCodeSearchDto _search = new();
        private List<TermCodeReportDto> _termcodeReport = new();
        private TermCodeEditDto _termCodeEdit;
        private List<TermCodeDto> _termCodeList = new();


        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;
        public string? _warnMessage;
        public string? _successMessage;

        public bool _editTermCode;

        protected override async Task OnInitializedAsync()
        {
            if (!await AuthService.CheckLogin())
                NavManager.NavigateTo("/");

            try
            {
                _termCodeList = await TermCodeListService.GetTermCodesAsync();
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }
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
    }
}

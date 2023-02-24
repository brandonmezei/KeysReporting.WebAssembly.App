using Microsoft.AspNetCore.Components;
using KeysReporting.WebAssembly.App.Shared.TermCodes;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Client.Services.TermCodes;
using System.Runtime;
using Microsoft.JSInterop;

namespace KeysReporting.WebAssembly.App.Client.Pages.TermCodes
{
    public partial class Correction
    {
        [Inject]
        private ITermCodeService TermCodeService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        [Inject]
        private IJSRuntime JS { get; set; }

        private SearchDto _search = new();
        private List<TermCodeReportDto> _termcodeReport = new();
        private TermCodeReportDto _termCodeEdit;


        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;
        public string? _warnMessage;
        public string? _successMessage;

        public bool _editTermCode;

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

        }

        private void ToggleEdit(long? id)
        {
            _editTermCode = !_editTermCode;

            if (id.HasValue && _termcodeReport.Any())
                _termCodeEdit = _termcodeReport
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
        }
    }
}

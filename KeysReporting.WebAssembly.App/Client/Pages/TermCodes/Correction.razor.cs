using Microsoft.AspNetCore.Components;
using KeysReporting.WebAssembly.App.Shared.TermCodes;

namespace KeysReporting.WebAssembly.App.Client.Pages.TermCodes
{
    public partial class Correction
    {
        [Inject]
        private NavigationManager NavManager { get; set; }

        private SearchDto _search = new();

        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;
        public string? _warnMessage;
        public string? _successMessage;
    }
}

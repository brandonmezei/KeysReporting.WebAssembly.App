using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Shared.CPH;
using Microsoft.AspNetCore.Components;

namespace KeysReporting.WebAssembly.App.Client.Pages.CPH
{
    public partial class Report
    {
        [Parameter]
        public string SearchDate { get; set; }

        private SearchDto _search = new();
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _search.SearchDate = DateTime.Parse(SearchDate);
            }
            catch
            {
                _errorMessage = Messages.NothingFound;
            }
        }
    }
}

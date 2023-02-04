using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Shared.CPH;
using Microsoft.AspNetCore.Components;

namespace KeysReporting.WebAssembly.App.Client.Pages.CPH
{
    public partial class SearchShared
    {
        [Inject]
        private NavigationManager _NavManager { get; set; }

        [Parameter]
        public DateTime? SearchDate { get; set; }

        private SearchDto _search = new();
        private string? _buttonClass;

        protected override async Task OnInitializedAsync()
        {
            if(SearchDate.HasValue)
                _search.SearchDate = SearchDate.Value;
        }

        private async Task HandleSearch()
        {
            _buttonClass = CSSClasses.ButtonSpin;

            if(_search.SearchDate.HasValue)
                _NavManager.NavigateTo($"/CPH/Report/{ _search.SearchDate.Value.ToString("yyyy-MM-dd") }");
           
            _buttonClass = string.Empty;
        }
    }
}

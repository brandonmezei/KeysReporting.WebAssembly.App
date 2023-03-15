using KeysReporting.WebAssembly.App.Client.Services.ApiError;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Shared.ApiError;
using KeysReporting.WebAssembly.App.Shared.VirtualResponse;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace KeysReporting.WebAssembly.App.Client.Pages.APIErrors
{
    public partial class ApiError
    {
        [Inject]
        private IAuthenticationService AuthService { get; set; }

        [Inject]
        private IApiErrorService ApiErrorService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        private VirtualResponseDto<ApiErrorDto> _response;

        private List<ApiErrorDto> _errorList;

        public int TotalSize { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!await AuthService.CheckLogin())
                NavManager.NavigateTo("/");

            _response = await ApiErrorService.GetApiErrorAsync(new QueryParamDto { StartIndex = 0 });

            if (_response != null)
                _errorList = _response.Items;
                            
        }

        private async Task LoadApiError(QueryParamDto queryParameters)
        {
            var virtualizedResult = await ApiErrorService.GetApiErrorAsync(queryParameters);
            _errorList = virtualizedResult.Items.ToList();
            TotalSize = virtualizedResult.TotalSize;
        }
    }
}

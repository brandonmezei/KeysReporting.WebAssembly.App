using KeysReporting.WebAssembly.App.Shared.ApiError;
using KeysReporting.WebAssembly.App.Shared.VirtualResponse;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace KeysReporting.WebAssembly.App.Client.Pages.APIErrors
{
    public partial class ApiErrorTable
    {
        [Parameter]
        public List<ApiErrorDto> ApiErrors { get; set; }

        [Parameter]
        public int TotalSize { get; set; }

        [Parameter]
        public EventCallback<QueryParamDto> OnScroll { get; set; }

        private async ValueTask<ItemsProviderResult<ApiErrorDto>> LoadApiError(ItemsProviderRequest request)
        {
            var Num = Math.Min(request.Count, TotalSize - request.StartIndex);
            await OnScroll.InvokeAsync(new QueryParamDto
            {
                StartIndex = request.StartIndex,
                PageSize = Num == 0 ? request.Count : Num
            });

            return new ItemsProviderResult<ApiErrorDto>(ApiErrors, TotalSize);
        }
    }
}

using Blazored.LocalStorage;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.Base;
using KeysReporting.WebAssembly.App.Shared.Auth;
using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.File;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace KeysReporting.WebAssembly.App.Client.Services.CPH
{
    public class CPHReportService : BaseHttpService, ICPHReportService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalStorageService _localStorage;

        public CPHReportService(IAuthenticationService authenticationService, ILocalStorageService localStorage, IWebAssemblyHostEnvironment hostEnvironment) : base(localStorage, hostEnvironment)
        {
            _authenticationService = authenticationService;
            _localStorage = localStorage;
        }

        public async Task<List<ProjectListDto>> GetProjectListAsync(DateTime reportDate)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<List<ProjectListDto>>("Api/CPH/GetProjects", HttpMethod.Get, null, $"?ReportDate={reportDate.ToString("yyyy-MM-dd")}");
        }

        public async Task<CPHReportDto> GetCPHReportAsync(SearchDto searchDto)
        {            
            if(!await _authenticationService.CheckLogin())
                return null;
            
            await GetBearerToken();

            return await SendRequest<CPHReportDto>("Api/CPH", HttpMethod.Post, searchDto);

        }

        public async Task<ProjectListDto> CreateProjectAsync(AddProjectDto addProjectDto)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<ProjectListDto>("Api/CPH/CreateProject", HttpMethod.Post, addProjectDto);
        }

        public async Task<CPHReportDto> EditCPHReportTimeAsync(EditTimeDto editTimeDto)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<CPHReportDto>("Api/CPH/EditTimeLine", HttpMethod.Post, editTimeDto);
        }

        public async Task<CPHReportDto> DeleteProjectAsync(DeleteProjectDto deleteProjectDto)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<CPHReportDto>("Api/CPH/DeleteProject", HttpMethod.Post, deleteProjectDto);
        }

        public async Task<CPHReportDto> EditCPH(EditCPHDto editCPHDto)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<CPHReportDto>("Api/CPH/EditCPH", HttpMethod.Post, editCPHDto);
        }

        public async Task<ServiceFileDto> DownloadFile(SearchDto searchDto)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<ServiceFileDto>("Api/CPH/GetFullReport", HttpMethod.Post, searchDto);
        }
    }
}

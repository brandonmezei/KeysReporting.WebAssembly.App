using KeysReporting.WebAssembly.App.Shared.VirtualResponse;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.ApiError;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace KeysReporting.WebAssembly.App.Server.Services.Reports.ApiErrors
{
    public class ApiErrorService : IApiErrorService
    {
        public readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public ApiErrorService(CallDispositionContext callDispositionContext, IMapper mapper)
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        public async Task<VirtualResponseDto<ApiErrorDto>> GetApiErrorsAsync(QueryParamDto queryParm)
        {

            var totalSize = await _callDispositionContext.Apierrors               
                .CountAsync();

            var items = await _callDispositionContext.Apierrors
                .OrderByDescending(x => x.Id)
                .Skip(queryParm.StartIndex)
                .Take(queryParm.PageSize)
                .ProjectTo<ApiErrorDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new VirtualResponseDto<ApiErrorDto> { Items = items, TotalSize = totalSize };
        }
    }
}

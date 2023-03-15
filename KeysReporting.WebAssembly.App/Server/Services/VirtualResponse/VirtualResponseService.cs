using AutoMapper;
using AutoMapper.QueryableExtensions;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.VirtualResponse;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.VirtualResponse
{
    public class VirtualResponseService<T> : IVirtualResponseService<T> where T : class
    {
        public readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public VirtualResponseService(CallDispositionContext callDispositionContext, IMapper mapper)
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
    }
        public async Task<VirtualResponseDto<TResult>> GetAllAsync<TResult>(QueryParamDto queryParm) where TResult : class
        {
            var totalSize = await _callDispositionContext
                .Set<T>()
                .CountAsync();

            var items = await _callDispositionContext
                .Set<T>()
                .Skip(queryParm.StartIndex)
                .Take(queryParm.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new VirtualResponseDto<TResult> { Items = items, TotalSize = totalSize };
        }
    }


}

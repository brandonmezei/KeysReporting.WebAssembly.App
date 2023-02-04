using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.CPH;

namespace KeysReporting.WebAssembly.App.Server.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Cphheader, CPHReportDto>().ReverseMap();
        }
    }
}

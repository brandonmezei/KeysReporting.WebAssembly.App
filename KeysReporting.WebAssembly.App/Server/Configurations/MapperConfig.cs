using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Server.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            CreateMap<ProjectCode, ProjectListDto>();

            //CPH Report
            CreateMap<Cphheader, CPHReportDto>();
            CreateMap<Cphline, CPHReportLineDto>();
        }
    }
}

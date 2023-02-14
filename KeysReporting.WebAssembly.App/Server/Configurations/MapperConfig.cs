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
            CreateMap<SourceTable, SourceTableListDto>();
            CreateMap<Agent, AgentListDto>()
                .ForMember(x => x.AgentName, q => q
                    .MapFrom(mapper => $"{mapper.FirstName} {mapper.LastName}"));

            //CPH Report
            CreateMap<Cphheader, CPHReportDto>();
            CreateMap<Cphline, CPHReportLineDto>();
        }
    }
}

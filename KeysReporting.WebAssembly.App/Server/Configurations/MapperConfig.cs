using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.ApiError;
using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;
using KeysReporting.WebAssembly.App.Shared.TermCodes;

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

            //TermCode
            CreateMap<TermCodeCategory, TermCodeCategoryListDto>();
            CreateMap<TermCode, TermCodeDto>()
                .ForMember(x => x.Category, q => q
                    .MapFrom(mapper => mapper.FkTermCodeCategoryNavigation));

            CreateMap<CallDisposition, TermCodeReportDto>()
                .ForMember(x => x.ProjectList, q => q
                    .MapFrom(mapper => mapper.FkProjectCodeNavigation))
                .ForMember(x => x.LastWriteTime, q => q
                    .MapFrom(mapper => mapper.FkFtpfileNavigation.LastWriteTime))
                .ForMember(x => x.TermCode, q => q
                    .MapFrom(mapper => mapper.FkTermCodeNavigation));

            CreateMap<TermCodeEditDto, CallDisposition>()
                .ForMember(x => x.FkTermCode, q => q
                    .MapFrom(mapper => mapper.TermCodeID));


            CreateMap<TermCodeAddDto, CallDisposition>()
                 .ForMember(x => x.FkAgent, q => q
                    .MapFrom(mapper => mapper.AgentID))
                 .ForMember(x => x.FkTermCode, q => q
                    .MapFrom(mapper => mapper.TermCodeID))
                 .ForMember(x => x.FkProjectCode, q => q
                    .MapFrom(mapper => mapper.ProjectID))
                ;

            //ApiError
            CreateMap<Apierror, ApiErrorDto>();
        }
    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.CPHReport
{
    public class CPHReport : ICPHReport
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public CPHReport(CallDispositionContext callDispositionContext, IMapper mapper) 
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        private async Task CreateHeader(DateTime reportTime)
        {
            reportTime = reportTime.Date;

            //Create if Doesn't Exist
            if (!_callDispositionContext.Cphheaders.Where(x => x.ReportDate == reportTime).Any())
            {
                await _callDispositionContext.Cphheaders.AddAsync(new Cphheader
                {
                    ReportDate = reportTime
                });

                await _callDispositionContext.SaveChangesAsync();
            }
        }

        public async Task<List<ProjectListDto>> GetProjectListAsync(DateTime reportTime)
        {
            var returnDto = new ProjectListDto();

            await CreateHeader(reportTime);

            var dbModel = await _callDispositionContext.ProjectCodes
                .Where(x => x.CphprojectControls.Where(i => i.FkCphheaderNavigation.ReportDate == reportTime).Any())
                .ProjectTo<ProjectListDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
                

            return _mapper.Map<List<ProjectListDto>>(dbModel);
        }

        public async Task<CPHReportDto> GetReportAsync(SearchDto searchDto)
        {

            await CreateHeader(searchDto.SearchDate);

            var dbModelHead = await _callDispositionContext.Cphheaders
                .Where(x => x.ReportDate == searchDto.SearchDate)
                .FirstOrDefaultAsync();
           
            var returnMap = _mapper.Map<CPHReportDto>(dbModelHead);

            var dbModelLines = await _callDispositionContext.Cphlines
                .Include(x => x.FkCphheaderNavigation)
                .Where(x => x.FkCphheaderNavigation.ReportDate == searchDto.SearchDate && x.FkProjectId == searchDto.ProjectID)
                .ToListAsync();

            foreach (var line in dbModelLines)
                returnMap.CPHLines.Add(_mapper.Map<CPHReportLineDto>(line));

            return returnMap;
        }

        public async Task<ProjectListDto> CreateNewProject(AddProjectDto addProjectDto)
        {
            await CreateHeader(addProjectDto.ReportDate);

            var report = await _callDispositionContext.Cphheaders
                .Include(x => x.CphprojectControls)
                    .ThenInclude(x => x.FkProjectCodeNavigation)
                .FirstOrDefaultAsync(x => x.ReportDate == addProjectDto.ReportDate);

            if (report == null)
                return null;

            var project = await _callDispositionContext.ProjectCodes
                .FirstOrDefaultAsync(x => x.ProjectCode1.ToLower() == addProjectDto.ProjectCode.ToLower());

            if (project == null)
            {
                project = new ProjectCode
                {
                    ProjectCode1 = addProjectDto.ProjectCode
                };

                await _callDispositionContext.ProjectCodes.AddAsync(project);
            }

            if(report.CphprojectControls.Where(x => x.FkProjectCodeNavigation.ProjectCode1.ToLower() == addProjectDto.ProjectCode.ToLower()).Any())
                return _mapper.Map<ProjectListDto>(
                        report.CphprojectControls.Select(x => x.FkProjectCodeNavigation)
                        .FirstOrDefault(x => x.ProjectCode1.ToLower() == addProjectDto.ProjectCode.ToLower())
                );

            report.CphprojectControls.Add(new CphprojectControl { 
                FkProjectCodeNavigation = project,
                Cph = 10
            });

            var lineTime = report.ReportDate.AddHours(9);

            while (lineTime < report.ReportDate.AddDays(1).AddMinutes(5))
            {
                report.Cphlines.Add(new Cphline
                {
                    FkProject = project,
                    Series = lineTime,
                    Agent = 0,
                    Contact = 0
                });

                lineTime = lineTime.AddMinutes(5);
            }

            await _callDispositionContext.SaveChangesAsync();

            return _mapper.Map<ProjectListDto>(project);
        }
    }
}

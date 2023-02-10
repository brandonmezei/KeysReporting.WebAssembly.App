using AutoMapper;
using AutoMapper.QueryableExtensions;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.CPHReport
{
    public class CPHReportService : ICPHReportService
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public CPHReportService(CallDispositionContext callDispositionContext, IMapper mapper) 
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        private async Task Recalculate(long reportID, long projectID, long? updateLine = null)
        {
            var dbModelHead = await _callDispositionContext.Cphheaders
                .Include(x => x.CphprojectControls.Where(i => i.FkProjectCode == projectID))
                .Include(x => x.Cphlines.Where(i => i.FkProjectId == projectID))
                .Where(x => x.Id == reportID)
                .FirstOrDefaultAsync();

            var editLine = await _callDispositionContext.Cphlines
               .Where(x => x.Id == updateLine)
               .FirstOrDefaultAsync();

            if (dbModelHead == null || !dbModelHead.CphprojectControls.Any())
                return;

            var cphDiv = (double)dbModelHead.CphprojectControls.FirstOrDefault().Cph / 12;

            var lines = dbModelHead.Cphlines.OrderBy(x => x.Series).ToList();

            if(editLine != null)
                foreach (var line in lines.Where(x => x.Series > editLine.Series))
                        line.Agent = editLine.Agent;

            for (int i = 1; i < lines.Count; i++)
                lines[i].Contact = Math.Round((double)lines[i - 1].Contact + (lines[i - 1].Agent * cphDiv), 5);
            

            await _callDispositionContext.SaveChangesAsync();
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
                .Include(x => x.CphprojectControls.Where(i => i.FkProjectCode == searchDto.ProjectID))
                .Where(x => x.ReportDate == searchDto.SearchDate)
                .FirstOrDefaultAsync();
           
            var returnMap = _mapper.Map<CPHReportDto>(dbModelHead);

            returnMap.CPHLines = new();

            var dbModelLines = await _callDispositionContext.Cphlines
                .Include(x => x.FkProject)
                .Include(x => x.FkCphheaderNavigation)
                .Where(x => x.FkCphheaderNavigation.ReportDate == searchDto.SearchDate && x.FkProjectId == searchDto.ProjectID)
                .OrderBy(x => x.Series)
                .ToListAsync();

            foreach (var line in dbModelLines)
                returnMap.CPHLines.Add(_mapper.Map<CPHReportLineDto>(line));

            returnMap.Project = _mapper.Map<ProjectListDto>(dbModelLines.FirstOrDefault()?.FkProject);
            returnMap.CPH = dbModelHead?.CphprojectControls.FirstOrDefault()?.Cph;
            returnMap.TotalHours = dbModelLines[^1]?.Contact;
            returnMap.Completes = _callDispositionContext.CallDispositions
                .Include(x => x.FkFtpfileNavigation)
                .Where(x => x.FkFtpfileNavigation.LastWriteTime.Date == searchDto.SearchDate
                                && x.FkProjectCode == searchDto.ProjectID)
                .Count();

            return returnMap;
        }

        public async Task<ProjectListDto> CreateNewProjectAsync(AddProjectDto addProjectDto)
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

            if(report.CphprojectControls.Any(x => x.FkProjectCode == project.Id))
                return _mapper.Map<ProjectListDto>(
                        report.CphprojectControls
                        .Where(x => x.FkProjectCode == project.Id)
                        .Select(x => x.FkProjectCodeNavigation)
                        .FirstOrDefault()
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

        public async Task<CPHReportDto> EditTimeLineAsync(EditTimeDto editTimeDto)
        {
            var editLine = await _callDispositionContext.Cphlines
                .Include(x => x.FkCphheaderNavigation)
                .Where(x => x.Id == editTimeDto.Id)
                .FirstOrDefaultAsync();

            if (editLine == null)
                return null;

            editLine.Agent = editTimeDto.Agent;

            await _callDispositionContext.SaveChangesAsync();

            await Recalculate(editLine.FkCphheader, editLine.FkProjectId, editLine.Id);

            return await GetReportAsync(new SearchDto { SearchDate = editLine.FkCphheaderNavigation.ReportDate, ProjectID = editLine.FkProjectId });
        }

        public async Task<CPHReportDto> DeleteProjectAsync(DeleteProjectDto deleteProjectDto)
        {
            var dbModel = await _callDispositionContext.Cphheaders
                .Include(x => x.CphprojectControls.Where(x => x.FkProjectCode == deleteProjectDto.ProjectID))
                .Include(x => x.Cphlines.Where(x => x.FkProjectId == deleteProjectDto.ProjectID))
                .Where(x => x.ReportDate == deleteProjectDto.SearchDate)
                .FirstOrDefaultAsync();

            if (dbModel == null)
                return await GetReportAsync(new SearchDto { SearchDate = deleteProjectDto.SearchDate });

            if(dbModel.CphprojectControls.Any())
                _callDispositionContext.CphprojectControls.Remove(dbModel.CphprojectControls.FirstOrDefault());

            foreach (var line in dbModel.Cphlines)
                _callDispositionContext.Cphlines.Remove(line);

            await _callDispositionContext.SaveChangesAsync();

            return await GetReportAsync(new SearchDto { SearchDate = deleteProjectDto.SearchDate });
        }

        public async Task<CPHReportDto> EditCPH(EditCPHDto editCPHDto)
        {
            var dbModel = await _callDispositionContext.CphprojectControls
                .Include(x => x.FkCphheaderNavigation)
                    .ThenInclude(x => x.Cphlines.Where(x => x.FkProjectId == editCPHDto.ProjectID))
                .Where(x => x.FkCphheaderNavigation.ReportDate == editCPHDto.SearchDate && x.FkProjectCode == editCPHDto.ProjectID)
                .FirstOrDefaultAsync();

            if(dbModel == null)
                return await GetReportAsync(new SearchDto { SearchDate = editCPHDto.SearchDate, ProjectID = editCPHDto.ProjectID });

            if(editCPHDto.CPH.HasValue)
                dbModel.Cph = editCPHDto.CPH.Value;

            await _callDispositionContext.SaveChangesAsync();
            await Recalculate(dbModel.FkCphheaderNavigation.Id, dbModel.FkProjectCode);

            return await GetReportAsync(new SearchDto { SearchDate = editCPHDto.SearchDate, ProjectID = editCPHDto.ProjectID });
        }
    }
}

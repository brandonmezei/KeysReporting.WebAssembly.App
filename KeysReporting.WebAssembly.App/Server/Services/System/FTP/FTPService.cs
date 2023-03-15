using CsvHelper;
using CsvHelper.Configuration;
using KeysReporting.WebAssembly.App.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace KeysReporting.WebAssembly.App.Server.Services.System.FTP
{
    public class FTPService : IFTPService
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IConfiguration _configuration;
        private readonly CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { BadDataFound = null, HeaderValidated = null, MissingFieldFound = null };
        private readonly Chilkat.SFtp sftp;

        public FTPService(CallDispositionContext callDispositionContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _callDispositionContext = callDispositionContext;

#if DEBUG
            sftp = new Chilkat.SFtp();
#else
            sftp = new Chilkat.SFtp()
            {
                HttpProxyHostname = _configuration["FTP:Proxy"],
                HttpProxyPort = int.Parse(_configuration["FTP:ProxyPort"]),
            };
#endif
        }

        public async Task ProcessFileAsync()
        {
            var ftpService = await _callDispositionContext.Ftpservices.FirstOrDefaultAsync();

            if (ftpService == null)
            {
                ftpService = new Ftpservice { LastRun = DateTime.Now };
                _callDispositionContext.Ftpservices.Add(ftpService);
                await _callDispositionContext.SaveChangesAsync();
            }

            if (DateTime.Now.Subtract(ftpService.LastRun).TotalMinutes < 15)
                return;

            ftpService.LastRun = DateTime.Now;
            await _callDispositionContext.SaveChangesAsync();

            //Connect
            if (!sftp.Connect(_configuration["FTP:Link"], 22))
            {
                _callDispositionContext.Apierrors.Add(new Apierror { Apierror1 = sftp.LastErrorText });
                await _callDispositionContext.SaveChangesAsync();

                return;
            }

            //Authenticate
            if (!sftp.AuthenticatePw(_configuration["FTP:User"], _configuration["FTP:Password"]))
            {
                _callDispositionContext.Apierrors.Add(new Apierror { Apierror1 = sftp.LastErrorText });
                await _callDispositionContext.SaveChangesAsync();

                return;
            }

            //Init
            if (!sftp.InitializeSftp())
            {
                _callDispositionContext.Apierrors.Add(new Apierror { Apierror1 = sftp.LastErrorText });
                await _callDispositionContext.SaveChangesAsync();

                return;
            }

            //Get Directory
            var directoryList = sftp.ReadDir(sftp.OpenDir(@"/ftpOut"));

            if (sftp.LastMethodSuccess != true)
            {
                _callDispositionContext.Apierrors.Add(new Apierror { Apierror1 = sftp.LastErrorText });
                await _callDispositionContext.SaveChangesAsync();

                return;
            }

            //Process Per File
            for (int i = 0; i < directoryList.NumFilesAndDirs; i++)
            {
                var file = directoryList.GetFileObject(i);

                if (file.Filename.Contains("KeysAgentDispostions") && DateTime.Parse(file.LastModifiedTimeStr) >= DateTime.Today.AddDays(-7))
                {
                    try
                    {
                        if (_callDispositionContext.Ftpcontrols.Where(x => x.FileName.ToLower() == file.Filename.ToLower()).Any())
                            continue;

                        var fileHandle = sftp.OpenFile(@$"/ftpOut/{file.Filename}", "readOnly", "openExisting");

                        if (sftp.LastMethodSuccess != true)
                        {
                            _callDispositionContext.Apierrors.Add(new Apierror { Apierror1 = sftp.LastErrorText });
                            await _callDispositionContext.SaveChangesAsync();
                            continue;
                        }

                        using MemoryStream ms = new(sftp.ReadFileBytes(fileHandle, file.Size32));
                        ms.Position = 0;

                        using var reader = new StreamReader(ms);
                        using var csv = new CsvReader(reader, csvConfig);

                        var records = csv.GetRecords<Models.Files.FTP>().ToList();

                        var timestring = file.Filename.Split('_')[0].Replace("KeysAgentDispostions", "");
                        var timestamp = DateTime.ParseExact(timestring, "yyyyMMdd", CultureInfo.InvariantCulture);

                        var ftpFile = new Ftpcontrol { FileName = file.Filename, LastWriteTime = timestamp, Result = "OK", RecordCount = records.Count };

                        //New Agents
                        foreach (var agent in records.Where(x => x.Agent_Id.HasValue).GroupBy(x => x.Agent_Id))
                        {
                            var aRecord = await _callDispositionContext.Agents
                                .Where(x => x.AgentId == agent.Key)
                                .FirstOrDefaultAsync();

                            if (aRecord == null)
                            {
                                aRecord = new Agent
                                {
                                    AgentId = agent.Key.Value,
                                    AgentName = agent.First().AgentName,
                                };

                                _callDispositionContext.Agents.Add(aRecord);
                            }

                            aRecord.FirstName = string.IsNullOrEmpty(aRecord.FirstName) ? agent.First().AgentFirst : aRecord.FirstName;
                            aRecord.LastName = string.IsNullOrEmpty(aRecord.LastName) ? agent.First().AgentLast : aRecord.LastName;
                        }

                        //New Client
                        foreach (var client in records.Where(x => !string.IsNullOrEmpty(x.ClientName)).GroupBy(x => x.ClientName))
                            if (!_callDispositionContext.Clients.Where(x => x.ClientName.ToLower() == client.Key.ToLower()).Any())
                                _callDispositionContext.Clients.Add(new Data.Client { ClientName = client.Key });

                        //New Project
                        foreach (var project in records.Where(x => !string.IsNullOrEmpty(x.ProjectName)).GroupBy(x => x.ProjectName))
                            if (!_callDispositionContext.ProjectCodes.Where(x => x.ProjectCode1.ToLower() == project.Key.ToLower()).Any())
                                _callDispositionContext.ProjectCodes.Add(new ProjectCode { ProjectCode1 = project.Key });

                        //New SourceTable
                        foreach (var sourceTable in records.Where(x => !string.IsNullOrEmpty(x.SourceTable)).GroupBy(x => x.SourceTable))
                            if (!_callDispositionContext.SourceTables.Where(x => x.SourceTable1.ToLower() == sourceTable.Key.ToLower()).Any())
                                _callDispositionContext.SourceTables.Add(new SourceTable { SourceTable1 = sourceTable.Key });


                        await _callDispositionContext.SaveChangesAsync();

                        // Insert Records
                        foreach (var record in records.Where(x => x.Agent_Id.HasValue && !string.IsNullOrEmpty(x.TermCode)
                            && !string.IsNullOrEmpty(x.ProjectName) && !string.IsNullOrEmpty(x.ClientName)))
                        {
                            //Exit if term code not found
                            if (!_callDispositionContext.TermCodes.Where(x => x.TermCode1.Contains(record.TermCode)).Any())
                                continue;

                            ftpFile.CallDispositions.Add(new CallDisposition
                            {
                                FkAgentNavigation = await _callDispositionContext.Agents.Where(x => x.AgentId == record.Agent_Id).FirstOrDefaultAsync(),
                                FkTermCodeNavigation = await _callDispositionContext.TermCodes.Where(x => x.TermCode1.Contains(record.TermCode)).FirstOrDefaultAsync(),
                                FkProjectCodeNavigation = await _callDispositionContext.ProjectCodes.Where(x => x.ProjectCode1.ToLower() == record.ProjectName.ToLower()).FirstOrDefaultAsync(),
                                FkClientNavigation = await _callDispositionContext.Clients.Where(x => x.ClientName.ToLower() == record.ClientName.ToLower()).FirstOrDefaultAsync(),
                                FkSourceTableNavigation = await _callDispositionContext.SourceTables.Where(x => x.SourceTable1.ToLower() == record.SourceTable.ToLower()).FirstOrDefaultAsync(),
                                Account = record.Account,
                                TotalPtp = record.TotalPTP,
                                Address = record.Address,
                                LastGiftM = record.LastGiftM,
                                DblDip = record.DblDip,
                                MemberId = record.MemberID,
                                Title1 = record.Title1,
                                Title2 = record.Title2,
                                Gender1 = record.Gender1,
                                Gender2 = record.Gender2,
                                Will = record.WILL,
                                ProcessDate = record.ProcessDate,
                                FirstDate = record.FirstDate,
                                FirstGift = record.FirstGift,
                                LastDate = record.LastDate,
                                LastGift = record.LastGift,
                                HighestDate = record.HighestDate,
                                HighestGift = record.HighestGift,
                                LastDateM = record.LastDateM,
                                TotalCount = record.TotalCount,
                                TotalGift = record.TotalGift,
                                KillNotes = record.KillNotes,
                                TotalTalk = record.TotalTalk,
                                TotalWrap = record.TotalWrap,
                                AttemptsToday = record.AttemptsToday,
                                AttemptsLifeTime = record.AttemptsLifeTime
                            });
                        }

                        _callDispositionContext.Ftpcontrols.Add(ftpFile);
                        await _callDispositionContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _callDispositionContext.Ftpcontrols.Add(new Ftpcontrol { FileName = file.Filename, LastWriteTime = DateTime.Parse(file.LastModifiedTimeStr), Result = ex.Message });
                        await _callDispositionContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}

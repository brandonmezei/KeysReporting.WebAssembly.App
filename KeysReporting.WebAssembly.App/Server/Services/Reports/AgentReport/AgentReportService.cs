using AutoMapper;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Shared.Agent;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Services.Reports.AgentReport
{
    public class AgentReportService : IAgentReportService
    {
        private readonly CallDispositionContext _callDispositionContext;
        private readonly IMapper _mapper;

        public AgentReportService(CallDispositionContext callDispositionContext, IMapper mapper)
        {
            _callDispositionContext = callDispositionContext;
            _mapper = mapper;
        }

        private async Task<AgentReportDto> BindReportAsync(List<CallDisposition> callDispositions)
        {
            var returnReport = new AgentReportDto();

            foreach (var agent in callDispositions.GroupBy(x => x.FkAgentNavigation.AgentId))
            {
                var line = new AgentDetailsDto
                {
                    AgentID = agent.Key.ToString(),
                    AgentName = string.IsNullOrEmpty(agent.FirstOrDefault().FkAgentNavigation.FirstName) && string.IsNullOrEmpty(agent.FirstOrDefault().FkAgentNavigation.LastName)
                        ? agent.FirstOrDefault().FkAgentNavigation.AgentName
                        : $"{agent.FirstOrDefault().FkAgentNavigation.FirstName} {agent.FirstOrDefault().FkAgentNavigation.LastName}",
                    Complete = agent.Count(),
                    Bads = agent.Where(x => string.Compare(x.FkTermCodeNavigation.FkTermCodeCategoryNavigation.Category, "Bad Numbers", true) == 0
                        || string.Compare(x.FkTermCodeNavigation.FkTermCodeCategoryNavigation.Category, "Language", true) == 0
                        || string.Compare(x.FkTermCodeNavigation.Alias, "18 DUP Duplicate Record", true) == 0
                        || string.Compare(x.FkTermCodeNavigation.Alias, "11 HTLE Hostile", true) == 0
                        ).Count(),
                    Callable = agent.Where(x => string.Compare(x.FkTermCodeNavigation.FkTermCodeCategoryNavigation.Category, "Bad Numbers", true) != 0
                        && string.Compare(x.FkTermCodeNavigation.FkTermCodeCategoryNavigation.Category, "Language", true) != 0
                        && string.Compare(x.FkTermCodeNavigation.Alias, "18 DUP Duplicate Record", true) != 0
                        && string.Compare(x.FkTermCodeNavigation.Alias, "11 HTLE Hostile", true) != 0
                        ).Count(),
                    PacCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "33 CCPAC Monthly through Creditcard", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "35 EPAC Monthly through Banking", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "39 EXP New Expiry", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "40 REPRO Reprocess", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "1002 PSMS PAC through SMS", true) == 0
                                            ).Count(),
                    OtgCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "1 CCOTG One Time Gift", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "1003 OSMS OTG through SMS", true) == 0
                                            ).Count(),
                    DblDipCount = agent.Where(x => x.DblDip.HasValue && x.DblDip > 0).Count(),
                    MupCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "333 MUP Monthly Upgrade", true) == 0).Count(),
                    CancelCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "339 CANCM Cancel Monthly Donation", true) == 0).Count(),
                    RefusalCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.FkTermCodeCategoryNavigation.Category, "Refusal", true) == 0).Count(),
                    DncCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.FkTermCodeCategoryNavigation.Category, "DNC", true) == 0).Count(),
                    NrnCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "8 NRN Not Right Now", true) == 0).Count(),
                    WillSendCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "74 WSWO Will Send with out Amount", true) == 0
                                                || string.Compare(x.FkTermCodeNavigation.Alias, "73 WSWA Will Send with Amount", true) == 0
                                                || string.Compare(x.FkTermCodeNavigation.Alias, "79 ONLIN Will Give Online", true) == 0
                                                || string.Compare(x.FkTermCodeNavigation.Alias, "78 EMAIL Email Pledge", true) == 0).Count(),
                    MailCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "4 MAIL Mail Pledge with Amount", true) == 0
                                                || string.Compare(x.FkTermCodeNavigation.Alias, "43 MAILP Mail Monthly Pledge with Amount", true) == 0
                                                || string.Compare(x.FkTermCodeNavigation.Alias, "7 PLEDGE Mail Out Pledge", true) == 0).Count(),
                    QuestCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "2 QUEST Answered Questions", true) == 0).Count(),
                    MaybeSendCount = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "7 PLEDGE Mail Out Pledge", true) == 0).Count(),
                    BillMe = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "4 MAIL Mail Pledge with Amount", true) == 0
                                                || string.Compare(x.FkTermCodeNavigation.Alias, "43 MAILP Mail Monthly Pledge with Amount", true) == 0).Count()
                };

                //PTP Sums
                line.PacSum = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "33 CCPAC Monthly through Creditcard", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "35 EPAC Monthly through Banking", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "1002 PSMS PAC through SMS", true) == 0).Sum(x => x.TotalPtp);
                line.OTGSum = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "1 CCOTG One Time Gift", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "1003 OSMS OTG through SMS", true) == 0).Sum(x => x.TotalPtp);
                line.MUPSum = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "333 MUP Monthly Upgrade", true) == 0).Sum(x => x.TotalPtp);
                line.DblDipSum = agent.Sum(x => x.DblDip);
                line.OtgPlusDBL = line.OTGSum + line.DblDipSum;
                line.PacYear = line.PacSum.HasValue ? line.PacSum * 12 : 0;
                line.LGPacSum = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "33 CCPAC Monthly through Creditcard", true) == 0
                                            || string.Compare(x.FkTermCodeNavigation.Alias, "35 EPAC Monthly through Banking", true) == 0).Sum(x => x.LastGift);
                line.PacDiff = line.PacYear > 0 && line.LGPacSum > 0 ? Math.Round(((decimal)line.PacYear / (decimal)line.LGPacSum) - 1, 2) : 0;
                line.LGOTGSum = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "1 CCOTG One Time Gift", true) == 0).Sum(x => x.LastGift);
                line.OTGDiff = line.OTGSum > 0 && line.LGOTGSum > 0 ? Math.Round(((decimal)line.OTGSum / (decimal)line.LGOTGSum) - 1, 2) : 0;
                line.BillMeSum = agent.Where(x => string.Compare(x.FkTermCodeNavigation.Alias, "4 MAIL Mail Pledge with Amount", true) == 0
                                                || string.Compare(x.FkTermCodeNavigation.Alias, "43 MAILP Mail Monthly Pledge with Amount", true) == 0).Sum(x => x.TotalPtp);


                //Averages
                line.PacAverage = line.PacSum.HasValue && line.PacCount.HasValue && line.PacCount > 0 ? Math.Round((decimal)line.PacSum.Value / (decimal)line.PacCount.Value, 2) : 0;
                line.OTGAverage = line.OTGSum.HasValue && line.OtgCount.HasValue && line.OtgCount > 0 ? Math.Round((decimal)line.OTGSum.Value / (decimal)line.OtgCount.Value, 2) : 0;
                line.MUPAverage = line.MUPSum.HasValue && line.MupCount.HasValue && line.MupCount > 0 ? Math.Round((decimal)line.MUPSum.Value / (decimal)line.MupCount.Value, 2) : 0;
                line.DblDipAverage = line.DblDipSum.HasValue && line.DblDipCount.HasValue && line.DblDipCount > 0 ? Math.Round((decimal)line.DblDipSum.Value / (decimal)line.DblDipCount.Value, 2) : 0;

                //Rates
                line.Response = line.Complete > 0 ? Math.Round(((decimal)line.Callable / (decimal)line.Complete) * 100, 2) : 0;
                line.PacRate = line.Callable > 0 ? Math.Round(((decimal)line.PacCount / (decimal)line.Callable) * 100, 2) : 0;
                line.OtgRate = line.Callable > 0 ? Math.Round((((decimal)line.OtgCount + (decimal)line.DblDipCount) / (decimal)line.Callable) * 100, 2) : 0;
                line.MupRate = line.Callable > 0 ? Math.Round(((decimal)line.MupCount / (decimal)line.Callable) * 100, 2) : 0;
                line.CancelRate = line.Callable > 0 ? Math.Round(((decimal)line.CancelCount / (decimal)line.Callable) * 100, 2) : 0;
                line.DNCRate = line.Callable > 0 ? Math.Round(((decimal)line.DncCount / (decimal)line.Callable) * 100, 2) : 0;
                line.QuestRate = line.Callable > 0 ? Math.Round(((decimal)line.QuestCount / (decimal)line.Callable) * 100, 2) : 0;

                //Time Counts
                line.TotalTalk = agent.Sum(x => x.TotalTalk) / 60;
                line.TotalTalkAvg = line.Callable > 0 ? Math.Round(((decimal)line.TotalTalk / (decimal)line.Callable), 2) : 0;
                line.TotalWrap = agent.Sum(x => x.TotalWrap) / 60;
                line.TotalWrapAvg = line.Callable > 0 ? Math.Round(((decimal)line.TotalWrap / (decimal)line.Callable), 2) : 0;

                //Dollars Raised
                line.DollarsRaised = (decimal)12 * (line.PacSum + line.MUPSum) + line.OTGSum + line.DblDipSum;

                returnReport.AgentLines.Add(line);
            }

            returnReport.Summary.Complete = returnReport.AgentLines.Sum(x => x.Complete);
            returnReport.Summary.Bads = returnReport.AgentLines.Sum(x => x.Bads);
            returnReport.Summary.Callable = returnReport.AgentLines.Sum(x => x.Callable);
            returnReport.Summary.PacCount = returnReport.AgentLines.Sum(x => x.PacCount);
            returnReport.Summary.OtgCount = returnReport.AgentLines.Sum(x => x.OtgCount);
            returnReport.Summary.DblDipCount = returnReport.AgentLines.Sum(x => x.DblDipCount);
            returnReport.Summary.MupCount = returnReport.AgentLines.Sum(x => x.MupCount);
            returnReport.Summary.CancelCount = returnReport.AgentLines.Sum(x => x.CancelCount);
            returnReport.Summary.RefusalCount = returnReport.AgentLines.Sum(x => x.RefusalCount);
            returnReport.Summary.RefusalCount = returnReport.AgentLines.Sum(x => x.RefusalCount);
            returnReport.Summary.DncCount = returnReport.AgentLines.Sum(x => x.DncCount);
            returnReport.Summary.NrnCount = returnReport.AgentLines.Sum(x => x.NrnCount);
            returnReport.Summary.WillSendCount = returnReport.AgentLines.Sum(x => x.WillSendCount);
            returnReport.Summary.MaybeSendCount = returnReport.AgentLines.Sum(x => x.MaybeSendCount);
            returnReport.Summary.BillMe = returnReport.AgentLines.Sum(x => x.BillMe);
            returnReport.Summary.BillMeSum = returnReport.AgentLines.Sum(x => x.BillMeSum);
            returnReport.Summary.MailCount = returnReport.AgentLines.Sum(x => x.MailCount);
            returnReport.Summary.QuestCount = returnReport.AgentLines.Sum(x => x.QuestCount);
            returnReport.Summary.PacSum = returnReport.AgentLines.Sum(x => x.PacSum);
            returnReport.Summary.OTGSum = returnReport.AgentLines.Sum(x => x.OTGSum);
            returnReport.Summary.MUPSum = returnReport.AgentLines.Sum(x => x.MUPSum);
            returnReport.Summary.DblDipSum = returnReport.AgentLines.Sum(x => x.DblDipSum);
            returnReport.Summary.OtgPlusDBL = returnReport.AgentLines.Sum(x => x.OtgPlusDBL);
            returnReport.Summary.PacYear = returnReport.AgentLines.Sum(x => x.PacYear);
            returnReport.Summary.LGPacSum = returnReport.AgentLines.Sum(x => x.LGPacSum);
            returnReport.Summary.PacDiff = returnReport.AgentLines.Sum(x => x.PacDiff);
            returnReport.Summary.LGOTGSum = returnReport.AgentLines.Sum(x => x.LGOTGSum);
            returnReport.Summary.OTGDiff = returnReport.AgentLines.Sum(x => x.OTGDiff);

            returnReport.Summary.PacAverage = returnReport.Summary.PacSum.HasValue && returnReport.Summary.PacCount.HasValue && returnReport.Summary.PacCount > 0 ? Math.Round((decimal)returnReport.Summary.PacSum.Value / (decimal)returnReport.Summary.PacCount.Value, 2) : 0;
            returnReport.Summary.OTGAverage = returnReport.Summary.OTGSum.HasValue && returnReport.Summary.OtgCount.HasValue && returnReport.Summary.OtgCount > 0 ? Math.Round((decimal)returnReport.Summary.OTGSum.Value / (decimal)returnReport.Summary.OtgCount.Value, 2) : 0;
            returnReport.Summary.MUPAverage = returnReport.Summary.MUPSum.HasValue && returnReport.Summary.MupCount.HasValue && returnReport.Summary.MupCount > 0 ? Math.Round((decimal)returnReport.Summary.MUPSum.Value / (decimal)returnReport.Summary.MupCount.Value, 2) : 0;
            returnReport.Summary.DblDipAverage = returnReport.Summary.DblDipSum.HasValue && returnReport.Summary.DblDipCount.HasValue && returnReport.Summary.DblDipCount > 0 ? Math.Round((decimal)returnReport.Summary.DblDipSum.Value / (decimal)returnReport.Summary.DblDipCount.Value, 2) : 0;

            returnReport.Summary.Response = returnReport.Summary.Complete > 0 ? Math.Round(((decimal)returnReport.Summary.Callable / (decimal)returnReport.Summary.Complete) * 100, 2) : 0;
            returnReport.Summary.PacRate = returnReport.Summary.Callable > 0 ? Math.Round(((decimal)returnReport.Summary.PacCount / (decimal)returnReport.Summary.Callable) * 100, 2) : 0;
            returnReport.Summary.OtgRate = returnReport.Summary.Callable > 0 ? Math.Round(((decimal)returnReport.Summary.OtgCount / (decimal)returnReport.Summary.Callable) * 100, 2) : 0;
            returnReport.Summary.MupRate = returnReport.Summary.Callable > 0 ? Math.Round(((decimal)returnReport.Summary.MupCount / (decimal)returnReport.Summary.Callable) * 100, 2) : 0;
            returnReport.Summary.CancelRate = returnReport.Summary.Callable > 0 ? Math.Round(((decimal)returnReport.Summary.CancelCount / (decimal)returnReport.Summary.Callable) * 100, 2) : 0;
            returnReport.Summary.DNCRate = returnReport.Summary.Callable > 0 ? Math.Round(((decimal)returnReport.Summary.DncCount / (decimal)returnReport.Summary.Callable) * 100, 2) : 0;
            returnReport.Summary.QuestRate = returnReport.Summary.Callable > 0 ? Math.Round(((decimal)returnReport.Summary.QuestCount / (decimal)returnReport.Summary.Callable) * 100, 2) : 0;

            returnReport.Summary.TotalTalk = returnReport.AgentLines.Sum(x => x.TotalTalk);
            returnReport.Summary.TotalTalkAvg = returnReport.Summary.Callable > 0 ? Math.Round(((decimal)returnReport.Summary.TotalTalk / (decimal)returnReport.Summary.Callable), 2) : 0;
            returnReport.Summary.TotalWrap = returnReport.AgentLines.Sum(x => x.TotalWrap);
            returnReport.Summary.TotalWrapAvg = returnReport.Summary.Callable > 0 ? Math.Round(((decimal)returnReport.Summary.TotalWrap / (decimal)returnReport.Summary.Callable), 2) : 0;

            returnReport.Summary.DollarsRaised = (decimal)12 * (returnReport.Summary.PacSum + returnReport.Summary.MUPSum) + returnReport.Summary.OTGSum + returnReport.Summary.DblDipSum;

            return returnReport;
        }

        public async Task<AgentReportDto> GetReportAsync(SearchDto searchDto)
        {
            //Prevent Entire DB selects
            if (string.IsNullOrEmpty(searchDto.Client) && !searchDto.SourceTable.HasValue && !searchDto.Project.HasValue
                && !searchDto.Agent.HasValue && !searchDto.StartDate.HasValue && !searchDto.EndDate.HasValue)
                return new AgentReportDto();

            //Filter Out Bad Codes
            var query = _callDispositionContext.CallDispositions
                .Include(x => x.FkAgentNavigation)
                .Include(x => x.FkClientNavigation)
                .Include(x => x.FkFtpfileNavigation)
                .Include(x => x.FkProjectCodeNavigation)
                .Include(x => x.FkTermCodeNavigation.FkTermCodeCategoryNavigation)
                .Where(x => x.FkTermCodeNavigation.Alias != "24 BUSY Busy Signal"
                    && x.FkTermCodeNavigation.Alias != "55 ANSM Answering Machine Hangup"
                    && x.FkTermCodeNavigation.Alias != "66 GENCB Dead Air"
                    && x.FkTermCodeNavigation.Alias != "66 GENCB No Answer"
                    && x.FkTermCodeNavigation.Alias != "67 PUBCB Global Callback"
                    && x.FkTermCodeNavigation.Alias != "99 PRICB Personal Callback"
                 );

            query = searchDto.StartDate.HasValue
                ? query.Where(x => x.FkFtpfileNavigation.LastWriteTime >= searchDto.StartDate)
                : query;

            query = searchDto.EndDate.HasValue
                ? query.Where(x => x.FkFtpfileNavigation.LastWriteTime < searchDto.EndDate.Value.AddDays(1))
                : query;

            query = searchDto.Agent.HasValue
                ? query.Where(x => x.FkAgent == searchDto.Agent)
                : query;

            query = searchDto.SourceTable.HasValue
               ? query.Where(x => x.FkSourceTable == searchDto.SourceTable)
               : query;

            query = string.IsNullOrEmpty(searchDto.Client)
               ? query
               : query.Where(x => x.FkSourceTableNavigation.SourceTable1.Contains(searchDto.Client));

            query = searchDto.Project.HasValue
                    ? query.Where(x => x.FkProjectCode == searchDto.Project)
                    : query;

            return await BindReportAsync(await query.ToListAsync());
        }
    }
}

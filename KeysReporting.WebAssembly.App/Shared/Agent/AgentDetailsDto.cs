using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.Agent
{
    public class AgentDetailsDto
    {
        public string AgentID { get; set; }
        public string AgentName { get; set; }
        public decimal? DollarsRaised { get; set; }
        public int? Complete { get; set; }
        public int? Bads { get; set; }
        public int? Callable { get; set; }
        public decimal? Response { get; set; }
        public int? PacCount { get; set; }
        public decimal? PacSum { get; set; }
        public decimal? PacAverage { get; set; }
        public decimal? PacRate { get; set; }
        public decimal? PacYear { get; set; }
        public decimal? LGPacSum { get; set; }
        public decimal? PacDiff { get; set; }
        public int? OtgCount { get; set; }
        public decimal? OTGSum { get; set; }
        public decimal? OTGAverage { get; set; }
        public decimal? OtgRate { get; set; }
        public decimal? LGOTGSum { get; set; }
        public decimal? OTGDiff { get; set; }
        public int? DblDipCount { get; set; }
        public decimal? DblDipSum { get; set; }
        public decimal? DblDipAverage { get; set; }
        public decimal? OtgPlusDBL { get; set; }
        public int? MupCount { get; set; }
        public decimal? MUPSum { get; set; }
        public decimal? MUPAverage { get; set; }
        public decimal? MupRate { get; set; }
        public int? CancelCount { get; set; }
        public decimal? CancelRate { get; set; }
        public int? RefusalCount { get; set; }
        public int? DncCount { get; set; }
        public decimal? DNCRate { get; set; }
        public int? NrnCount { get; set; }
        public int? WillSendCount { get; set; }
        public int? MaybeSendCount { get; set; }
        public int? MailCount { get; set; }
        public int? QuestCount { get; set; }
        public int? BillMe { get; set; }
        public decimal? BillMeSum { get; set; }
        public decimal? QuestRate { get; set; }
        public decimal? TotalTalk { get; set; }
        public decimal? TotalWrap { get; set; }
        public decimal? TotalTalkAvg { get; set; }
        public decimal? TotalWrapAvg { get; set; }
    }
}

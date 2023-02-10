using KeysReporting.WebAssembly.App.Shared.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.CPH
{
    public class CPHReportDto
    {
        public DateTime ReportDate { get; set; }

        public ProjectListDto Project { get; set; } = new();

        public int? CPH { get; set; }

        public double? TotalHours { get; set; } 
        public int? Completes { get; set; }

        public List<CPHReportLineDto> CPHLines { get; set; } = new();
    }

}

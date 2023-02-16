using KeysReporting.WebAssembly.App.Shared.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.TermCodes
{
    public class TermCodeReportDto
    {
        public string? Account { get; set; }
        public ProjectListDto ProjectList { get; set; }
        public DateTime? LastWriteTime { get; set; }
        public decimal? TotalPtp { get; set; }
        public decimal? DblDip { get; set; }
    }

}

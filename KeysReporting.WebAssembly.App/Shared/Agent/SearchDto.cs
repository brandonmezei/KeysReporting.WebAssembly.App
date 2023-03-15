using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.Agent
{
    public class SearchDto
    {
        public string? Client { get; set; }
        public long? SourceTable { get; set; }
        public List<long>? Project { get; set; }
        public long? Agent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

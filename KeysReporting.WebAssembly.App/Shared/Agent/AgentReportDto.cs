using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.Agent
{
    public class AgentReportDto
    {
        public List<AgentDetailsDto> AgentLines { get; set; } = new();

        public AgentDetailsDto Summary { get; set; } = new();
    }
}

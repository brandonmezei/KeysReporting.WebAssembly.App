using KeysReporting.WebAssembly.App.Shared.Lists;

namespace KeysReporting.WebAssembly.App.Shared.CPH
{
    public class CPHReportLineDto
    {
        public long Id { get; set; }

        public DateTime Series { get; set; }

        public int Agent { get; set; }

        public double Contact { get; set; }
    }

}

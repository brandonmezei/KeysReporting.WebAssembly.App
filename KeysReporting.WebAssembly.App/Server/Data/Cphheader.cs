using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class Cphheader
{
    public long Id { get; set; }

    public DateTime ReportDate { get; set; }

    public virtual ICollection<Cphline> Cphlines { get; } = new List<Cphline>();

    public virtual ICollection<CphprojectControl> CphprojectControls { get; } = new List<CphprojectControl>();
}

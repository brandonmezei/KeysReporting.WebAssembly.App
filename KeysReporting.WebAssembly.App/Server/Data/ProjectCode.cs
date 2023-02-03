using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class ProjectCode
{
    public long Id { get; set; }

    public string? ProjectCode1 { get; set; }

    public virtual ICollection<CallDisposition> CallDispositions { get; } = new List<CallDisposition>();

    public virtual ICollection<Cphline> Cphlines { get; } = new List<Cphline>();

    public virtual ICollection<CphprojectControl> CphprojectControls { get; } = new List<CphprojectControl>();
}

using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class Cphline
{
    public long Id { get; set; }

    public long FkCphheader { get; set; }

    public long FkProjectId { get; set; }

    public DateTime Series { get; set; }

    public int Agent { get; set; }

    public double Contact { get; set; }

    public virtual Cphheader FkCphheaderNavigation { get; set; } = null!;

    public virtual ProjectCode FkProject { get; set; } = null!;
}

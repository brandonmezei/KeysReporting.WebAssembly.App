using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class CphprojectControl
{
    public long Id { get; set; }

    public long FkCphheader { get; set; }

    public long FkProjectCode { get; set; }

    public int Cph { get; set; }

    public virtual Cphheader FkCphheaderNavigation { get; set; } = null!;

    public virtual ProjectCode FkProjectCodeNavigation { get; set; } = null!;
}

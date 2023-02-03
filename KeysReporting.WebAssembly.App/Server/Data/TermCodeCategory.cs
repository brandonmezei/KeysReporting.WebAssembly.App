using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class TermCodeCategory
{
    public long Id { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<TermCode> TermCodes { get; } = new List<TermCode>();
}

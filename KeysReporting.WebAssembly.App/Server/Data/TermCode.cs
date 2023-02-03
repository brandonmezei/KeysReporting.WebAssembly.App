using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class TermCode
{
    public long Id { get; set; }

    public long FkTermCodeCategory { get; set; }

    public string? TermCode1 { get; set; }

    public string? Alias { get; set; }

    public virtual ICollection<CallDisposition> CallDispositions { get; } = new List<CallDisposition>();

    public virtual TermCodeCategory FkTermCodeCategoryNavigation { get; set; } = null!;
}

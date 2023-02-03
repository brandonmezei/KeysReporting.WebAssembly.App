using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class SourceTable
{
    public long Id { get; set; }

    public string SourceTable1 { get; set; } = null!;

    public virtual ICollection<CallDisposition> CallDispositions { get; } = new List<CallDisposition>();
}

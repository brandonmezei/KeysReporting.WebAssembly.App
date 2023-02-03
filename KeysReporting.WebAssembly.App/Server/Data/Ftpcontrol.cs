using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class Ftpcontrol
{
    public long Id { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime LastWriteTime { get; set; }

    public string Result { get; set; } = null!;

    public int? RecordCount { get; set; }

    public virtual ICollection<CallDisposition> CallDispositions { get; } = new List<CallDisposition>();
}

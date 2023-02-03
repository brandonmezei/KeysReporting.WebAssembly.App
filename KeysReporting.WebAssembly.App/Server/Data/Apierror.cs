using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class Apierror
{
    public long Id { get; set; }

    public string Apierror1 { get; set; } = null!;

    public DateTime SystemTime { get; set; }
}

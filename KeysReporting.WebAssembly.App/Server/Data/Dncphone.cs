using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class Dncphone
{
    public long Id { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime CreateDate { get; set; }
}

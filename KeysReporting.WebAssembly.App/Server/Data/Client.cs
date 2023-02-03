using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class Client
{
    public long Id { get; set; }

    public string ClientName { get; set; } = null!;

    public virtual ICollection<CallDisposition> CallDispositions { get; } = new List<CallDisposition>();
}

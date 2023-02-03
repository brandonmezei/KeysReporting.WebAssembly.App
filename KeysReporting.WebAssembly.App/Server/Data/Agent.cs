using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class Agent
{
    public long Id { get; set; }

    public int AgentId { get; set; }

    public string AgentName { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<CallDisposition> CallDispositions { get; } = new List<CallDisposition>();
}

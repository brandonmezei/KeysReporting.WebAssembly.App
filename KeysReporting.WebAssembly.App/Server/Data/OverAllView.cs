using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class OverAllView
{
    public DateTime LastRun { get; set; }

    public string Category { get; set; } = null!;

    public string? TermCode { get; set; }

    public string ClientName { get; set; } = null!;

    public string? ProjectCode { get; set; }

    public int AgentId { get; set; }

    public string AgentName { get; set; } = null!;

    public string? Account { get; set; }

    public decimal? TotalPtp { get; set; }

    public string? Address { get; set; }

    public decimal? LastGiftM { get; set; }

    public decimal? DblDip { get; set; }

    public string? MemberId { get; set; }

    public string? Title1 { get; set; }

    public string? Title2 { get; set; }

    public string? Gender1 { get; set; }

    public string? Gender2 { get; set; }

    public string? Will { get; set; }

    public string? CreditCard { get; set; }

    public int? Ccmonth { get; set; }

    public int? Ccyear { get; set; }

    public int? Cvv { get; set; }

    public string? NameOnCard { get; set; }

    public DateTime? ProcessDate { get; set; }

    public DateTime? FirstDate { get; set; }

    public decimal? FirstGift { get; set; }

    public DateTime? LastDate { get; set; }

    public decimal? LastGift { get; set; }

    public DateTime? HighestDate { get; set; }

    public decimal? HighestGift { get; set; }

    public DateTime? LastDateM { get; set; }

    public int? TotalCount { get; set; }

    public decimal? TotalGift { get; set; }

    public string? KillNotes { get; set; }

    public int? TotalTalk { get; set; }

    public int? TotalWrap { get; set; }

    public int? AttemptsToday { get; set; }

    public int? AttemptsLifeTime { get; set; }
}

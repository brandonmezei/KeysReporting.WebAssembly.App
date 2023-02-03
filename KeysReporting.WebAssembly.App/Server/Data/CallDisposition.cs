using System;
using System.Collections.Generic;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class CallDisposition
{
    public long Id { get; set; }

    public long FkFtpfile { get; set; }

    public long FkAgent { get; set; }

    public long FkTermCode { get; set; }

    public long FkProjectCode { get; set; }

    public long FkClient { get; set; }

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

    public DateTime? UpdateDate { get; set; }

    public int? UpdateId { get; set; }

    public long? FkSourceTable { get; set; }

    public virtual Agent FkAgentNavigation { get; set; } = null!;

    public virtual Client FkClientNavigation { get; set; } = null!;

    public virtual Ftpcontrol FkFtpfileNavigation { get; set; } = null!;

    public virtual ProjectCode FkProjectCodeNavigation { get; set; } = null!;

    public virtual SourceTable? FkSourceTableNavigation { get; set; }

    public virtual TermCode FkTermCodeNavigation { get; set; } = null!;
}

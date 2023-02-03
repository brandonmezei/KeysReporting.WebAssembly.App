using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KeysReporting.WebAssembly.App.Server.Data;

public partial class CallDispositionContext : DbContext
{
    public CallDispositionContext()
    {
    }

    public CallDispositionContext(DbContextOptions<CallDispositionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<Apierror> Apierrors { get; set; }

    public virtual DbSet<CallDisposition> CallDispositions { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Cphheader> Cphheaders { get; set; }

    public virtual DbSet<Cphline> Cphlines { get; set; }

    public virtual DbSet<CphprojectControl> CphprojectControls { get; set; }

    public virtual DbSet<Dncphone> Dncphones { get; set; }

    public virtual DbSet<Ftpcontrol> Ftpcontrols { get; set; }

    public virtual DbSet<Ftpservice> Ftpservices { get; set; }

    public virtual DbSet<OverAllView> OverAllViews { get; set; }

    public virtual DbSet<ProjectCode> ProjectCodes { get; set; }

    public virtual DbSet<SourceTable> SourceTables { get; set; }

    public virtual DbSet<TermCode> TermCodes { get; set; }

    public virtual DbSet<TermCodeCategory> TermCodeCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DBTest");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.ToTable("Agent");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AgentId).HasColumnName("Agent_Id");
            entity.Property(e => e.AgentName).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
        });

        modelBuilder.Entity<Apierror>(entity =>
        {
            entity.ToTable("APIError");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apierror1).HasColumnName("APIError");
            entity.Property(e => e.SystemTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CallDisposition>(entity =>
        {
            entity.ToTable("CallDisposition");

            entity.HasIndex(e => e.Account, "Account_INDX");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Account).HasMaxLength(255);
            entity.Property(e => e.Ccmonth).HasColumnName("CCMonth");
            entity.Property(e => e.Ccyear).HasColumnName("CCYear");
            entity.Property(e => e.CreditCard).HasMaxLength(50);
            entity.Property(e => e.Cvv).HasColumnName("CVV");
            entity.Property(e => e.DblDip).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FirstDate).HasColumnType("date");
            entity.Property(e => e.FirstGift).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FkAgent).HasColumnName("FK_Agent");
            entity.Property(e => e.FkClient).HasColumnName("FK_Client");
            entity.Property(e => e.FkFtpfile).HasColumnName("FK_FTPFile");
            entity.Property(e => e.FkProjectCode).HasColumnName("FK_ProjectCode");
            entity.Property(e => e.FkSourceTable).HasColumnName("FK_SourceTable");
            entity.Property(e => e.FkTermCode).HasColumnName("FK_TermCode");
            entity.Property(e => e.Gender1).HasMaxLength(50);
            entity.Property(e => e.Gender2).HasMaxLength(50);
            entity.Property(e => e.HighestDate).HasColumnType("date");
            entity.Property(e => e.HighestGift).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastDate).HasColumnType("date");
            entity.Property(e => e.LastDateM).HasColumnType("date");
            entity.Property(e => e.LastGift).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastGiftM).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MemberId)
                .HasMaxLength(255)
                .HasColumnName("MemberID");
            entity.Property(e => e.NameOnCard).HasMaxLength(255);
            entity.Property(e => e.ProcessDate).HasColumnType("date");
            entity.Property(e => e.Title1).HasMaxLength(50);
            entity.Property(e => e.Title2).HasMaxLength(50);
            entity.Property(e => e.TotalGift).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPtp)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalPTP");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            entity.Property(e => e.Will)
                .HasMaxLength(50)
                .HasColumnName("WILL");

            entity.HasOne(d => d.FkAgentNavigation).WithMany(p => p.CallDispositions)
                .HasForeignKey(d => d.FkAgent)
                .HasConstraintName("FK_CallDisposition_Agent");

            entity.HasOne(d => d.FkClientNavigation).WithMany(p => p.CallDispositions)
                .HasForeignKey(d => d.FkClient)
                .HasConstraintName("FK_CallDisposition_Client");

            entity.HasOne(d => d.FkFtpfileNavigation).WithMany(p => p.CallDispositions)
                .HasForeignKey(d => d.FkFtpfile)
                .HasConstraintName("FK_CallDisposition_FTPControl");

            entity.HasOne(d => d.FkProjectCodeNavigation).WithMany(p => p.CallDispositions)
                .HasForeignKey(d => d.FkProjectCode)
                .HasConstraintName("FK_CallDisposition_ProjectCode");

            entity.HasOne(d => d.FkSourceTableNavigation).WithMany(p => p.CallDispositions)
                .HasForeignKey(d => d.FkSourceTable)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CallDisposition_SourceTable");

            entity.HasOne(d => d.FkTermCodeNavigation).WithMany(p => p.CallDispositions)
                .HasForeignKey(d => d.FkTermCode)
                .HasConstraintName("FK_CallDisposition_TermCode");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClientName).HasMaxLength(255);
        });

        modelBuilder.Entity<Cphheader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CPH_Header");

            entity.ToTable("CPHHeader");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ReportDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Cphline>(entity =>
        {
            entity.ToTable("CPHLine");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FkCphheader).HasColumnName("FK_CPHHeader");
            entity.Property(e => e.FkProjectId).HasColumnName("FK_ProjectID");
            entity.Property(e => e.Series).HasColumnType("datetime");

            entity.HasOne(d => d.FkCphheaderNavigation).WithMany(p => p.Cphlines)
                .HasForeignKey(d => d.FkCphheader)
                .HasConstraintName("FK_CPHLine_CPHHeader");

            entity.HasOne(d => d.FkProject).WithMany(p => p.Cphlines)
                .HasForeignKey(d => d.FkProjectId)
                .HasConstraintName("FK_CPHLine_ProjectCode");
        });

        modelBuilder.Entity<CphprojectControl>(entity =>
        {
            entity.ToTable("CPHProjectControl");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cph).HasColumnName("CPH");
            entity.Property(e => e.FkCphheader).HasColumnName("FK_CPHHeader");
            entity.Property(e => e.FkProjectCode).HasColumnName("FK_ProjectCode");

            entity.HasOne(d => d.FkCphheaderNavigation).WithMany(p => p.CphprojectControls)
                .HasForeignKey(d => d.FkCphheader)
                .HasConstraintName("FK_CPHProjectControl_CPHHeader");

            entity.HasOne(d => d.FkProjectCodeNavigation).WithMany(p => p.CphprojectControls)
                .HasForeignKey(d => d.FkProjectCode)
                .HasConstraintName("FK_CPHProjectControl_ProjectCode");
        });

        modelBuilder.Entity<Dncphone>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DNCPhone");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Phone).HasMaxLength(255);
        });

        modelBuilder.Entity<Ftpcontrol>(entity =>
        {
            entity.ToTable("FTPControl");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastWriteTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Ftpservice>(entity =>
        {
            entity.ToTable("FTPService");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastRun)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<OverAllView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("OverAllView");

            entity.Property(e => e.Account).HasMaxLength(255);
            entity.Property(e => e.AgentId).HasColumnName("Agent_Id");
            entity.Property(e => e.AgentName).HasMaxLength(255);
            entity.Property(e => e.Category).HasMaxLength(255);
            entity.Property(e => e.Ccmonth).HasColumnName("CCMonth");
            entity.Property(e => e.Ccyear).HasColumnName("CCYear");
            entity.Property(e => e.ClientName).HasMaxLength(255);
            entity.Property(e => e.CreditCard).HasMaxLength(50);
            entity.Property(e => e.Cvv).HasColumnName("CVV");
            entity.Property(e => e.DblDip).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FirstDate).HasColumnType("date");
            entity.Property(e => e.FirstGift).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Gender1).HasMaxLength(50);
            entity.Property(e => e.Gender2).HasMaxLength(50);
            entity.Property(e => e.HighestDate).HasColumnType("date");
            entity.Property(e => e.HighestGift).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastDate).HasColumnType("date");
            entity.Property(e => e.LastDateM).HasColumnType("date");
            entity.Property(e => e.LastGift).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastGiftM).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastRun).HasColumnType("datetime");
            entity.Property(e => e.MemberId)
                .HasMaxLength(255)
                .HasColumnName("MemberID");
            entity.Property(e => e.NameOnCard).HasMaxLength(255);
            entity.Property(e => e.ProcessDate).HasColumnType("date");
            entity.Property(e => e.ProjectCode).HasMaxLength(255);
            entity.Property(e => e.TermCode).HasMaxLength(255);
            entity.Property(e => e.Title1).HasMaxLength(50);
            entity.Property(e => e.Title2).HasMaxLength(50);
            entity.Property(e => e.TotalGift).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPtp)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalPTP");
            entity.Property(e => e.Will)
                .HasMaxLength(50)
                .HasColumnName("WILL");
        });

        modelBuilder.Entity<ProjectCode>(entity =>
        {
            entity.ToTable("ProjectCode");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProjectCode1)
                .HasMaxLength(255)
                .HasColumnName("ProjectCode");
        });

        modelBuilder.Entity<SourceTable>(entity =>
        {
            entity.ToTable("SourceTable");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SourceTable1)
                .HasMaxLength(255)
                .HasColumnName("SourceTable");
        });

        modelBuilder.Entity<TermCode>(entity =>
        {
            entity.ToTable("TermCode");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.FkTermCodeCategory).HasColumnName("FK_TermCodeCategory");
            entity.Property(e => e.TermCode1)
                .HasMaxLength(255)
                .HasColumnName("TermCode");

            entity.HasOne(d => d.FkTermCodeCategoryNavigation).WithMany(p => p.TermCodes)
                .HasForeignKey(d => d.FkTermCodeCategory)
                .HasConstraintName("FK_TermCode_TermCodeCategory");
        });

        modelBuilder.Entity<TermCodeCategory>(entity =>
        {
            entity.ToTable("TermCodeCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Category).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

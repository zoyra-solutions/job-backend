using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recruitment.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Core entity tables
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<Candidate> Candidates { get; set; }

    // Relationship tables
    public DbSet<Application> Applications { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Contract> Contracts { get; set; }

    // Supporting tables
    public DbSet<CommissionRule> CommissionRules { get; set; }
    public DbSet<Commission> Commissions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Dispute> Disputes { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<KPITracking> KPITrackings { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Company>().ToTable("Companies");
        modelBuilder.Entity<Vacancy>().ToTable("Vacancies");
        modelBuilder.Entity<Candidate>().ToTable("Candidates");
        modelBuilder.Entity<Application>().ToTable("Applications");
        modelBuilder.Entity<Interview>().ToTable("Interviews");
        modelBuilder.Entity<Contract>().ToTable("Contracts");
        modelBuilder.Entity<CommissionRule>().ToTable("CommissionRules");
        modelBuilder.Entity<Commission>().ToTable("Commissions");
        modelBuilder.Entity<Transaction>().ToTable("Transactions");
        modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");
        modelBuilder.Entity<Dispute>().ToTable("Disputes");
        modelBuilder.Entity<Notification>().ToTable("Notifications");
        modelBuilder.Entity<KPITracking>().ToTable("KPITrackings");
        modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");

        // Configure relationships and constraints
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany()
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Vacancy>()
            .HasOne(v => v.Company)
            .WithMany()
            .HasForeignKey(v => v.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vacancy>()
            .HasOne(v => v.CommissionRule)
            .WithMany()
            .HasForeignKey(v => v.CommissionRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Vacancy)
            .WithMany()
            .HasForeignKey(a => a.VacancyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Candidate)
            .WithMany()
            .HasForeignKey(a => a.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Recruiter)
            .WithMany()
            .HasForeignKey(a => a.RecruiterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraints
        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.VacancyId, a.CandidateId })
            .IsUnique();

        modelBuilder.Entity<KPITracking>()
            .HasIndex(k => new { k.UserId, k.TrackingMonth })
            .IsUnique();
    }
}

// Entity models
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FullName { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public Guid? CompanyId { get; set; }
    public string Avatar { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string BankAccountInfo { get; set; }
    public string BankAccountInfo_IV { get; set; }

    public Company Company { get; set; }
}

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TaxCode { get; set; }
    public string Description { get; set; }
    public string Logo { get; set; }
    public string Website { get; set; }
    public string Address { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string Ward { get; set; }
    public string BankInfo { get; set; }
    public string BankInfo_IV { get; set; }
    public Guid AdminUserId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public User AdminUser { get; set; }
}

public class Vacancy
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public int Quantity { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string SalaryType { get; set; }
    public string RequiredSkills { get; set; }
    public int? ExperienceYears { get; set; }
    public string EducationLevel { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public Guid CommissionRuleId { get; set; }
    public decimal EscrowAmount { get; set; }
    public string PaymentPolicy { get; set; }
    public string Status { get; set; }
    public bool IsInternalOnly { get; set; }
    public int PriorityLevel { get; set; }
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Company Company { get; set; }
    public CommissionRule CommissionRule { get; set; }
    public User CreatedByUser { get; set; }
}

public class Candidate
{
    public Guid Id { get; set; }
    public Guid UploadedByUserId { get; set; }
    public string FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string Ward { get; set; }
    public string CurrentPosition { get; set; }
    public int ExperienceYears { get; set; }
    public string EducationLevel { get; set; }
    public string Skills { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public DateTime? AvailabilityDate { get; set; }
    public string Metadata { get; set; }
    public string Notes { get; set; }
    public string CVFilePath { get; set; }
    public string PhotoFilePath { get; set; }
    public string IDCardFilePath { get; set; }
    public string VideoFilePath { get; set; }
    public string Certificates { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public int ViewCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public User UploadedByUser { get; set; }
}

public class Application
{
    public Guid Id { get; set; }
    public Guid VacancyId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid RecruiterId { get; set; }
    public string Status { get; set; }
    public string RecruiterNotes { get; set; }
    public int? RecruiterRating { get; set; }
    public DateTimeOffset AppliedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }

    public Vacancy Vacancy { get; set; }
    public Candidate Candidate { get; set; }
    public User Recruiter { get; set; }
    public User UpdatedByUser { get; set; }
}

public class Interview
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid InterviewerId { get; set; }
    public string Stage { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public DateTimeOffset? ActualStartAt { get; set; }
    public DateTimeOffset? ActualEndAt { get; set; }
    public string Location { get; set; }
    public string MeetingLink { get; set; }
    public string Result { get; set; }
    public int? Score { get; set; }
    public string Notes { get; set; }
    public string Feedback { get; set; }
    public string NextSteps { get; set; }
    public DateTimeOffset? NextInterviewDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Application Application { get; set; }
    public User Interviewer { get; set; }
}

public class Contract
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public decimal ContractValue { get; set; }
    public string Currency { get; set; }
    public decimal MonthlySalary { get; set; }
    public string SalaryType { get; set; }
    public string Benefits { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TrialPeriodDays { get; set; }
    public DateTime? ProbationEndDate { get; set; }
    public string ContractFilePath { get; set; }
    public string DigitalSignatureCandidate { get; set; }
    public string DigitalSignatureEmployer { get; set; }
    public DateTimeOffset? SignedByCandidateAt { get; set; }
    public DateTimeOffset? SignedByEmployerAt { get; set; }
    public string Status { get; set; }
    public string PenaltyClause { get; set; }
    public string TerminationReason { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Application Application { get; set; }
}

public class CommissionRule
{
    public Guid Id { get; set; }
    public Guid VacancyId { get; set; }
    public string RuleType { get; set; }
    public decimal RuleValue { get; set; }
    public string StagedDetails { get; set; }
    public decimal? MinContractValue { get; set; }
    public decimal? MaxContractValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Vacancy Vacancy { get; set; }
}

public class Commission
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid RecruiterId { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public string CalculationBasis { get; set; }
    public Guid? PayoutTransactionId { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
    public string PaymentReference { get; set; }
    public string AdjustmentReason { get; set; }
    public Guid? DisputeId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Application Application { get; set; }
    public User Recruiter { get; set; }
    public Transaction PayoutTransaction { get; set; }
}

public class Transaction
{
    public Guid Id { get; set; }
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public decimal Fee { get; set; }
    public string BankTransactionId { get; set; }
    public string BankReference { get; set; }
    public string BankStatus { get; set; }
    public string Status { get; set; }
    public string FailureReason { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? VacancyId { get; set; }
    public Guid? CommissionId { get; set; }
    public Guid? ProcessedBy { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public DateTimeOffset? WebhookReceivedAt { get; set; }
    public string WebhookData { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Company Company { get; set; }
    public User User { get; set; }
    public Vacancy Vacancy { get; set; }
    public Commission Commission { get; set; }
    public User ProcessedByUser { get; set; }
}

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid ActorId { get; set; }
    public string Action { get; set; }
    public string TargetTable { get; set; }
    public Guid TargetId { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public string Changes { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string SessionId { get; set; }
    public string AdditionalData { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public User Actor { get; set; }
}

public class Dispute
{
    public Guid Id { get; set; }
    public Guid CommissionId { get; set; }
    public Guid RaisedBy { get; set; }
    public string Reason { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Resolution { get; set; }
    public decimal RefundAmount { get; set; }
    public Guid? ResolvedBy { get; set; }
    public DateTimeOffset? ResolvedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Commission Commission { get; set; }
    public User RaisedByUser { get; set; }
    public User ResolvedByUser { get; set; }
}

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public string RelatedEntityType { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public bool IsRead { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
    public string DeliveryStatus { get; set; }
    public string Channel { get; set; }
    public string ExternalMessageId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public User User { get; set; }
}

public class KPITracking
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime TrackingMonth { get; set; }
    public int CVsSubmitted { get; set; }
    public int CVsShortlisted { get; set; }
    public int CVsInterviewed { get; set; }
    public int ContractsSigned { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal AverageTimeToHire { get; set; }
    public decimal TotalEarnings { get; set; }
    public string PerformanceTier { get; set; }
    public DateTimeOffset? TierCalculatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public User User { get; set; }
}

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public string MessageType { get; set; }
    public string Content { get; set; }
    public string FilePath { get; set; }
    public int? FileSize { get; set; }
    public bool IsRead { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public User FromUser { get; set; }
    public User ToUser { get; set; }
}
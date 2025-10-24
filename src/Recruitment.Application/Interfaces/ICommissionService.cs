using Recruitment.Application.DTOs;

namespace Recruitment.Application.Interfaces;

public interface ICommissionService
{
    Task<IEnumerable<Commission>> GetCommissionsAsync(Guid userId, CommissionFilterDto filter);
    Task<Commission> GetCommissionByIdAsync(Guid id, Guid userId);
    Task<Commission> CreateCommissionAsync(CreateCommissionDto dto, Guid userId);
    Task<Commission> UpdateCommissionAsync(Guid id, UpdateCommissionDto dto, Guid userId);
    Task<List<CommissionPaymentResult>> ProcessBulkCommissionPaymentsAsync(BulkCommissionPaymentDto dto, Guid userId);
    Task<RecruiterEarnings> GetRecruiterEarningsAsync(Guid recruiterId, DateTime? periodStart, DateTime? periodEnd);
    Task<List<RecruiterEarnings>> GetAllRecruitersEarningsAsync(DateTime periodStart, DateTime periodEnd);
    Task<decimal> CalculateCommissionAmountAsync(Guid applicationId, Guid commissionRuleId);
    Task<Dispute> DisputeCommissionAsync(Guid commissionId, CreateDisputeDto dto, Guid userId);
    Task<CommissionStatistics> GetCommissionStatisticsAsync(Guid userId);
    Task<string> ExportCommissionsAsync(CommissionFilterDto filter, Guid userId);
    Task<List<Commission>> GetPendingCommissionsAsync();
    Task ProcessPendingCommissionsAsync();
    Task<ReconciliationReport> GetReconciliationReportAsync(DateTime? from, DateTime? to);
    Task<ReconciliationResult> ReconcileTransactionsAsync(ReconcileRequestDto dto);
}

public class CommissionFilterDto
{
    public string Status { get; set; }
    public Guid? RecruiterId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class CreateCommissionDto
{
    public Guid ApplicationId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}

public class UpdateCommissionDto
{
    public decimal Amount { get; set; }
    public string Status { get; set; }
}

public class BulkCommissionPaymentDto
{
    public List<Guid> CommissionIds { get; set; }
}

public class CommissionPaymentResult
{
    public Guid CommissionId { get; set; }
    public bool Success { get; set; }
    public string TransactionId { get; set; }
    public Guid RecruiterId { get; set; }
}

public class RecruiterEarnings
{
    public Guid RecruiterId { get; set; }
    public decimal TotalEarnings { get; set; }
    public int TotalCommissions { get; set; }
    public decimal AverageCommission { get; set; }
}

public class CreateDisputeDto
{
    public string Reason { get; set; }
    public string Description { get; set; }
}

public class CommissionStatistics
{
    public decimal TotalCommissions { get; set; }
    public decimal PaidCommissions { get; set; }
    public decimal PendingCommissions { get; set; }
    public int TotalDisputes { get; set; }
}

public class ReconcileRequestDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class ReconciliationReport
{
    public decimal TotalDeposits { get; set; }
    public decimal TotalPayouts { get; set; }
    public decimal Balance { get; set; }
    public List<Transaction> Transactions { get; set; }
}

public class ReconciliationResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<string> Discrepancies { get; set; }
}
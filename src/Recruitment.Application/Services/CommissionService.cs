using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Application.Services;

public class CommissionService : ICommissionService
{
    private readonly ApplicationDbContext _context;

    public CommissionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Commission>> GetCommissionsAsync(Guid userId, CommissionFilterDto filter)
    {
        var query = _context.Commissions
            .Include(c => c.Application)
            .ThenInclude(a => a.Vacancy)
            .Include(c => c.Recruiter)
            .AsQueryable();

        if (filter.Status != null)
            query = query.Where(c => c.Status == filter.Status);

        if (filter.RecruiterId != null)
            query = query.Where(c => c.RecruiterId == filter.RecruiterId);

        if (filter.FromDate != null)
            query = query.Where(c => c.CreatedAt >= filter.FromDate);

        if (filter.ToDate != null)
            query = query.Where(c => c.CreatedAt <= filter.ToDate);

        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<Commission> GetCommissionByIdAsync(Guid id, Guid userId)
    {
        return await _context.Commissions
            .Include(c => c.Application)
            .Include(c => c.Recruiter)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Commission> CreateCommissionAsync(CreateCommissionDto dto, Guid userId)
    {
        var application = await _context.Applications.FindAsync(dto.ApplicationId);
        if (application == null)
            throw new KeyNotFoundException("Application not found");

        var commission = new Commission
        {
            ApplicationId = dto.ApplicationId,
            RecruiterId = application.RecruiterId,
            OriginalAmount = dto.Amount,
            FinalAmount = dto.Amount,
            Currency = dto.Currency ?? "VND",
            Status = "pending",
            CalculationBasis = "manual",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Commissions.Add(commission);
        await _context.SaveChangesAsync();

        return commission;
    }

    public async Task<Commission> UpdateCommissionAsync(Guid id, UpdateCommissionDto dto, Guid userId)
    {
        var commission = await _context.Commissions.FindAsync(id);
        if (commission == null)
            throw new KeyNotFoundException("Commission not found");

        commission.FinalAmount = dto.Amount;
        commission.Status = dto.Status;
        commission.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return commission;
    }

    public async Task<List<CommissionPaymentResult>> ProcessBulkCommissionPaymentsAsync(BulkCommissionPaymentDto dto, Guid userId)
    {
        var commissions = await _context.Commissions
            .Where(c => dto.CommissionIds.Contains(c.Id) && c.Status == "approved")
            .ToListAsync();

        var results = new List<CommissionPaymentResult>();

        foreach (var commission in commissions)
        {
            try
            {
                // Process payment
                var transaction = new Transaction
                {
                    TransactionType = "commission_payout",
                    Amount = commission.FinalAmount,
                    Currency = commission.Currency,
                    Status = "completed",
                    UserId = commission.RecruiterId,
                    CommissionId = commission.Id,
                    ProcessedBy = userId,
                    ProcessedAt = DateTimeOffset.UtcNow,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                _context.Transactions.Add(transaction);

                commission.Status = "paid";
                commission.PayoutTransactionId = transaction.Id;
                commission.PaidAt = DateTimeOffset.UtcNow;
                commission.UpdatedAt = DateTimeOffset.UtcNow;

                results.Add(new CommissionPaymentResult
                {
                    CommissionId = commission.Id,
                    Success = true,
                    TransactionId = transaction.Id.ToString(),
                    RecruiterId = commission.RecruiterId
                });
            }
            catch (Exception ex)
            {
                results.Add(new CommissionPaymentResult
                {
                    CommissionId = commission.Id,
                    Success = false,
                    TransactionId = "",
                    RecruiterId = commission.RecruiterId
                });
            }
        }

        await _context.SaveChangesAsync();
        return results;
    }

    public async Task<RecruiterEarnings> GetRecruiterEarningsAsync(Guid recruiterId, DateTime? periodStart, DateTime? periodEnd)
    {
        var query = _context.Commissions
            .Where(c => c.RecruiterId == recruiterId && c.Status == "paid");

        if (periodStart != null)
            query = query.Where(c => c.PaidAt >= periodStart);

        if (periodEnd != null)
            query = query.Where(c => c.PaidAt <= periodEnd);

        var commissions = await query.ToListAsync();

        return new RecruiterEarnings
        {
            RecruiterId = recruiterId,
            TotalEarnings = commissions.Sum(c => c.FinalAmount),
            TotalCommissions = commissions.Count,
            AverageCommission = commissions.Any() ? commissions.Average(c => c.FinalAmount) : 0
        };
    }

    public async Task<List<RecruiterEarnings>> GetAllRecruitersEarningsAsync(DateTime periodStart, DateTime periodEnd)
    {
        var earnings = await _context.Commissions
            .Where(c => c.Status == "paid" && c.PaidAt >= periodStart && c.PaidAt <= periodEnd)
            .GroupBy(c => c.RecruiterId)
            .Select(g => new RecruiterEarnings
            {
                RecruiterId = g.Key,
                TotalEarnings = g.Sum(c => c.FinalAmount),
                TotalCommissions = g.Count(),
                AverageCommission = g.Average(c => c.FinalAmount)
            })
            .ToListAsync();

        return earnings;
    }

    public async Task<decimal> CalculateCommissionAmountAsync(Guid applicationId, Guid commissionRuleId)
    {
        var application = await _context.Applications
            .Include(a => a.Vacancy)
            .ThenInclude(v => v.CommissionRule)
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null)
            throw new KeyNotFoundException("Application not found");

        var rule = application.Vacancy.CommissionRule;
        var contract = await _context.Contracts.FirstOrDefaultAsync(c => c.ApplicationId == applicationId);

        if (contract == null)
            return 0;

        decimal amount = 0;

        switch (rule.RuleType)
        {
            case "fixed":
                amount = rule.RuleValue;
                break;
            case "percentage":
                amount = contract.ContractValue * (rule.RuleValue / 100);
                break;
            case "staged":
                // Implement staged calculation logic
                amount = rule.RuleValue;
                break;
        }

        return Math.Min(amount, rule.MaxContractValue ?? amount);
    }

    public async Task<Dispute> DisputeCommissionAsync(Guid commissionId, CreateDisputeDto dto, Guid userId)
    {
        var commission = await _context.Commissions.FindAsync(commissionId);
        if (commission == null)
            throw new KeyNotFoundException("Commission not found");

        var dispute = new Dispute
        {
            CommissionId = commissionId,
            RaisedBy = userId,
            Reason = dto.Reason,
            Description = dto.Description,
            Status = "open",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Disputes.Add(dispute);

        commission.Status = "disputed";
        commission.DisputeId = dispute.Id;
        commission.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return dispute;
    }

    public async Task<CommissionStatistics> GetCommissionStatisticsAsync(Guid userId)
    {
        var commissions = await _context.Commissions.ToListAsync();

        return new CommissionStatistics
        {
            TotalCommissions = commissions.Sum(c => c.FinalAmount),
            PaidCommissions = commissions.Where(c => c.Status == "paid").Sum(c => c.FinalAmount),
            PendingCommissions = commissions.Where(c => c.Status == "pending").Sum(c => c.FinalAmount),
            TotalDisputes = await _context.Disputes.CountAsync()
        };
    }

    public async Task<string> ExportCommissionsAsync(CommissionFilterDto filter, Guid userId)
    {
        var commissions = await GetCommissionsAsync(userId, filter);

        // Generate CSV content
        var csv = "Id,ApplicationId,RecruiterId,Amount,Status,CreatedAt\n";
        foreach (var commission in commissions)
        {
            csv += $"{commission.Id},{commission.ApplicationId},{commission.RecruiterId},{commission.FinalAmount},{commission.Status},{commission.CreatedAt}\n";
        }

        return csv;
    }

    public async Task<List<Commission>> GetPendingCommissionsAsync()
    {
        return await _context.Commissions
            .Where(c => c.Status == "approved")
            .Include(c => c.Application)
            .Include(c => c.Recruiter)
            .ToListAsync();
    }

    public async Task ProcessPendingCommissionsAsync()
    {
        var pendingCommissions = await GetPendingCommissionsAsync();

        foreach (var commission in pendingCommissions)
        {
            // Auto-process commission payment
            commission.Status = "paid";
            commission.PaidAt = DateTimeOffset.UtcNow;
            commission.UpdatedAt = DateTimeOffset.UtcNow;

            var transaction = new Transaction
            {
                TransactionType = "commission_payout",
                Amount = commission.FinalAmount,
                Currency = commission.Currency,
                Status = "completed",
                UserId = commission.RecruiterId,
                CommissionId = commission.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _context.Transactions.Add(transaction);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<ReconciliationReport> GetReconciliationReportAsync(DateTime? from, DateTime? to)
    {
        var query = _context.Transactions.AsQueryable();

        if (from != null)
            query = query.Where(t => t.CreatedAt >= from);

        if (to != null)
            query = query.Where(t => t.CreatedAt <= to);

        var transactions = await query.ToListAsync();

        return new ReconciliationReport
        {
            TotalDeposits = transactions.Where(t => t.TransactionType == "deposit").Sum(t => t.Amount),
            TotalPayouts = transactions.Where(t => t.TransactionType == "commission_payout").Sum(t => t.Amount),
            Balance = transactions.Sum(t => t.Amount),
            Transactions = transactions
        };
    }

    public async Task<ReconciliationResult> ReconcileTransactionsAsync(ReconcileRequestDto dto)
    {
        var report = await GetReconciliationReportAsync(dto.From, dto.To);

        var discrepancies = new List<string>();

        // Check for discrepancies
        if (report.Balance != 0)
            discrepancies.Add($"Balance mismatch: {report.Balance}");

        return new ReconciliationResult
        {
            Success = !discrepancies.Any(),
            Message = discrepancies.Any() ? "Discrepancies found" : "Reconciliation successful",
            Discrepancies = discrepancies
        };
    }
}
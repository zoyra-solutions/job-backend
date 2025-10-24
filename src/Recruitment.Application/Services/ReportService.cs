using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Application.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<KPIReport> GetKpiReportAsync(Guid? recruiterId, DateTime? from, DateTime? to, Guid userId)
    {
        var query = _context.KPITrackings.AsQueryable();

        if (recruiterId != null)
            query = query.Where(k => k.UserId == recruiterId);

        if (from != null)
            query = query.Where(k => k.TrackingMonth >= from.Value.Date);

        if (to != null)
            query = query.Where(k => k.TrackingMonth <= to.Value.Date);

        var kpi = await query.FirstOrDefaultAsync();

        if (kpi == null)
        {
            return new KPIReport
            {
                RecruiterId = recruiterId ?? Guid.Empty,
                CVsSubmitted = 0,
                CVsShortlisted = 0,
                CVsInterviewed = 0,
                ContractsSigned = 0,
                ConversionRate = 0,
                AverageTimeToHire = 0,
                TotalEarnings = 0,
                PerformanceTier = "bronze"
            };
        }

        return new KPIReport
        {
            RecruiterId = kpi.UserId,
            CVsSubmitted = kpi.CVsSubmitted,
            CVsShortlisted = kpi.CVsShortlisted,
            CVsInterviewed = kpi.CVsInterviewed,
            ContractsSigned = kpi.ContractsSigned,
            ConversionRate = kpi.ConversionRate,
            AverageTimeToHire = kpi.AverageTimeToHire,
            TotalEarnings = kpi.TotalEarnings,
            PerformanceTier = kpi.PerformanceTier
        };
    }

    public async Task<VacancyReport> GetVacancyReportAsync(Guid? vacancyId, DateTime? from, DateTime? to, Guid userId)
    {
        var query = _context.Applications.AsQueryable();

        if (vacancyId != null)
            query = query.Where(a => a.VacancyId == vacancyId);

        if (from != null)
            query = query.Where(a => a.AppliedAt >= from);

        if (to != null)
            query = query.Where(a => a.AppliedAt <= to);

        var applications = await query.ToListAsync();

        var vacancy = vacancyId != null ? await _context.Vacancies.FindAsync(vacancyId) : null;

        return new VacancyReport
        {
            VacancyId = vacancyId ?? Guid.Empty,
            TotalApplications = applications.Count,
            Shortlisted = applications.Count(a => a.Status == "shortlisted"),
            Interviewed = applications.Count(a => a.Status == "interviewed"),
            Hired = applications.Count(a => a.Status == "contract_signed"),
            AverageTimeToHire = CalculateAverageTimeToHire(applications),
            CostPerHire = vacancy != null && applications.Any(a => a.Status == "contract_signed") ?
                vacancy.EscrowAmount / applications.Count(a => a.Status == "contract_signed") : 0
        };
    }

    public async Task<CommissionReport> GetCommissionReportAsync(DateTime? from, DateTime? to)
    {
        var query = _context.Commissions.AsQueryable();

        if (from != null)
            query = query.Where(c => c.CreatedAt >= from);

        if (to != null)
            query = query.Where(c => c.CreatedAt <= to);

        var commissions = await query.ToListAsync();

        return new CommissionReport
        {
            TotalCommissions = commissions.Sum(c => c.FinalAmount),
            PaidCommissions = commissions.Where(c => c.Status == "paid").Sum(c => c.FinalAmount),
            PendingCommissions = commissions.Where(c => c.Status == "pending").Sum(c => c.FinalAmount),
            TotalDisputes = await _context.Disputes.CountAsync(),
            Commissions = commissions
        };
    }

    public async Task<byte[]> ExportReportAsync(string type, Guid? id, DateTime? from, DateTime? to, Guid userId)
    {
        string csvContent = "";

        switch (type.ToLower())
        {
            case "kpi":
                var kpiReport = await GetKpiReportAsync(id, from, to, userId);
                csvContent = $"RecruiterId,CVsSubmitted,CVsShortlisted,CVsInterviewed,ContractsSigned,ConversionRate,AverageTimeToHire,TotalEarnings,PerformanceTier\n";
                csvContent += $"{kpiReport.RecruiterId},{kpiReport.CVsSubmitted},{kpiReport.CVsShortlisted},{kpiReport.CVsInterviewed},{kpiReport.ContractsSigned},{kpiReport.ConversionRate},{kpiReport.AverageTimeToHire},{kpiReport.TotalEarnings},{kpiReport.PerformanceTier}\n";
                break;

            case "vacancy":
                var vacancyReport = await GetVacancyReportAsync(id, from, to, userId);
                csvContent = $"VacancyId,TotalApplications,Shortlisted,Interviewed,Hired,AverageTimeToHire,CostPerHire\n";
                csvContent += $"{vacancyReport.VacancyId},{vacancyReport.TotalApplications},{vacancyReport.Shortlisted},{vacancyReport.Interviewed},{vacancyReport.Hired},{vacancyReport.AverageTimeToHire},{vacancyReport.CostPerHire}\n";
                break;

            case "commission":
                var commissionReport = await GetCommissionReportAsync(from, to);
                csvContent = $"TotalCommissions,PaidCommissions,PendingCommissions,TotalDisputes\n";
                csvContent += $"{commissionReport.TotalCommissions},{commissionReport.PaidCommissions},{commissionReport.PendingCommissions},{commissionReport.TotalDisputes}\n";
                break;
        }

        return System.Text.Encoding.UTF8.GetBytes(csvContent);
    }

    private decimal CalculateAverageTimeToHire(List<Application> applications)
    {
        var hiredApplications = applications.Where(a => a.Status == "contract_signed").ToList();

        if (!hiredApplications.Any())
            return 0;

        var totalDays = hiredApplications.Sum(a =>
        {
            var contract = _context.Contracts.FirstOrDefault(c => c.ApplicationId == a.Id);
            return contract != null ? (contract.StartDate - a.AppliedAt.DateTime.Date).Days : 0;
        });

        return totalDays / hiredApplications.Count;
    }
}
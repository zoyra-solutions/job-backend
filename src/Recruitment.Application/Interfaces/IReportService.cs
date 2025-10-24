using Recruitment.Application.DTOs;

namespace Recruitment.Application.Interfaces;

public interface IReportService
{
    Task<KPIReport> GetKpiReportAsync(Guid? recruiterId, DateTime? from, DateTime? to, Guid userId);
    Task<VacancyReport> GetVacancyReportAsync(Guid? vacancyId, DateTime? from, DateTime? to, Guid userId);
    Task<CommissionReport> GetCommissionReportAsync(DateTime? from, DateTime? to);
    Task<byte[]> ExportReportAsync(string type, Guid? id, DateTime? from, DateTime? to, Guid userId);
}

public class KPIReport
{
    public Guid RecruiterId { get; set; }
    public int CVsSubmitted { get; set; }
    public int CVsShortlisted { get; set; }
    public int CVsInterviewed { get; set; }
    public int ContractsSigned { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal AverageTimeToHire { get; set; }
    public decimal TotalEarnings { get; set; }
    public string PerformanceTier { get; set; }
}

public class VacancyReport
{
    public Guid VacancyId { get; set; }
    public int TotalApplications { get; set; }
    public int Shortlisted { get; set; }
    public int Interviewed { get; set; }
    public int Hired { get; set; }
    public decimal AverageTimeToHire { get; set; }
    public decimal CostPerHire { get; set; }
}

public class CommissionReport
{
    public decimal TotalCommissions { get; set; }
    public decimal PaidCommissions { get; set; }
    public decimal PendingCommissions { get; set; }
    public int TotalDisputes { get; set; }
    public List<Commission> Commissions { get; set; }
}
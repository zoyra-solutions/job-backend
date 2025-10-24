using System.ComponentModel.DataAnnotations;

namespace Recruitment.Application.DTOs;

public class CreateVacancyDto
{
    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Location { get; set; } = string.Empty;

    public string? Province { get; set; }
    public string? District { get; set; }

    [Required]
    [Range(1, 10000)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalaryMin { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalaryMax { get; set; }

    public string SalaryType { get; set; } = "monthly";

    public List<string>? RequiredSkills { get; set; }

    [Range(0, 50)]
    public int? ExperienceYears { get; set; }

    public string? EducationLevel { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public DateTime ApplicationDeadline { get; set; }

    [Required]
    public Guid CommissionRuleId { get; set; }

    public decimal? EscrowAmount { get; set; }
    public string PaymentPolicy { get; set; } = "post_paid";

    public bool IsInternalOnly { get; set; } = false;
    public int PriorityLevel { get; set; } = 1;
}

public class UpdateVacancyDto
{
    [StringLength(255)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [StringLength(500)]
    public string? Location { get; set; }

    public string? Province { get; set; }
    public string? District { get; set; }

    [Range(1, 10000)]
    public int? Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalaryMin { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalaryMax { get; set; }

    public string? SalaryType { get; set; }
    public List<string>? RequiredSkills { get; set; }

    [Range(0, 50)]
    public int? ExperienceYears { get; set; }

    public string? EducationLevel { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ApplicationDeadline { get; set; }

    public string? Status { get; set; }
    public bool? IsInternalOnly { get; set; }
    public int? PriorityLevel { get; set; }
}

public class VacancyFilterDto
{
    public string? Status { get; set; }
    public string? Province { get; set; }
    public int? PriorityLevel { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "desc";
}

public class VacancyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Province { get; set; }
    public string? District { get; set; }
    public int Quantity { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string SalaryType { get; set; } = "monthly";
    public List<string>? RequiredSkills { get; set; }
    public int? ExperienceYears { get; set; }
    public string? EducationLevel { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public Guid CommissionRuleId { get; set; }
    public decimal? EscrowAmount { get; set; }
    public string PaymentPolicy { get; set; } = "post_paid";
    public string Status { get; set; } = "draft";
    public bool IsInternalOnly { get; set; }
    public int PriorityLevel { get; set; }
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Related data
    public CompanyDto? Company { get; set; }
    public CommissionRuleDto? CommissionRule { get; set; }
}

public class CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Logo { get; set; }
}

public class CommissionRuleDto
{
    public Guid Id { get; set; }
    public string RuleType { get; set; } = string.Empty;
    public decimal RuleValue { get; set; }
}

public class VacancyStatisticsDto
{
    public int TotalApplications { get; set; }
    public int NewApplications { get; set; }
    public int ShortlistedCount { get; set; }
    public int InterviewedCount { get; set; }
    public int HiredCount { get; set; }
    public int RejectedCount { get; set; }
    public double AverageTimeToHire { get; set; }
    public double ConversionRate { get; set; }
}
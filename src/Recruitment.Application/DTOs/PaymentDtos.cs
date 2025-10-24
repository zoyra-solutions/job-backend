using System.ComponentModel.DataAnnotations;

namespace Recruitment.Application.DTOs;

public class PaymentDto
{
    public Guid Id { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "VND";
    public decimal Fee { get; set; }
    public string Status { get; set; } = "pending";
    public string? BankTransactionId { get; set; }
    public string? BankReference { get; set; }
    public string? BankStatus { get; set; }
    public string? FailureReason { get; set; }

    // Related entities
    public Guid? CompanyId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? VacancyId { get; set; }
    public Guid? CommissionId { get; set; }

    public string? ProcessedBy { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime? WebhookReceivedAt { get; set; }
    public string? WebhookData { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Related data
    public CompanyDto? Company { get; set; }
    public UserDto? User { get; set; }
    public CommissionDto? Commission { get; set; }
}

public class CreatePaymentDto
{
    [Required]
    public string PaymentType { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public string Currency { get; set; } = "VND";
    public decimal Fee { get; set; } = 0;

    [Required]
    public Guid CompanyId { get; set; }

    public Guid? UserId { get; set; }
    public Guid? VacancyId { get; set; }
    public Guid? CommissionId { get; set; }

    public Dictionary<string, string>? Metadata { get; set; }
}

public class ProcessPaymentDto
{
    [Required]
    public Guid PaymentId { get; set; }

    public string? PaymentReference { get; set; }
    public Dictionary<string, object>? BankResponse { get; set; }
}

public class PaymentFilterDto
{
    public string? PaymentType { get; set; }
    public string? Status { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "desc";
}

public class BankPaymentRequestDto
{
    [Required]
    public string BankCode { get; set; } = string.Empty;

    [Required]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    public string AccountHolder { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public string Currency { get; set; } = "VND";
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }

    // Bank-specific fields
    public Dictionary<string, string>? BankSpecificFields { get; set; }
}

public class BankPaymentResponseDto
{
    public bool Success { get; set; }
    public string? TransactionId { get; set; }
    public string? BankReference { get; set; }
    public string? Status { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, object>? BankResponse { get; set; }
    public DateTime ProcessedAt { get; set; }
}

public class WebhookDto
{
    [Required]
    public string EventType { get; set; } = string.Empty;

    [Required]
    public string BankCode { get; set; } = string.Empty;

    public string? TransactionId { get; set; }
    public string? BankReference { get; set; }
    public string? Status { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? TransactionDate { get; set; }

    public Dictionary<string, object>? AdditionalData { get; set; }
    public string? Signature { get; set; }
    public DateTime ReceivedAt { get; set; }
}

public class PaymentReconciliationDto
{
    public Guid PaymentId { get; set; }
    public string? ExternalTransactionId { get; set; }
    public string? ExternalReference { get; set; }
    public decimal ExternalAmount { get; set; }
    public string ExternalCurrency { get; set; } = "VND";
    public DateTime ExternalTransactionDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<string> Discrepancies { get; set; } = new();
    public bool IsMatched { get; set; }
    public DateTime ReconciledAt { get; set; }
}

public class DailyTransactionSummaryDto
{
    public DateTime Date { get; set; }
    public int TotalTransactions { get; set; }
    public decimal TotalAmount { get; set; }
    public int SuccessfulTransactions { get; set; }
    public decimal SuccessfulAmount { get; set; }
    public int FailedTransactions { get; set; }
    public decimal FailedAmount { get; set; }
    public int PendingTransactions { get; set; }
    public decimal PendingAmount { get; set; }
    public Dictionary<string, int> TransactionsByType { get; set; } = new();
    public Dictionary<string, decimal> AmountByType { get; set; } = new();
}
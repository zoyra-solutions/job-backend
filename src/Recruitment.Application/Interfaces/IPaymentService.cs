using Recruitment.Application.DTOs;

namespace Recruitment.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto dto, Guid createdBy);
    Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId, Guid userId);
    Task<IEnumerable<PaymentDto>> GetPaymentsAsync(Guid userId, PaymentFilterDto filter);

    Task<BankPaymentResponseDto> ProcessBankPaymentAsync(Guid paymentId, BankPaymentRequestDto request);
    Task<WebhookDto> ProcessPaymentWebhookAsync(Dictionary<string, object> webhookData, string signature);

    Task<IEnumerable<PaymentReconciliationDto>> ReconcileTransactionsAsync(DateTime fromDate, DateTime toDate);
    Task<DailyTransactionSummaryDto> GetDailyTransactionSummaryAsync(DateTime date);

    Task<PaymentDto> UpdatePaymentStatusAsync(Guid paymentId, string status, string? failureReason = null);

    Task<IEnumerable<PaymentDto>> GetPendingPaymentsAsync();
    Task ProcessPendingPaymentsAsync();
}
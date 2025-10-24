using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Recruitment.Infrastructure.Services;

public class PaymentService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public PaymentService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<string> CreatePaymentAsync(decimal amount, string description, string callbackUrl)
    {
        var bankApiUrl = _configuration["Payments:BankApiUrl"];
        var apiKey = _configuration["Payments:ApiKey"];

        var paymentData = new
        {
            amount = amount,
            description = description,
            callbackUrl = callbackUrl
        };

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await _httpClient.PostAsJsonAsync($"{bankApiUrl}/payments", paymentData);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("paymentId").GetString();
    }

    public async Task<bool> VerifyPaymentAsync(string paymentId)
    {
        var bankApiUrl = _configuration["Payments:BankApiUrl"];
        var apiKey = _configuration["Payments:ApiKey"];

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await _httpClient.GetAsync($"{bankApiUrl}/payments/{paymentId}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("status").GetString() == "completed";
    }

    public async Task<string> TransferMoneyAsync(string fromAccount, string toAccount, decimal amount, string description)
    {
        var bankApiUrl = _configuration["Payments:BankApiUrl"];
        var apiKey = _configuration["Payments:ApiKey"];

        var transferData = new
        {
            fromAccount = fromAccount,
            toAccount = toAccount,
            amount = amount,
            description = description
        };

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await _httpClient.PostAsJsonAsync($"{bankApiUrl}/transfers", transferData);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("transactionId").GetString();
    }
}
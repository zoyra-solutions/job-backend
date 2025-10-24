using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Recruitment.Application.Interfaces;
using Recruitment.Domain.Entities;

namespace Recruitment.Infrastructure.Services;

public class BlockchainService : IBlockchainService
{
    private readonly ILogger<BlockchainService> _logger;
    private readonly List<Block> _blockchain = new();
    private readonly List<Transaction> _pendingTransactions = new();
    private const int Difficulty = 4; // Mining difficulty

    public BlockchainService(ILogger<BlockchainService> logger)
    {
        _logger = logger;
        InitializeBlockchain();
    }

    private void InitializeBlockchain()
    {
        // Create genesis block
        var genesisBlock = new Block
        {
            Index = 0,
            Timestamp = DateTime.UtcNow,
            Transactions = new List<Transaction>(),
            PreviousHash = "0",
            Nonce = 0
        };

        genesisBlock.Hash = MineBlock(genesisBlock);
        _blockchain.Add(genesisBlock);

        _logger.LogInformation("Blockchain initialized with genesis block");
    }

    public async Task<string> CreateCommissionTransactionAsync(
        Guid commissionId,
        Guid recruiterId,
        Guid employerId,
        decimal amount,
        string currency,
        string description)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            CommissionId = commissionId,
            RecruiterId = recruiterId,
            EmployerId = employerId,
            Amount = amount,
            Currency = currency,
            Description = description,
            Timestamp = DateTime.UtcNow,
            Status = TransactionStatus.Pending,
            Hash = string.Empty
        };

        // Create transaction hash
        transaction.Hash = CalculateTransactionHash(transaction);

        _pendingTransactions.Add(transaction);

        _logger.LogInformation($"Commission transaction created: {transaction.Id}");

        return transaction.Id;
    }

    public async Task<bool> VerifyCommissionTransactionAsync(string transactionId)
    {
        var transaction = _pendingTransactions.FirstOrDefault(t => t.Id == transactionId);
        if (transaction == null)
        {
            _logger.LogWarning($"Transaction not found: {transactionId}");
            return false;
        }

        // Verify transaction integrity
        var calculatedHash = CalculateTransactionHash(transaction);
        if (calculatedHash != transaction.Hash)
        {
            _logger.LogWarning($"Transaction hash mismatch for: {transactionId}");
            return false;
        }

        // Check if recruiter and employer exist and are active
        // In real implementation, this would query the database

        return true;
    }

    public async Task<string> MinePendingTransactionsAsync(string minerAddress)
    {
        if (!_pendingTransactions.Any())
        {
            throw new InvalidOperationException("No pending transactions to mine");
        }

        var block = new Block
        {
            Index = _blockchain.Count,
            Timestamp = DateTime.UtcNow,
            Transactions = new List<Transaction>(_pendingTransactions),
            PreviousHash = _blockchain.Last().Hash,
            Nonce = 0
        };

        // Mine the block
        block.Hash = MineBlock(block);

        // Add to blockchain
        _blockchain.Add(block);

        // Clear pending transactions
        _pendingTransactions.Clear();

        _logger.LogInformation($"Block mined successfully: {block.Hash}");

        return block.Hash;
    }

    public async Task<bool> ValidateBlockchainAsync()
    {
        for (int i = 1; i < _blockchain.Count; i++)
        {
            var currentBlock = _blockchain[i];
            var previousBlock = _blockchain[i - 1];

            // Validate current block hash
            if (currentBlock.Hash != CalculateBlockHash(currentBlock))
            {
                _logger.LogError($"Block {i} hash validation failed");
                return false;
            }

            // Validate previous block reference
            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                _logger.LogError($"Block {i} previous hash validation failed");
                return false;
            }
        }

        return true;
    }

    public async Task<Block> GetBlockByHashAsync(string hash)
    {
        return _blockchain.FirstOrDefault(b => b.Hash == hash);
    }

    public async Task<IEnumerable<Block>> GetBlockchainAsync()
    {
        return _blockchain.AsReadOnly();
    }

    public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
    {
        // Search in all blocks
        foreach (var block in _blockchain)
        {
            var transaction = block.Transactions.FirstOrDefault(t => t.Id == transactionId);
            if (transaction != null)
                return transaction;
        }

        throw new KeyNotFoundException($"Transaction not found: {transactionId}");
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByRecruiterAsync(Guid recruiterId)
    {
        var transactions = new List<Transaction>();

        foreach (var block in _blockchain)
        {
            transactions.AddRange(
                block.Transactions.Where(t => t.RecruiterId == recruiterId)
            );
        }

        return transactions;
    }

    public async Task<decimal> GetTotalCommissionEarnedAsync(Guid recruiterId)
    {
        var transactions = await GetTransactionsByRecruiterAsync(recruiterId);
        return transactions
            .Where(t => t.Status == TransactionStatus.Confirmed)
            .Sum(t => t.Amount);
    }

    public async Task<bool> DisputeCommissionAsync(string transactionId, string reason)
    {
        var transaction = await GetTransactionByIdAsync(transactionId);
        if (transaction == null)
            return false;

        // Create dispute transaction
        var disputeTransaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            CommissionId = transaction.CommissionId,
            RecruiterId = transaction.RecruiterId,
            EmployerId = transaction.EmployerId,
            Amount = -transaction.Amount, // Negative amount for dispute
            Currency = transaction.Currency,
            Description = $"Dispute: {reason}",
            Timestamp = DateTime.UtcNow,
            Status = TransactionStatus.Disputed,
            Hash = string.Empty
        };

        disputeTransaction.Hash = CalculateTransactionHash(disputeTransaction);
        _pendingTransactions.Add(disputeTransaction);

        _logger.LogInformation($"Commission dispute created: {disputeTransaction.Id}");

        return true;
    }

    private string MineBlock(Block block)
    {
        var hash = CalculateBlockHash(block);

        while (!hash.StartsWith(new string('0', Difficulty)))
        {
            block.Nonce++;
            hash = CalculateBlockHash(block);
        }

        return hash;
    }

    private string CalculateBlockHash(Block block)
    {
        var blockData = $"{block.Index}{block.Timestamp}{JsonConvert.SerializeObject(block.Transactions)}{block.PreviousHash}{block.Nonce}";
        return CalculateSHA256(blockData);
    }

    private string CalculateTransactionHash(Transaction transaction)
    {
        var transactionData = $"{transaction.Id}{transaction.CommissionId}{transaction.RecruiterId}{transaction.EmployerId}{transaction.Amount}{transaction.Currency}{transaction.Description}{transaction.Timestamp}";
        return CalculateSHA256(transactionData);
    }

    private string CalculateSHA256(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash).ToLower();
    }

    public async Task<string> GetBlockchainStatsAsync()
    {
        var stats = new
        {
            TotalBlocks = _blockchain.Count,
            TotalTransactions = _blockchain.Sum(b => b.Transactions.Count),
            PendingTransactions = _pendingTransactions.Count,
            LastBlockHash = _blockchain.LastOrDefault()?.Hash,
            BlockchainValid = await ValidateBlockchainAsync()
        };

        return JsonConvert.SerializeObject(stats, Formatting.Indented);
    }
}

// Domain models for blockchain
public class Block
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
    public string PreviousHash { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public int Nonce { get; set; }
}

public class Transaction
{
    public string Id { get; set; } = string.Empty;
    public Guid CommissionId { get; set; }
    public Guid RecruiterId { get; set; }
    public Guid EmployerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "VND";
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public TransactionStatus Status { get; set; }
    public string Hash { get; set; } = string.Empty;
}

public enum TransactionStatus
{
    Pending,
    Confirmed,
    Disputed,
    Resolved,
    Cancelled
}

// Blockchain verification service
public class BlockchainVerificationService : IBlockchainVerificationService
{
    private readonly IBlockchainService _blockchainService;
    private readonly ILogger<BlockchainVerificationService> _logger;

    public BlockchainVerificationService(
        IBlockchainService blockchainService,
        ILogger<BlockchainVerificationService> logger)
    {
        _blockchainService = blockchainService;
        _logger = logger;
    }

    public async Task<bool> VerifyCommissionPaymentAsync(Guid commissionId)
    {
        try
        {
            // Get all transactions for this commission
            var transactions = new List<Transaction>();

            foreach (var block in await _blockchainService.GetBlockchainAsync())
            {
                transactions.AddRange(
                    block.Transactions.Where(t => t.CommissionId == commissionId)
                );
            }

            // Check if there's a confirmed transaction
            var confirmedTransaction = transactions
                .FirstOrDefault(t => t.Status == TransactionStatus.Confirmed);

            if (confirmedTransaction == null)
            {
                _logger.LogWarning($"No confirmed transaction found for commission: {commissionId}");
                return false;
            }

            // Verify transaction hash
            var isValid = await _blockchainService.VerifyCommissionTransactionAsync(confirmedTransaction.Id);

            if (!isValid)
            {
                _logger.LogWarning($"Transaction verification failed for: {confirmedTransaction.Id}");
                return false;
            }

            _logger.LogInformation($"Commission payment verified on blockchain: {commissionId}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error verifying commission payment: {commissionId}");
            return false;
        }
    }

    public async Task<CommissionVerificationResult> GetCommissionVerificationResultAsync(Guid commissionId)
    {
        var transactions = new List<Transaction>();

        foreach (var block in await _blockchainService.GetBlockchainAsync())
        {
            transactions.AddRange(
                block.Transactions.Where(t => t.CommissionId == commissionId)
            );
        }

        var confirmedTransaction = transactions
            .FirstOrDefault(t => t.Status == TransactionStatus.Confirmed);

        if (confirmedTransaction == null)
        {
            return new CommissionVerificationResult
            {
                IsVerified = false,
                Status = "No blockchain record found",
                BlockIndex = null,
                TransactionHash = null,
                Timestamp = null
            };
        }

        // Find which block contains this transaction
        int? blockIndex = null;
        foreach (var block in await _blockchainService.GetBlockchainAsync())
        {
            if (block.Transactions.Any(t => t.Id == confirmedTransaction.Id))
            {
                blockIndex = block.Index;
                break;
            }
        }

        return new CommissionVerificationResult
        {
            IsVerified = true,
            Status = "Verified on blockchain",
            BlockIndex = blockIndex,
            TransactionHash = confirmedTransaction.Hash,
            Timestamp = confirmedTransaction.Timestamp,
            Amount = confirmedTransaction.Amount,
            Currency = confirmedTransaction.Currency
        };
    }

    public async Task<bool> CreateImmutableCommissionRecordAsync(
        Guid commissionId,
        Guid recruiterId,
        Guid employerId,
        decimal amount,
        string description)
    {
        try
        {
            // Create blockchain transaction
            var transactionId = await _blockchainService.CreateCommissionTransactionAsync(
                commissionId, recruiterId, employerId, amount, "VND", description);

            // Mine the transaction into a block
            await _blockchainService.MinePendingTransactionsAsync("system");

            _logger.LogInformation($"Immutable commission record created: {transactionId}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create immutable commission record: {commissionId}");
            return false;
        }
    }
}

public class CommissionVerificationResult
{
    public bool IsVerified { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? BlockIndex { get; set; }
    public string? TransactionHash { get; set; }
    public DateTime? Timestamp { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
}

// Smart contract interface for commission automation
public interface ISmartContractService
{
    Task<SmartContract> DeployCommissionContractAsync(CommissionContractDto contractDto);
    Task<bool> ExecuteCommissionPayoutAsync(string contractAddress, Guid commissionId);
    Task<ContractState> GetContractStateAsync(string contractAddress);
    Task<IEnumerable<Transaction>> GetContractTransactionsAsync(string contractAddress);
}

public class SmartContractService : ISmartContractService
{
    private readonly ILogger<SmartContractService> _logger;

    public SmartContractService(ILogger<SmartContractService> logger)
    {
        _logger = logger;
    }

    public async Task<SmartContract> DeployCommissionContractAsync(CommissionContractDto contractDto)
    {
        // In real implementation, this would deploy to Ethereum/Polygon
        var contract = new SmartContract
        {
            Address = $"0x{GenerateRandomAddress()}",
            DeployerId = contractDto.CreatedBy,
            CommissionRuleId = contractDto.CommissionRuleId,
            CreatedAt = DateTime.UtcNow,
            State = ContractState.Active
        };

        _logger.LogInformation($"Smart contract deployed: {contract.Address}");

        return contract;
    }

    public async Task<bool> ExecuteCommissionPayoutAsync(string contractAddress, Guid commissionId)
    {
        // In real implementation, this would call the smart contract
        _logger.LogInformation($"Commission payout executed via smart contract: {contractAddress}");

        return true;
    }

    public async Task<ContractState> GetContractStateAsync(string contractAddress)
    {
        // In real implementation, this would query the blockchain
        return ContractState.Active;
    }

    public async Task<IEnumerable<Transaction>> GetContractTransactionsAsync(string contractAddress)
    {
        // In real implementation, this would get contract events from blockchain
        return new List<Transaction>();
    }

    private string GenerateRandomAddress()
    {
        var random = new Random();
        var bytes = new byte[20];
        random.NextBytes(bytes);
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}

public class SmartContract
{
    public string Address { get; set; } = string.Empty;
    public Guid DeployerId { get; set; }
    public Guid CommissionRuleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ContractState State { get; set; }
}

public class CommissionContractDto
{
    public Guid CommissionRuleId { get; set; }
    public Guid CreatedBy { get; set; }
    public decimal MaxCommissionAmount { get; set; }
    public int LockPeriodDays { get; set; }
}

public enum ContractState
{
    Active,
    Paused,
    Completed,
    Cancelled
}
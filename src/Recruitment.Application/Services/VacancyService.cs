using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Application.Services;

public class VacancyService : IVacancyService
{
    private readonly ApplicationDbContext _context;

    public VacancyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vacancy>> GetVacanciesAsync(Guid userId, VacancyFilterDto filter)
    {
        var query = _context.Vacancies
            .Include(v => v.Company)
            .Include(v => v.CommissionRule)
            .AsQueryable();

        // Apply filters
        if (filter.Status != null)
            query = query.Where(v => v.Status == filter.Status);

        if (filter.Province != null)
            query = query.Where(v => v.Province == filter.Province);

        if (filter.StartDateFrom != null)
            query = query.Where(v => v.StartDate >= filter.StartDateFrom);

        if (filter.StartDateTo != null)
            query = query.Where(v => v.StartDate <= filter.StartDateTo);

        return await query.OrderByDescending(v => v.CreatedAt).ToListAsync();
    }

    public async Task<Vacancy> GetVacancyByIdAsync(Guid id, Guid userId)
    {
        return await _context.Vacancies
            .Include(v => v.Company)
            .Include(v => v.CommissionRule)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Vacancy> CreateVacancyAsync(CreateVacancyDto dto, Guid userId)
    {
        var vacancy = new Vacancy
        {
            CompanyId = dto.CompanyId,
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            Province = dto.Province,
            District = dto.District,
            Quantity = dto.Quantity,
            SalaryMin = dto.SalaryMin,
            SalaryMax = dto.SalaryMax,
            SalaryType = dto.SalaryType,
            RequiredSkills = dto.RequiredSkills,
            ExperienceYears = dto.ExperienceYears,
            EducationLevel = dto.EducationLevel,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ApplicationDeadline = dto.ApplicationDeadline,
            CommissionRuleId = dto.CommissionRuleId,
            EscrowAmount = dto.EscrowAmount,
            PaymentPolicy = dto.PaymentPolicy,
            Status = "draft",
            CreatedBy = userId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Vacancies.Add(vacancy);
        await _context.SaveChangesAsync();

        return vacancy;
    }

    public async Task<Vacancy> UpdateVacancyAsync(Guid id, UpdateVacancyDto dto, Guid userId)
    {
        var vacancy = await _context.Vacancies.FindAsync(id);
        if (vacancy == null)
            throw new KeyNotFoundException("Vacancy not found");

        vacancy.Title = dto.Title;
        vacancy.Description = dto.Description;
        vacancy.Location = dto.Location;
        vacancy.Province = dto.Province;
        vacancy.District = dto.District;
        vacancy.Quantity = dto.Quantity;
        vacancy.SalaryMin = dto.SalaryMin;
        vacancy.SalaryMax = dto.SalaryMax;
        vacancy.SalaryType = dto.SalaryType;
        vacancy.RequiredSkills = dto.RequiredSkills;
        vacancy.ExperienceYears = dto.ExperienceYears;
        vacancy.EducationLevel = dto.EducationLevel;
        vacancy.StartDate = dto.StartDate;
        vacancy.EndDate = dto.EndDate;
        vacancy.ApplicationDeadline = dto.ApplicationDeadline;
        vacancy.CommissionRuleId = dto.CommissionRuleId;
        vacancy.EscrowAmount = dto.EscrowAmount;
        vacancy.PaymentPolicy = dto.PaymentPolicy;
        vacancy.Status = dto.Status;
        vacancy.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return vacancy;
    }

    public async Task DeleteVacancyAsync(Guid id, Guid userId)
    {
        var vacancy = await _context.Vacancies.FindAsync(id);
        if (vacancy == null)
            throw new KeyNotFoundException("Vacancy not found");

        _context.Vacancies.Remove(vacancy);
        await _context.SaveChangesAsync();
    }

    public async Task<VacancyStatistics> GetVacancyStatisticsAsync(Guid id, Guid userId)
    {
        var vacancy = await _context.Vacancies.FindAsync(id);
        if (vacancy == null)
            throw new KeyNotFoundException("Vacancy not found");

        var applications = await _context.Applications
            .Where(a => a.VacancyId == id)
            .ToListAsync();

        return new VacancyStatistics
        {
            TotalApplications = applications.Count,
            Shortlisted = applications.Count(a => a.Status == "shortlisted"),
            Interviewed = applications.Count(a => a.Status == "interviewed"),
            Hired = applications.Count(a => a.Status == "contract_signed")
        };
    }

    public async Task<Application> SubmitApplicationToVacancyAsync(Guid vacancyId, CreateApplicationDto dto, Guid userId)
    {
        var vacancy = await _context.Vacancies.FindAsync(vacancyId);
        if (vacancy == null)
            throw new KeyNotFoundException("Vacancy not found");

        var candidate = await _context.Candidates.FindAsync(dto.CandidateId);
        if (candidate == null)
            throw new KeyNotFoundException("Candidate not found");

        var application = new Application
        {
            VacancyId = vacancyId,
            CandidateId = dto.CandidateId,
            RecruiterId = userId,
            Status = "applied",
            AppliedAt = DateTimeOffset.UtcNow,
            LastUpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return application;
    }
}
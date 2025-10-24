using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;

    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Application>> GetApplicationsAsync(Guid userId, ApplicationFilterDto filter)
    {
        var query = _context.Applications
            .Include(a => a.Vacancy)
            .Include(a => a.Candidate)
            .Include(a => a.Recruiter)
            .AsQueryable();

        // Apply filters
        if (filter.VacancyId != null)
            query = query.Where(a => a.VacancyId == filter.VacancyId);

        if (filter.Status != null)
            query = query.Where(a => a.Status == filter.Status);

        if (filter.RecruiterId != null)
            query = query.Where(a => a.RecruiterId == filter.RecruiterId);

        return await query.OrderByDescending(a => a.AppliedAt).ToListAsync();
    }

    public async Task<Application> GetApplicationByIdAsync(Guid id, Guid userId)
    {
        return await _context.Applications
            .Include(a => a.Vacancy)
            .Include(a => a.Candidate)
            .Include(a => a.Recruiter)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Application> SubmitApplicationAsync(CreateApplicationDto dto, Guid userId)
    {
        var vacancy = await _context.Vacancies.FindAsync(dto.VacancyId);
        if (vacancy == null)
            throw new KeyNotFoundException("Vacancy not found");

        var candidate = await _context.Candidates.FindAsync(dto.CandidateId);
        if (candidate == null)
            throw new KeyNotFoundException("Candidate not found");

        var application = new Application
        {
            VacancyId = dto.VacancyId,
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

    public async Task<Application> UpdateApplicationStatusAsync(Guid id, UpdateApplicationStatusDto dto, Guid userId)
    {
        var application = await _context.Applications.FindAsync(id);
        if (application == null)
            throw new KeyNotFoundException("Application not found");

        application.Status = dto.Status;
        application.UpdatedBy = userId;
        application.LastUpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return application;
    }

    public async Task<Interview> ScheduleInterviewAsync(Guid applicationId, CreateInterviewDto dto, Guid userId)
    {
        var application = await _context.Applications.FindAsync(applicationId);
        if (application == null)
            throw new KeyNotFoundException("Application not found");

        var interview = new Interview
        {
            ApplicationId = applicationId,
            InterviewerId = userId,
            Stage = dto.Stage,
            ScheduledAt = dto.ScheduledAt,
            Location = dto.Location,
            MeetingLink = dto.MeetingLink,
            Result = "pending",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Interviews.Add(interview);
        await _context.SaveChangesAsync();

        return interview;
    }

    public async Task<Interview> UpdateInterviewResultAsync(Guid applicationId, Guid interviewId, UpdateInterviewDto dto, Guid userId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        interview.Result = dto.Result;
        interview.Score = dto.Score;
        interview.Notes = dto.Notes;
        interview.Feedback = dto.Feedback;
        interview.NextSteps = dto.NextSteps;
        interview.NextInterviewDate = dto.NextInterviewDate;
        interview.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return interview;
    }

    public async Task<string> UploadDocumentsAsync(Guid applicationId, UploadDocumentsDto dto, Guid userId)
    {
        var application = await _context.Applications.FindAsync(applicationId);
        if (application == null)
            throw new KeyNotFoundException("Application not found");

        // Handle file uploads here
        // For now, return a placeholder
        return "Documents uploaded successfully";
    }

    public async Task<string> GetApplicationTimelineAsync(Guid id, Guid userId)
    {
        var application = await _context.Applications
            .Include(a => a.Interviews)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application == null)
            throw new KeyNotFoundException("Application not found");

        // Generate timeline based on application and interviews
        return "Timeline generated";
    }

    public async Task<Note> AddNoteAsync(Guid applicationId, CreateNoteDto dto, Guid userId)
    {
        var application = await _context.Applications.FindAsync(applicationId);
        if (application == null)
            throw new KeyNotFoundException("Application not found");

        var note = new Note
        {
            ApplicationId = applicationId,
            Content = dto.Content,
            CreatedBy = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return note;
    }

    public async Task<ApplicationStatistics> GetApplicationStatisticsAsync(Guid userId)
    {
        var applications = await _context.Applications
            .Where(a => a.RecruiterId == userId)
            .ToListAsync();

        return new ApplicationStatistics
        {
            TotalApplications = applications.Count,
            Shortlisted = applications.Count(a => a.Status == "shortlisted"),
            Interviewed = applications.Count(a => a.Status == "interviewed"),
            Hired = applications.Count(a => a.Status == "contract_signed")
        };
    }

    public async Task<List<Application>> BulkUpdateStatusAsync(BulkStatusUpdateDto dto, Guid userId)
    {
        var applications = await _context.Applications
            .Where(a => dto.ApplicationIds.Contains(a.Id))
            .ToListAsync();

        foreach (var application in applications)
        {
            application.Status = dto.Status;
            application.UpdatedBy = userId;
            application.LastUpdatedAt = DateTimeOffset.UtcNow;
        }

        await _context.SaveChangesAsync();
        return applications;
    }

    public async Task<List<Interview>> GetApplicationInterviewsAsync(Guid applicationId, Guid userId)
    {
        return await _context.Interviews
            .Where(i => i.ApplicationId == applicationId)
            .OrderBy(i => i.ScheduledAt)
            .ToListAsync();
    }
}
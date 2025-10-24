using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces;
using Recruitment.Application.DTOs;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Application.Services;

public class InterviewService : IInterviewService
{
    private readonly ApplicationDbContext _context;

    public InterviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Interview>> GetInterviewsAsync(Guid userId, InterviewFilterDto filter)
    {
        var query = _context.Interviews
            .Include(i => i.Application)
            .ThenInclude(a => a.Vacancy)
            .Include(i => i.Interviewer)
            .AsQueryable();

        if (filter.ApplicationId != null)
            query = query.Where(i => i.ApplicationId == filter.ApplicationId);

        if (filter.InterviewerId != null)
            query = query.Where(i => i.InterviewerId == filter.InterviewerId);

        if (filter.Status != null)
            query = query.Where(i => i.Result == filter.Status);

        return await query.OrderBy(i => i.ScheduledAt).ToListAsync();
    }

    public async Task<Interview> GetInterviewByIdAsync(Guid id, Guid userId)
    {
        return await _context.Interviews
            .Include(i => i.Application)
            .Include(i => i.Interviewer)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Interview> ScheduleInterviewAsync(CreateInterviewDto dto, Guid userId)
    {
        var application = await _context.Applications.FindAsync(dto.ApplicationId);
        if (application == null)
            throw new KeyNotFoundException("Application not found");

        var interview = new Interview
        {
            ApplicationId = dto.ApplicationId,
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

    public async Task<Interview> UpdateInterviewAsync(Guid id, UpdateInterviewDto dto, Guid userId)
    {
        var interview = await _context.Interviews.FindAsync(id);
        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        interview.Stage = dto.Stage ?? interview.Stage;
        interview.ScheduledAt = dto.ScheduledAt ?? interview.ScheduledAt;
        interview.Location = dto.Location ?? interview.Location;
        interview.MeetingLink = dto.MeetingLink ?? interview.MeetingLink;
        interview.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return interview;
    }

    public async Task<Interview> CancelInterviewAsync(Guid id, Guid userId)
    {
        var interview = await _context.Interviews.FindAsync(id);
        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        interview.Result = "cancelled";
        interview.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return interview;
    }

    public async Task<Interview> CompleteInterviewAsync(Guid id, CompleteInterviewDto dto, Guid userId)
    {
        var interview = await _context.Interviews.FindAsync(id);
        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        interview.Result = dto.Result;
        interview.Score = dto.Score;
        interview.Notes = dto.Notes;
        interview.Feedback = dto.Feedback;
        interview.ActualEndAt = DateTimeOffset.UtcNow;
        interview.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return interview;
    }

    public async Task<Note> AddInterviewNoteAsync(Guid interviewId, CreateNoteDto dto, Guid userId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        var note = new Note
        {
            ApplicationId = interview.ApplicationId,
            Content = dto.Content,
            CreatedBy = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return note;
    }

    public async Task<string> UploadRecordingAsync(Guid interviewId, UploadRecordingDto dto, Guid userId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        // Handle recording upload
        return "Recording uploaded successfully";
    }

    public async Task<List<string>> GetInterviewRecordingsAsync(Guid interviewId, Guid userId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        // Return list of recording paths
        return new List<string>();
    }

    public async Task<string> GenerateInterviewReportAsync(Guid interviewId, Guid userId)
    {
        var interview = await _context.Interviews
            .Include(i => i.Application)
            .Include(i => i.Interviewer)
            .FirstOrDefaultAsync(i => i.Id == interviewId);

        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        // Generate report
        return "Interview report generated";
    }

    public async Task<InterviewStatistics> GetInterviewStatisticsAsync(Guid userId)
    {
        var interviews = await _context.Interviews
            .Where(i => i.InterviewerId == userId)
            .ToListAsync();

        return new InterviewStatistics
        {
            TotalInterviews = interviews.Count,
            Completed = interviews.Count(i => i.Result == "pass" || i.Result == "fail"),
            Passed = interviews.Count(i => i.Result == "pass"),
            Failed = interviews.Count(i => i.Result == "fail")
        };
    }

    public async Task<List<Interview>> BulkScheduleInterviewsAsync(BulkScheduleDto dto, Guid userId)
    {
        var interviews = new List<Interview>();

        foreach (var applicationId in dto.ApplicationIds)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application != null)
            {
                var interview = new Interview
                {
                    ApplicationId = applicationId,
                    InterviewerId = userId,
                    Stage = "initial",
                    ScheduledAt = dto.ScheduledAt,
                    Location = dto.Location,
                    Result = "pending",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                _context.Interviews.Add(interview);
                interviews.Add(interview);
            }
        }

        await _context.SaveChangesAsync();
        return interviews;
    }

    public async Task<List<TimeSlot>> GetInterviewerAvailabilityAsync(Guid interviewerId, DateTime date)
    {
        // Get existing interviews for the day
        var existingInterviews = await _context.Interviews
            .Where(i => i.InterviewerId == interviewerId && i.ScheduledAt.Date == date.Date)
            .OrderBy(i => i.ScheduledAt)
            .ToListAsync();

        // Generate time slots (9 AM to 5 PM, 1-hour slots)
        var timeSlots = new List<TimeSlot>();
        var startTime = new TimeSpan(9, 0, 0);
        var endTime = new TimeSpan(17, 0, 0);

        for (var time = startTime; time < endTime; time = time.Add(TimeSpan.FromHours(1)))
        {
            var slotStart = date.Date.Add(time);
            var slotEnd = slotStart.Add(TimeSpan.FromHours(1));

            var isAvailable = !existingInterviews.Any(i =>
                i.ScheduledAt >= slotStart && i.ScheduledAt < slotEnd);

            timeSlots.Add(new TimeSlot
            {
                StartTime = slotStart,
                EndTime = slotEnd,
                IsAvailable = isAvailable
            });
        }

        return timeSlots;
    }

    public async Task<Interview> RescheduleInterviewAsync(Guid id, RescheduleInterviewDto dto, Guid userId)
    {
        var interview = await _context.Interviews.FindAsync(id);
        if (interview == null)
            throw new KeyNotFoundException("Interview not found");

        interview.ScheduledAt = dto.NewScheduledAt;
        interview.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return interview;
    }

    public async Task<List<InterviewTemplate>> GetInterviewTemplatesAsync()
    {
        // Return predefined templates
        return new List<InterviewTemplate>
        {
            new InterviewTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Technical Interview",
                Description = "Standard technical interview template",
                Questions = new List<string>
                {
                    "Tell me about your experience with [technology]",
                    "How would you solve [problem]",
                    "What are your strengths and weaknesses?"
                }
            }
        };
    }

    public async Task<InterviewTemplate> CreateInterviewTemplateAsync(CreateInterviewTemplateDto dto, Guid userId)
    {
        var template = new InterviewTemplate
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Questions = dto.Questions
        };

        // In a real implementation, save to database
        return template;
    }
}
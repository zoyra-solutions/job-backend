using Recruitment.Application.DTOs;

namespace Recruitment.Application.Interfaces;

public interface IInterviewService
{
    Task<IEnumerable<Interview>> GetInterviewsAsync(Guid userId, InterviewFilterDto filter);
    Task<Interview> GetInterviewByIdAsync(Guid id, Guid userId);
    Task<Interview> ScheduleInterviewAsync(CreateInterviewDto dto, Guid userId);
    Task<Interview> UpdateInterviewAsync(Guid id, UpdateInterviewDto dto, Guid userId);
    Task<Interview> CancelInterviewAsync(Guid id, Guid userId);
    Task<Interview> CompleteInterviewAsync(Guid id, CompleteInterviewDto dto, Guid userId);
    Task<Note> AddInterviewNoteAsync(Guid interviewId, CreateNoteDto dto, Guid userId);
    Task<string> UploadRecordingAsync(Guid interviewId, UploadRecordingDto dto, Guid userId);
    Task<List<string>> GetInterviewRecordingsAsync(Guid interviewId, Guid userId);
    Task<string> GenerateInterviewReportAsync(Guid interviewId, Guid userId);
    Task<InterviewStatistics> GetInterviewStatisticsAsync(Guid userId);
    Task<List<Interview>> BulkScheduleInterviewsAsync(BulkScheduleDto dto, Guid userId);
    Task<List<TimeSlot>> GetInterviewerAvailabilityAsync(Guid interviewerId, DateTime date);
    Task<Interview> RescheduleInterviewAsync(Guid id, RescheduleInterviewDto dto, Guid userId);
    Task<List<InterviewTemplate>> GetInterviewTemplatesAsync();
    Task<InterviewTemplate> CreateInterviewTemplateAsync(CreateInterviewTemplateDto dto, Guid userId);
}

public class InterviewFilterDto
{
    public Guid? ApplicationId { get; set; }
    public string Status { get; set; }
    public Guid? InterviewerId { get; set; }
}

public class CompleteInterviewDto
{
    public string Result { get; set; }
    public int Score { get; set; }
    public string Notes { get; set; }
    public string Feedback { get; set; }
}

public class UploadRecordingDto
{
    public IFormFile RecordingFile { get; set; }
}

public class InterviewStatistics
{
    public int TotalInterviews { get; set; }
    public int Completed { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
}

public class BulkScheduleDto
{
    public List<Guid> ApplicationIds { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public string Location { get; set; }
}

public class TimeSlot
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
}

public class RescheduleInterviewDto
{
    public DateTimeOffset NewScheduledAt { get; set; }
    public string Reason { get; set; }
}

public class InterviewTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Questions { get; set; }
}

public class CreateInterviewTemplateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Questions { get; set; }
}
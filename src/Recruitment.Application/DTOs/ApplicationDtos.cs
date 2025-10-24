namespace Recruitment.Application.DTOs;

public class ApplicationFilterDto
{
    public Guid? VacancyId { get; set; }
    public string Status { get; set; }
    public Guid? RecruiterId { get; set; }
}

public class CreateApplicationDto
{
    public Guid VacancyId { get; set; }
    public Guid CandidateId { get; set; }
}

public class UpdateApplicationStatusDto
{
    public string Status { get; set; }
}

public class CreateInterviewDto
{
    public string Stage { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public string Location { get; set; }
    public string MeetingLink { get; set; }
}

public class UpdateInterviewDto
{
    public string Result { get; set; }
    public int? Score { get; set; }
    public string Notes { get; set; }
    public string Feedback { get; set; }
    public string NextSteps { get; set; }
    public DateTimeOffset? NextInterviewDate { get; set; }
}

public class UploadDocumentsDto
{
    public IFormFile CVFile { get; set; }
    public IFormFile PhotoFile { get; set; }
    public IFormFile IDCardFile { get; set; }
    public IFormFile VideoFile { get; set; }
}

public class CreateNoteDto
{
    public string Content { get; set; }
}

public class BulkStatusUpdateDto
{
    public List<Guid> ApplicationIds { get; set; }
    public string Status { get; set; }
}

public class ApplicationStatistics
{
    public int TotalApplications { get; set; }
    public int Shortlisted { get; set; }
    public int Interviewed { get; set; }
    public int Hired { get; set; }
}

public class Note
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Content { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
using Recruitment.Application.DTOs;

namespace Recruitment.Application.Interfaces;

public interface IApplicationService
{
    Task<IEnumerable<Application>> GetApplicationsAsync(Guid userId, ApplicationFilterDto filter);
    Task<Application> GetApplicationByIdAsync(Guid id, Guid userId);
    Task<Application> SubmitApplicationAsync(CreateApplicationDto dto, Guid userId);
    Task<Application> UpdateApplicationStatusAsync(Guid id, UpdateApplicationStatusDto dto, Guid userId);
    Task<Interview> ScheduleInterviewAsync(Guid applicationId, CreateInterviewDto dto, Guid userId);
    Task<Interview> UpdateInterviewResultAsync(Guid applicationId, Guid interviewId, UpdateInterviewDto dto, Guid userId);
    Task<string> UploadDocumentsAsync(Guid applicationId, UploadDocumentsDto dto, Guid userId);
    Task<string> GetApplicationTimelineAsync(Guid id, Guid userId);
    Task<Note> AddNoteAsync(Guid applicationId, CreateNoteDto dto, Guid userId);
    Task<ApplicationStatistics> GetApplicationStatisticsAsync(Guid userId);
    Task<List<Application>> BulkUpdateStatusAsync(BulkStatusUpdateDto dto, Guid userId);
    Task<List<Interview>> GetApplicationInterviewsAsync(Guid applicationId, Guid userId);
}
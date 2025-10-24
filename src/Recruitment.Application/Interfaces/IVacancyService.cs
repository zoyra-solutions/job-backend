using Recruitment.Application.DTOs;

namespace Recruitment.Application.Interfaces;

public interface IVacancyService
{
    Task<IEnumerable<VacancyDto>> GetVacanciesAsync(Guid userId, VacancyFilterDto filter);
    Task<VacancyDto?> GetVacancyByIdAsync(Guid vacancyId, Guid userId);
    Task<VacancyDto> CreateVacancyAsync(CreateVacancyDto dto, Guid userId);
    Task<VacancyDto> UpdateVacancyAsync(Guid vacancyId, UpdateVacancyDto dto, Guid userId);
    Task DeleteVacancyAsync(Guid vacancyId, Guid userId);
    Task<VacancyStatisticsDto> GetVacancyStatisticsAsync(Guid vacancyId, Guid userId);
}
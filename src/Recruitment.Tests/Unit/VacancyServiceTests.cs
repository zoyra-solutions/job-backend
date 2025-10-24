using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;
using Recruitment.Domain.Entities;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Tests.Unit;

public class VacancyServiceTests
{
    private readonly Mock<IRepository<Vacancy>> _vacancyRepositoryMock;
    private readonly Mock<IRepository<Company>> _companyRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly VacancyService _vacancyService;

    public VacancyServiceTests()
    {
        _vacancyRepositoryMock = new Mock<IRepository<Vacancy>>();
        _companyRepositoryMock = new Mock<IRepository<Company>>();
        _userContextMock = new Mock<IUserContext>();

        _vacancyService = new VacancyService(
            _vacancyRepositoryMock.Object,
            _companyRepositoryMock.Object,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task CreateVacancy_ValidDto_ReturnsVacancyDto()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _userContextMock.Setup(x => x.GetCurrentUserId()).Returns(userId);

        var company = new Company
        {
            Id = companyId,
            Name = "Test Company",
            AdminUserId = userId
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(companyId))
            .ReturnsAsync(company);

        var createDto = new CreateVacancyDto
        {
            Title = "Software Engineer",
            Description = "Great job opportunity",
            Location = "Ho Chi Minh City",
            Quantity = 5,
            SalaryMin = 1000,
            SalaryMax = 2000,
            StartDate = DateTime.UtcNow.AddDays(30),
            ApplicationDeadline = DateTime.UtcNow.AddDays(15),
            CommissionRuleId = Guid.NewGuid()
        };

        _vacancyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Vacancy>()))
            .ReturnsAsync((Vacancy v) => v);

        // Act
        var result = await _vacancyService.CreateVacancyAsync(createDto, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.Title, result.Title);
        Assert.Equal(createDto.Description, result.Description);
        Assert.Equal(userId, result.CreatedBy);

        _vacancyRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Vacancy>()), Times.Once);
    }

    [Fact]
    public async Task CreateVacancy_UserNotCompanyAdmin_ThrowsUnauthorizedException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _userContextMock.Setup(x => x.GetCurrentUserId()).Returns(userId);

        var company = new Company
        {
            Id = companyId,
            Name = "Test Company",
            AdminUserId = Guid.NewGuid() // Different admin user
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(companyId))
            .ReturnsAsync(company);

        var createDto = new CreateVacancyDto
        {
            Title = "Software Engineer",
            Description = "Great job opportunity",
            Location = "Ho Chi Minh City",
            Quantity = 5,
            SalaryMin = 1000,
            SalaryMax = 2000,
            StartDate = DateTime.UtcNow.AddDays(30),
            ApplicationDeadline = DateTime.UtcNow.AddDays(15),
            CommissionRuleId = Guid.NewGuid()
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _vacancyService.CreateVacancyAsync(createDto, userId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateVacancy_InvalidQuantity_ThrowsValidationException(int quantity)
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userContextMock.Setup(x => x.GetCurrentUserId()).Returns(userId);

        var createDto = new CreateVacancyDto
        {
            Title = "Software Engineer",
            Description = "Great job opportunity",
            Location = "Ho Chi Minh City",
            Quantity = quantity,
            SalaryMin = 1000,
            SalaryMax = 2000,
            StartDate = DateTime.UtcNow.AddDays(30),
            ApplicationDeadline = DateTime.UtcNow.AddDays(15),
            CommissionRuleId = Guid.NewGuid()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _vacancyService.CreateVacancyAsync(createDto, userId));
    }

    [Fact]
    public async Task GetVacancies_WithFilter_ReturnsFilteredResults()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();

        _userContextMock.Setup(x => x.GetCurrentUserId()).Returns(userId);

        var vacancies = new List<Vacancy>
        {
            new Vacancy
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer",
                CompanyId = companyId,
                Status = "published",
                CreatedBy = userId
            },
            new Vacancy
            {
                Id = Guid.NewGuid(),
                Title = "Designer",
                CompanyId = companyId,
                Status = "draft",
                CreatedBy = userId
            }
        };

        _vacancyRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(vacancies.AsQueryable());

        var filter = new VacancyFilterDto
        {
            Status = "published",
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _vacancyService.GetVacanciesAsync(userId, filter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Software Engineer", result.First().Title);
        Assert.Equal("published", result.First().Status);
    }

    [Fact]
    public async Task UpdateVacancy_ValidDto_UpdatesVacancy()
    {
        // Arrange
        var vacancyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var existingVacancy = new Vacancy
        {
            Id = vacancyId,
            Title = "Old Title",
            CompanyId = Guid.NewGuid(),
            CreatedBy = userId
        };

        _vacancyRepositoryMock
            .Setup(x => x.GetByIdAsync(vacancyId))
            .ReturnsAsync(existingVacancy);

        var updateDto = new UpdateVacancyDto
        {
            Title = "New Title",
            Description = "Updated description"
        };

        _vacancyRepositoryMock
            .Setup(x => x.UpdateAsync(existingVacancy))
            .ReturnsAsync(existingVacancy);

        // Act
        var result = await _vacancyService.UpdateVacancyAsync(vacancyId, updateDto, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Title", result.Title);
        _vacancyRepositoryMock.Verify(x => x.UpdateAsync(existingVacancy), Times.Once);
    }

    [Fact]
    public async Task DeleteVacancy_ExistingVacancy_DeletesVacancy()
    {
        // Arrange
        var vacancyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var existingVacancy = new Vacancy
        {
            Id = vacancyId,
            Title = "Test Vacancy",
            CompanyId = Guid.NewGuid(),
            CreatedBy = userId
        };

        _vacancyRepositoryMock
            .Setup(x => x.GetByIdAsync(vacancyId))
            .ReturnsAsync(existingVacancy);

        // Act
        await _vacancyService.DeleteVacancyAsync(vacancyId, userId);

        // Assert
        _vacancyRepositoryMock.Verify(x => x.DeleteAsync(existingVacancy), Times.Once);
    }
}
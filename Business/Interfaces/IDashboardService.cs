using Business.Dtos;

namespace Business.Interfaces;

public interface IDashboardService
{ 
    Task<DashboardDto> GetDashboardAsync(string userId);
}
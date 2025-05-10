using SCIC_BE.DTO.HistoryDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IHistoryService
    {
        Task<List<HistoryModel>> GetListHistoryAsync();
        Task<HistoryModel> GetHistoryByIdAsync(Guid id);
        Task<List<HistoryModel>> GetHistoryByUserIdAsync(Guid userId);
        Task<List<HistoryModel>> GetHistoryByDeviceIdAsync(Guid deviceId);
        Task<List<HistoryModel>> GetHistoryByUsersAndDevicesAsync(Guid[] usersIds, Guid[] deviceIds, DateTime startTime, DateTime endTime);
        Task CreateHistoryAsync(CreateHistoryDTO history);
        Task UpdateHistoryAsync(HistoryModel history);
        Task DeleteHistoryAsync(Guid id);
    }
}

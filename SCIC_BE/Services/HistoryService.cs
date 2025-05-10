using DocumentFormat.OpenXml.Spreadsheet;
using SCIC_BE.DTO.HistoryDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.HistoryRepository;

namespace SCIC_BE.Services
{
    public class HistoryService
    {
        private readonly IHistoryRepository _historyRepository;

        public HistoryService(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }
        public async Task<List<HistoryModel>> GetListHistoryAsync()
        {
            var histories = await _historyRepository.GetAllHistoryAsync();

            return histories;
        }
        public async Task<HistoryModel> GetHistoryByIdAsync(Guid id)
        {
            var history = await _historyRepository.GetHistoryAsync(id);

            return history;
        }
        public async Task<List<HistoryModel>> GetHistoryByUserIdAsync(Guid userId)
        {
            var histories = await GetListHistoryAsync();
           
            var historiesForUser = histories.Where(hist => hist.UserId == userId).ToList();

            return historiesForUser;
        }
        public async Task<List<HistoryModel>> GetHistoryByDeviceIdAsync(Guid deviceId)
        {
            var histories = await GetListHistoryAsync();

            var historiesForDevice = histories.Where(hist => hist.DeviceId == deviceId).ToList();
            return historiesForDevice;
        }
        public async Task<List<HistoryModel>> GetHistoryByUsersAndDevicesAsync( Guid[] usersIds, 
                                                                                Guid[] deviceIds, 
                                                                                DateTime startTime, 
                                                                                DateTime endTime)
        {
            var histories = await GetListHistoryAsync();

            var history = histories.Where(hist =>   usersIds.Contains(hist.UserId) &&
                                                    deviceIds.Contains(hist.UserId) &&
                                                    hist.Timestamp >= startTime &&
                                                    hist.Timestamp <= endTime
                                                    ).ToList();

            return history;
        }

        public async Task CreateHistoryAsync(CreateHistoryDTO history)
        {
            var historyInfo = new HistoryModel()
            {
                Id = new Guid(),
                UserId = history.UserId,
                DeviceId = history.DeviceId,
                Type = history.Type,
                Status = history.Status,
                Timestamp = history.Timestamp,
            };

            await _historyRepository.AddAsync(historyInfo);

        }
    }
}

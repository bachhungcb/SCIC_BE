using SCIC_BE.Models;

namespace SCIC_BE.Repositories.HistoryRepository
{
    public interface IHistoryRepository
    {
        Task<HistoryModel> GetHistoryAsync(Guid id);
        Task<List<HistoryModel>> GetAllHistoryAsync();
        Task AddAsync(HistoryModel history);
        Task UpdateAsync (HistoryModel history);
        Task DeleteAsync (Guid id);
    }
}

using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.HistoryRepository
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly ScicDbContext _context;

        public HistoryRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<HistoryModel> GetHistoryAsync(Guid id)
        {
            var history = await _context.Histories.FindAsync(id);

            return history;
        }

        public async Task<List<HistoryModel>> GetAllHistoryAsync()
        {
            var histories = await _context.Histories.ToListAsync();
            return histories;
        }

        public async Task AddAsync(HistoryModel history)
        {
            _context.Histories.Add(history);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(HistoryModel history)
        {
            var existingHistory = await _context.Histories.FirstOrDefaultAsync(h => h.Id == history.Id);

            if (existingHistory == null)
            {
                throw new KeyNotFoundException("History not found to update");
            }

            _context.Histories.Update(existingHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingHistory = await _context.Histories.FirstOrDefaultAsync(h => h.Id == id);

            if (existingHistory == null)
            {
                throw new KeyNotFoundException("History not found to update");
            }
            
            _context.Histories.Remove(existingHistory);
            await _context.SaveChangesAsync();
        }
    }
}

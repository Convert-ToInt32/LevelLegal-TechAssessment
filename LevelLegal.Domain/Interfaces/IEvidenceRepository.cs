using LevelLegal.Domain.Entities;

namespace LevelLegal.Domain.Interfaces
{
    public interface IEvidenceRepository
    {
        Task AddAsync(Evidence evidence);
        Task<bool> ExistsAsync(int evidenceId);
        Task<List<Evidence>> GetAllAsync();
        Task<Evidence?> GetByIdAsync(int id);
    }
}

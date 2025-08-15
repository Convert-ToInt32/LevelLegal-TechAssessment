using LevelLegal.Domain.Entities;

namespace LevelLegal.Domain.Interfaces
{
    public interface IMatterRepository
    {
        Task AddAsync(Matter matter);
        Task<bool> ExistsAsync(int matterId);
    }
}

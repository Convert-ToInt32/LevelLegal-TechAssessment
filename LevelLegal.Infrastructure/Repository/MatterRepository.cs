using LevelLegal.Domain.Entities;
using LevelLegal.Domain.Interfaces;
using LevelLegal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LevelLegal.Infrastructure.Repository
{
    public class MatterRepository(AppDbContext context) : IMatterRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(Matter matter)
        {
            await _context.Database.OpenConnectionAsync();
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Matters ON");

            _context.Matters.Add(matter);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Matters OFF");
            await _context.Database.CloseConnectionAsync();

        }

        public async Task<bool> ExistsAsync(int matterId)
        {
            return await _context.Matters.AnyAsync(m => m.Id == matterId);
        }
    }
}

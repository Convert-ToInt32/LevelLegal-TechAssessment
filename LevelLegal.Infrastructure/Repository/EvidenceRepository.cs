using LevelLegal.Domain.Entities;
using LevelLegal.Domain.Interfaces;
using LevelLegal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LevelLegal.Infrastructure.Repository
{
    public class EvidenceRepository(AppDbContext context) : IEvidenceRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(Evidence evidence)
        {
            await _context.Database.OpenConnectionAsync();
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Evidence ON;");

            _context.Evidence.Add(evidence);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Evidence OFF;");
            await _context.Database.CloseConnectionAsync();
        }


        public async Task<bool> ExistsAsync(int evidenceId)
        {
            return await _context.Evidence.AnyAsync(e => e.Id == evidenceId);
        }
    }
}

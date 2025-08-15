using CsvHelper;
using LevelLegal.Domain.Entities;
using LevelLegal.Domain.Interfaces;
using System.Globalization;
using static LevelLegal.Application.CommandHandler.ImportCsvCommandHandler;

namespace LevelLegal.Application.CommandHandler
{
    public class ImportCsvCommandHandler(
        IMatterRepository matterRepo,
        IEvidenceRepository evidenceRepo
    ): ICsvImporter
    {
        private readonly IMatterRepository _matterRepo = matterRepo;
        private readonly IEvidenceRepository _evidenceRepo = evidenceRepo;

        public interface ICsvImporter
        {
            Task<(bool Success, string? ErrorMessage)> ImportAsync(string mattersCsv, string evidenceCsv);
        }

        public async Task<(bool Success, string? ErrorMessage)> ImportAsync(string mattersCsv, string evidenceCsv)
        {
            try
            {
                // --- Import Matters ---
                using var reader1 = new StringReader(mattersCsv);
                using var csv1 = new CsvReader(reader1, CultureInfo.InvariantCulture);
                var matterRecords = csv1.GetRecords<MatterCsvModel>()?.ToList() ?? [];

                foreach (var record in matterRecords)
                {
                    if (!await _matterRepo.ExistsAsync(record.MatterID))
                    {
                        await _matterRepo.AddAsync(new Matter
                        {
                            Id = record.MatterID,
                            Name = record.MatterName
                        });
                    }
                }

                // --- Import Evidence ---
                using var reader2 = new StringReader(evidenceCsv);
                using var csv2 = new CsvReader(reader2, CultureInfo.InvariantCulture);
                var evidenceRecords = csv2.GetRecords<EvidenceCsvModel>()?.ToList() ?? [];

                foreach (var record in evidenceRecords)
                {
                    if (!await _evidenceRepo.ExistsAsync(record.EvidenceID))
                    {
                        await _evidenceRepo.AddAsync(new Evidence
                        {
                            Id = record.EvidenceID,
                            EvidenceName = $"Serial: {record.SerialNumber}",
                            Description = record.Description,
                            MatterId = record.MatterID
                        });
                    }
                }

                return (true, null);
            }
            catch (HeaderValidationException ex)
            {
                // Special handling for CSV header issues
                Console.WriteLine($"CSV Header Error: {ex.Message}");
                return (false, $"CSV Header Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // General catch
                Console.WriteLine($"Error importing CSV: {ex}");
                return (false, $"Error importing CSV: {ex.Message}");
            }
        }

        // CSV mapping classes remain inside Application file
        public class MatterCsvModel
        {
            public int MatterID { get; set; }
            public string MatterName { get; set; } = null!;
            public string? ClientName { get; set; }
        }

        public class EvidenceCsvModel
        {
            public int EvidenceID { get; set; }
            public int MatterID { get; set; }
            public string Description { get; set; } = null!;
            public string SerialNumber { get; set; } = null!;
        }
    }
}

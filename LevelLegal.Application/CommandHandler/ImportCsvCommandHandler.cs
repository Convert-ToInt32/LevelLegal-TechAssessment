using CsvHelper;
using LevelLegal.Domain.Entities;
using LevelLegal.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;
using static LevelLegal.Application.CommandHandler.ImportCsvCommandHandler;

namespace LevelLegal.Application.CommandHandler
{
    public class ImportCsvCommandHandler(
        IMatterRepository matterRepo,
        IEvidenceRepository evidenceRepo,
        ILogger<ImportCsvCommandHandler> logger
    ) : ICsvImporter
    {
        private readonly IMatterRepository _matterRepo = matterRepo;
        private readonly IEvidenceRepository _evidenceRepo = evidenceRepo;
        private readonly ILogger<ImportCsvCommandHandler> _logger = logger;

        public interface ICsvImporter
        {
            Task<(bool Success, string? ErrorMessage)> ImportAsync(string mattersCsv, string evidenceCsv);
        }

        public async Task<(bool Success, string? ErrorMessage)> ImportAsync(string mattersCsv, string evidenceCsv)
        {
            _logger.LogInformation("Starting CSV import for Matters and Evidence.");

            try
            {
                // --- Import Matters ---
                using var reader1 = new StringReader(mattersCsv);
                using var csv1 = new CsvReader(reader1, CultureInfo.InvariantCulture);
                var matterRecords = csv1.GetRecords<MatterCsvModel>()?.ToList() ?? [];

                _logger.LogInformation("Found {Count} matter records in CSV.", matterRecords.Count);

                foreach (var record in matterRecords)
                {
                    if (!await _matterRepo.ExistsAsync(record.MatterID))
                    {
                        await _matterRepo.AddAsync(new Matter
                        {
                            Id = record.MatterID,
                            Name = record.MatterName

                        });
                        _logger.LogDebug("Inserted Matter {Id} - {Name}", record.MatterID, record.MatterName);
                    }
                    else
                    {
                        _logger.LogDebug("Skipped existing Matter {Id}", record.MatterID);
                    }
                }

                // --- Import Evidence ---
                using var reader2 = new StringReader(evidenceCsv);
                using var csv2 = new CsvReader(reader2, CultureInfo.InvariantCulture);
                var evidenceRecords = csv2.GetRecords<EvidenceCsvModel>()?.ToList() ?? [];

                _logger.LogInformation("Found {Count} evidence records in CSV.", evidenceRecords.Count);

                foreach (var record in evidenceRecords)
                {
                    if (!await _evidenceRepo.ExistsAsync(record.EvidenceID))
                    {
                        await _evidenceRepo.AddAsync(new Evidence
                        {
                            Id = record.EvidenceID,
                            EvidenceName = $"Serial: {record.SerialNumber}",
                            Description = record.Description,
                            MatterId = record.MatterID,
                            SerialNumber = record.SerialNumber
                        });
                        _logger.LogDebug("Inserted Evidence {Id} for Matter {MatterId}", record.EvidenceID, record.MatterID);
                    }
                    else
                    {
                        _logger.LogDebug("Skipped existing Evidence {Id}", record.EvidenceID);
                    }
                }

                _logger.LogInformation("CSV import completed successfully.");
                return (true, null);
            }
            catch (HeaderValidationException ex)
            {
                _logger.LogWarning(ex, "CSV Header Error during import.");
                return (false, $"CSV Header Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing CSV.");
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

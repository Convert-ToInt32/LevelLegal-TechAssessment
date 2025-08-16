using LevelLegal.Application.DTOs;
using LevelLegal.Application.Query;
using LevelLegal.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetAllEvidenceQueryHandler
    : IRequestHandler<GetAllEvidenceQuery, List<EvidenceDto>>
{
    private readonly IEvidenceRepository _repository;
    private readonly ILogger<GetAllEvidenceQueryHandler> _logger;

    public GetAllEvidenceQueryHandler(
        IEvidenceRepository repository,
        ILogger<GetAllEvidenceQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<EvidenceDto>> Handle(
    GetAllEvidenceQuery request,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllEvidenceQuery...");

        try
        {
            var evidences = await _repository.GetAllAsync();

            var result = evidences.Select(e => new EvidenceDto
            {
                Id = e.Id,
                EvidenceName = e.EvidenceName,
                Description = e.Description,
                SerialNumber = e.SerialNumber,
                MatterName = e.Matter != null ? e.Matter.Name : "No Matter"
            }).ToList();

            _logger.LogInformation("Successfully fetched {Count} evidence records.", result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching evidence");
            throw;
        }
    }

}

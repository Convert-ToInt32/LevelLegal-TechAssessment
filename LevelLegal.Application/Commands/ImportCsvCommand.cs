using MediatR;

namespace LevelLegal.Application.Commands
{
    public record ImportCsvCommand(string MattersCsv, string EvidenceCsv) : IRequest<bool>;
}

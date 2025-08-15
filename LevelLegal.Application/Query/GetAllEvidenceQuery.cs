using LevelLegal.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelLegal.Application.Query
{
    public record GetAllEvidenceQuery() : IRequest<List<EvidenceDto>>;

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelLegal.Application.DTOs
{
    public class EvidenceDto
    {
        public int Id { get; set; }
        public string? EvidenceName { get; set; }
        public string? Description { get; set; }    
        public string? SerialNumber { get; set; }    
        public string? MatterName { get; set; }
    }
}

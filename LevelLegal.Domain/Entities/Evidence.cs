using System.ComponentModel.DataAnnotations;

namespace LevelLegal.Domain.Entities
{
    public class Evidence
    {
        public int Id { get; set; }
        public string EvidenceName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;   
        public int MatterId { get; set; }
        public Matter? Matter { get; set; }
    }
}

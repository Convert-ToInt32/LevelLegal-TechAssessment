using System.ComponentModel.DataAnnotations;

namespace LevelLegal.Domain.Entities
{
    public class Matter
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        public ICollection<Evidence> EvidenceItems { get; set; } = [];
    }
}

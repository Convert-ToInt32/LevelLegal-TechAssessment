using System.ComponentModel.DataAnnotations;

namespace LevelLegal.Domain.Entities
{
    public class Evidence
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string EvidenceName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int MatterId { get; set; }

        public Matter Matter { get; set; } = null!;
    }
}

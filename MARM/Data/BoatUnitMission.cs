using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MARM.Data;

#nullable disable

[Table("BoatUnitMissions")]
public class BoatUnitMission
{
    [Column("Id", TypeName = "TEXT")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Column("MissionId", TypeName = "TEXT")]
    [Required]
    public Guid MissionId { get; set; }

    [Column("Name", TypeName = "TEXT")]
    [Required]
    public string Name { get; set; }

    [Column("Note", TypeName = "TEXT")]
    [Required]
    public string Note { get; set; }

    [NotMapped]
    public int ShotTotal { get; set; }

    [NotMapped]
    public int[] ShotCounts { get; set; } = new int[16];
}

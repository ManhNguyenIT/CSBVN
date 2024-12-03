using MARM.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MARM.Data;

#nullable disable

[Table("Missions")]
public class Mission
{
    [Column("Id", TypeName = "TEXT")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Column("NavalUnitId", TypeName = "TEXT")]
    [Required]
    public Guid NavalUnitId { get; set; }

    [Column("Name", TypeName = "TEXT")]
    [Required]
    public string Name { get; set; }

    [Column("State", TypeName = "INT")]
    public MissionState State { get; set; }

    [Column("CreateAt", TypeName = "TEXT")]
    [Required]
    public DateTime CreateAt { get; set; }

    [Column("StartAt", TypeName = "TEXT")]
    [Required]
    public DateTime StartAt { get; set; }

    [Column("ModifiedAt", TypeName = "TEXT")]
    [Required]
    public DateTime ModifiedAt { get; set; }

    [Column("Note", TypeName = "TEXT")]
    [Required]
    public string Note { get; set; }

    [NotMapped]
    public int BoatCount { get; set; }
}

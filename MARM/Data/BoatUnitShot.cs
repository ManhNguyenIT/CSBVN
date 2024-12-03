using MARM.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MARM.Data;

#nullable disable

[Table("BoatUnitShots")]
public class BoatUnitShot
{
    [Column("Id", TypeName = "TEXT")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Column("BoatUnitId", TypeName = "TEXT")]
    [Required]
    public Guid BoatUnitId { get; set; }

    [Column("Time", TypeName = "TEXT")]
    [Required]

    public DateTime Time { get; set; }

    [Column("Position", TypeName = "INT")]
    [Required]
    public int Position { get; set; }
}

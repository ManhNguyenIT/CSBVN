using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MARM.Data;

#nullable disable

[Table("NavalUnits")]
public class NavalUnit
{
    [Column("Id", TypeName = "TEXT")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Column("ParentId", TypeName = "TEXT")]
    public Guid ParentId { get; set; }

    [Column("Name", TypeName = "TEXT")]
    [Required]
    public string Name { get; set; }
}

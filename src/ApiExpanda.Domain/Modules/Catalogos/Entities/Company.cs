using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiExpanda.Domain.Modules.Catalogos.Entities;

[Index(nameof(Code), IsUnique = true, Name = "IX_Companies_Code")]
[Index(nameof(IsActive), Name = "IX_Companies_IsActive")]
public class Company
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Rfc { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreateAt { get; set; } = DateTime.Now;

    public DateTime? UpdateAt { get; set; }
}

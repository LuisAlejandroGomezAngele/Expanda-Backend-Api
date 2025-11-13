using System;
using System.ComponentModel.DataAnnotations;

namespace ApiExpanda.Domain.Modules.Catalogos.Entities;

public class Roles
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; } = true;
}
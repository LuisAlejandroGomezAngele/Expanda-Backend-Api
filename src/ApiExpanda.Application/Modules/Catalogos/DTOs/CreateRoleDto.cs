using System.ComponentModel.DataAnnotations;

namespace ApiExpanda.Application.Modules.Catalogos.DTOs;

public class CreateRoleDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(255, ErrorMessage = "El nombre no puede exceder 255 caracteres")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "La descripción no puede exceder 255 caracteres")]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

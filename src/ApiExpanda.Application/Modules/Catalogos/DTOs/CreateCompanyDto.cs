using System.ComponentModel.DataAnnotations;

namespace ApiExpanda.Application.Modules.Catalogos.DTOs;

public class CreateCompanyDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(255, ErrorMessage = "El nombre no puede exceder 255 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El código es requerido")]
    [MaxLength(255, ErrorMessage = "El código no puede exceder 255 caracteres")]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "El RFC no puede exceder 255 caracteres")]
    public string? Rfc { get; set; }

    public bool IsActive { get; set; } = true;
}

using System.ComponentModel.DataAnnotations;

namespace ApiExpanda.Application.Modules.Catalogos.DTOs;

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "El id es obligatorio.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
    public string Name { get; set; } = string.Empty;
}

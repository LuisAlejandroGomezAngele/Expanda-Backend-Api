using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ApiExpanda.Application.Modules.Catalogos.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string? ImgUrl { get; set; }

    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "El SKU es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El SKU no puede exceder los 50 caracteres.")]
    public string SKU { get; set; } = string.Empty;

    [Required(ErrorMessage = "El stock es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0.")]
    public int Stock { get; set; }

    public DateTime? CreationDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El ID de categoría es obligatorio.")]
    public int CategoryId { get; set; }
}

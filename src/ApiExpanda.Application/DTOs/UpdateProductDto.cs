using System;
using Microsoft.AspNetCore.Http;

namespace ApiExpanda.Application.DTOs;

public class UpdateProductDto {

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? ImgUrl { get; set; }

    public IFormFile? Image { get; set; }

    public string SKU { get; set; } = string.Empty;

    public int Stock { get; set; }

    public DateTime? CreationDate { get; set; } = DateTime.Now;

    public int CategoryId { get; set; }
}

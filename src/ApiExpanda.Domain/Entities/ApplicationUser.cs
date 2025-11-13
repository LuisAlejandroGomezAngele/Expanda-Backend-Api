using Microsoft.AspNetCore.Identity;

namespace ApiExpanda.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
}

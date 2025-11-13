# Guía Rápida: Desarrollo con Módulos

## 🚀 Inicio Rápido

### Crear una nueva entidad en un módulo existente

**Ejemplo: Crear entidad "Cliente" en módulo Comercial**

#### 1. Entidad (Domain Layer)
```csharp
// src/ApiExpanda.Domain/Modules/Comercial/Entities/Customer.cs
namespace ApiExpanda.Domain.Modules.Comercial.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
```

#### 2. DTOs (Application Layer)
```csharp
// src/ApiExpanda.Application/Modules/Comercial/DTOs/CustomerDto.cs
namespace ApiExpanda.Application.Modules.Comercial.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

// src/ApiExpanda.Application/Modules/Comercial/DTOs/CreateCustomerDto.cs
public class CreateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
```

#### 3. Interface del Repositorio (Application Layer)
```csharp
// src/ApiExpanda.Application/Modules/Comercial/Interfaces/ICustomerRepository.cs
using ApiExpanda.Domain.Modules.Comercial.Entities;

namespace ApiExpanda.Application.Modules.Comercial.Interfaces;

public interface ICustomerRepository
{
    ICollection<Customer> GetCustomers();
    Customer? GetCustomer(int id);
    bool CreateCustomer(Customer customer);
    bool UpdateCustomer(Customer customer);
    bool DeleteCustomer(Customer customer);
    bool CustomerExists(int id);
    bool Save();
}
```

#### 4. Interface del Servicio (Application Layer)
```csharp
// src/ApiExpanda.Application/Modules/Comercial/Services/Interfaces/ICustomerService.cs
using ApiExpanda.Application.Modules.Comercial.DTOs;

namespace ApiExpanda.Application.Modules.Comercial.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task<bool> UpdateCustomerAsync(int id, CreateCustomerDto updateCustomerDto);
    Task<bool> DeleteCustomerAsync(int id);
    Task<bool> CustomerExistsAsync(int id);
}
```

#### 5. Mapping (Application Layer)
```csharp
// src/ApiExpanda.Application/Modules/Comercial/Mappings/CustomerProfile.cs
using ApiExpanda.Application.Modules.Comercial.DTOs;
using ApiExpanda.Domain.Modules.Comercial.Entities;
using Mapster;

namespace ApiExpanda.Application.Modules.Comercial.Mappings;

public class CustomerProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, CustomerDto>();
        config.NewConfig<CreateCustomerDto, Customer>();
    }
}
```

#### 6. Repositorio (Infrastructure Layer)
```csharp
// src/ApiExpanda.Infrastructure/Modules/Comercial/Repositories/CustomerRepository.cs
using ApiExpanda.Application.Modules.Comercial.Interfaces;
using ApiExpanda.Domain.Modules.Comercial.Entities;
using ApiExpanda.Infrastructure.Data;

namespace ApiExpanda.Infrastructure.Modules.Comercial.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICollection<Customer> GetCustomers()
    {
        return _context.Set<Customer>().OrderBy(c => c.Id).ToList();
    }

    public Customer? GetCustomer(int id)
    {
        return _context.Set<Customer>().FirstOrDefault(c => c.Id == id);
    }

    public bool CreateCustomer(Customer customer)
    {
        customer.CreatedAt = DateTime.Now;
        _context.Set<Customer>().Add(customer);
        return Save();
    }

    public bool UpdateCustomer(Customer customer)
    {
        _context.Set<Customer>().Update(customer);
        return Save();
    }

    public bool DeleteCustomer(Customer customer)
    {
        _context.Set<Customer>().Remove(customer);
        return Save();
    }

    public bool CustomerExists(int id)
    {
        return _context.Set<Customer>().Any(c => c.Id == id);
    }

    public bool Save()
    {
        return _context.SaveChanges() >= 0;
    }
}
```

#### 7. Servicio (Infrastructure Layer)
```csharp
// src/ApiExpanda.Infrastructure/Modules/Comercial/Services/CustomerService.cs
using ApiExpanda.Application.Modules.Comercial.DTOs;
using ApiExpanda.Application.Modules.Comercial.Interfaces;
using ApiExpanda.Application.Modules.Comercial.Services.Interfaces;
using ApiExpanda.Domain.Modules.Comercial.Entities;
using MapsterMapper;

namespace ApiExpanda.Infrastructure.Modules.Comercial.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await Task.Run(() => _customerRepository.GetCustomers());
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await Task.Run(() => _customerRepository.GetCustomer(id));
        if (customer == null)
            return null;

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        var customer = _mapper.Map<Customer>(createCustomerDto);
        
        var created = await Task.Run(() => _customerRepository.CreateCustomer(customer));
        if (!created)
            throw new InvalidOperationException("Error al crear el cliente");

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<bool> UpdateCustomerAsync(int id, CreateCustomerDto updateCustomerDto)
    {
        var customer = _mapper.Map<Customer>(updateCustomerDto);
        customer.Id = id;

        return await Task.Run(() => _customerRepository.UpdateCustomer(customer));
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await Task.Run(() => _customerRepository.GetCustomer(id));
        if (customer == null)
            return false;

        return await Task.Run(() => _customerRepository.DeleteCustomer(customer));
    }

    public async Task<bool> CustomerExistsAsync(int id)
    {
        return await Task.Run(() => _customerRepository.CustomerExists(id));
    }
}
```

#### 8. Configuración EF Core (Infrastructure Layer)
```csharp
// src/ApiExpanda.Infrastructure/Modules/Comercial/Data/Configurations/CustomerConfiguration.cs
using ApiExpanda.Domain.Modules.Comercial.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiExpanda.Infrastructure.Modules.Comercial.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(c => c.Phone)
            .HasMaxLength(20);
            
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
```

#### 9. Registrar en ApplicationDbContext
```csharp
// src/ApiExpanda.Infrastructure/Data/ApplicationDbContext.cs
// Agregar DbSet
public DbSet<Customer> Customers { get; set; }

// En OnModelCreating
modelBuilder.ApplyConfiguration(new CustomerConfiguration());
```

#### 10. Controller (API Layer)
```csharp
// src/ApiExpanda.API/Controllers/Comercial/CustomersController.cs
using Microsoft.AspNetCore.Mvc;
using ApiExpanda.Application.Modules.Comercial.DTOs;
using ApiExpanda.Application.Modules.Comercial.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiExpanda.API.Controllers.Comercial;

[ApiController]
[Route("api/v{version:apiVersion}/Comercial/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound("El cliente no existe.");
        }
        return Ok(customer);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
    {
        if (createCustomerDto == null)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var customer = await _customerService.CreateCustomerAsync(createCustomerDto);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CreateCustomerDto updateCustomerDto)
    {
        if (!await _customerService.CustomerExistsAsync(id))
        {
            return NotFound("El cliente no existe.");
        }

        try
        {
            await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        if (!await _customerService.CustomerExistsAsync(id))
        {
            return NotFound("El cliente no existe.");
        }

        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}
```

#### 11. Registrar en Program.cs
```csharp
// src/ApiExpanda.API/Program.cs

// Repositorios
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Servicios
builder.Services.AddScoped<ICustomerService, CustomerService>();
```

#### 12. Crear Migración
```bash
cd src/ApiExpanda.Infrastructure
dotnet ef migrations add AddCustomerEntity --startup-project ../ApiExpanda.API
dotnet ef database update --startup-project ../ApiExpanda.API
```

#### 13. Probar
```bash
cd ../../
dotnet run --project src/ApiExpanda.API

# Navegar a http://localhost:5083/swagger
# Probar endpoints:
# GET    /api/v1/Comercial/Customers
# POST   /api/v1/Comercial/Customers
# GET    /api/v1/Comercial/Customers/{id}
# PUT    /api/v1/Comercial/Customers/{id}
# DELETE /api/v1/Comercial/Customers/{id}
```

## 📝 Checklist

- [ ] Crear entidad en Domain/Modules/{Modulo}/Entities
- [ ] Crear DTOs en Application/Modules/{Modulo}/DTOs
- [ ] Crear interface de repositorio en Application/Modules/{Modulo}/Interfaces
- [ ] Crear interface de servicio en Application/Modules/{Modulo}/Services/Interfaces
- [ ] Crear mapping en Application/Modules/{Modulo}/Mappings
- [ ] Implementar repositorio en Infrastructure/Modules/{Modulo}/Repositories
- [ ] Implementar servicio en Infrastructure/Modules/{Modulo}/Services
- [ ] Crear configuración EF Core en Infrastructure/Modules/{Modulo}/Data/Configurations
- [ ] Agregar DbSet y configuración a ApplicationDbContext
- [ ] Crear controller en API/Controllers/{Modulo}
- [ ] Registrar repositorio y servicio en Program.cs
- [ ] Crear migración con EF Core
- [ ] Aplicar migración a base de datos
- [ ] Probar endpoints en Swagger
- [ ] Documentar en README del módulo

## 💡 Tips

1. **Nombrado consistente**: `Customer`, `CustomerDto`, `CreateCustomerDto`, `ICustomerRepository`, `ICustomerService`
2. **Async siempre**: Todos los métodos de servicios deben ser async
3. **DTOs siempre**: Nunca exponer entidades directamente en APIs
4. **Validaciones**: Usar DataAnnotations en DTOs y FluentValidation si es necesario
5. **Excepciones**: Lanzar excepciones claras en servicios, manejarlas en controllers
6. **Testing**: Escribir pruebas unitarias para servicios

---

**Documento vivo** - Actualizar conforme evolucione el proyecto

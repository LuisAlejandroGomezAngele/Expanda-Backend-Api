# Módulo Shared (Compartido)

## Descripción
Componentes, entidades e interfaces compartidas entre todos los módulos.

## Contenido

### 🧱 **Entidades Base**

#### BaseEntity
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
```

#### AuditableEntity
```csharp
public abstract class AuditableEntity : BaseEntity
{
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
```

### 📦 **Value Objects**

- **Address**: Dirección completa (calle, número, colonia, CP, ciudad, estado, país)
- **Money**: Monto y moneda
- **Email**: Validación de email
- **PhoneNumber**: Número telefónico con formato
- **DateRange**: Rango de fechas (inicio, fin)

### 🔧 **Interfaces Comunes**

#### IRepository<T>
```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
```

#### IUnitOfWork
```csharp
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
```

### 📋 **Enumeraciones Comunes**

- **Estado**: Activo, Inactivo, Suspendido
- **TipoDocumento**: Factura, Ticket, NotaCrédito, NotaDébito
- **Moneda**: MXN, USD, EUR
- **TipoPersona**: Física, Moral

### 🛠️ **Utilidades**

- **Result<T>**: Patrón Result para operaciones (Success, Failure, Errors)
- **PagedList<T>**: Paginación de resultados
- **Specification<T>**: Patrón Specification para consultas complejas

### 📊 **DTOs Compartidos**

- **PaginationRequest**: page, pageSize, sortBy, sortOrder
- **PaginationResponse<T>**: items, totalItems, totalPages, currentPage
- **ErrorResponse**: código, mensaje, detalles
- **SuccessResponse<T>**: data, mensaje

## Casos de Uso

- Herencia de entidades base
- Auditoría automática
- Paginación estandarizada
- Manejo de errores consistente
- Validaciones comunes
- Helpers y extensiones

## Convenciones

- Todas las entidades deben heredar de BaseEntity
- Usar Value Objects para conceptos de dominio
- Implementar IRepository para acceso a datos genérico
- Usar Result<T> para operaciones que pueden fallar
- Documentar con XML comments

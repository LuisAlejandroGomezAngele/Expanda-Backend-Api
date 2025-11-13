# Arquitectura Modular - ApiExpanda

## 📋 Índice

1. [Visión General](#visión-general)
2. [Estructura de Módulos](#estructura-de-módulos)
3. [Módulos Disponibles](#módulos-disponibles)
4. [Convenciones y Estándares](#convenciones-y-estándares)
5. [Flujo de Desarrollo](#flujo-de-desarrollo)
6. [Dependencias entre Módulos](#dependencias-entre-módulos)

## 🎯 Visión General

El proyecto ApiExpanda está organizado siguiendo **Clean Architecture** con una estructura **modular** que permite:

- ✅ Escalabilidad horizontal (agregar nuevos módulos fácilmente)
- ✅ Mantenibilidad (cada módulo es independiente)
- ✅ Separación de responsabilidades
- ✅ Reutilización de código
- ✅ Desarrollo en paralelo por equipos

## 🏗️ Estructura de Módulos

Cada módulo sigue la misma estructura a través de las 4 capas de Clean Architecture:

```
ApiExpanda.sln
├── src/
│   ├── ApiExpanda.Domain/
│   │   ├── Entities/              # ⚠️ Legacy - migrar a Modules
│   │   └── Modules/
│   │       ├── Shared/
│   │       │   └── Entities/
│   │       ├── Catalogos/
│   │       │   └── Entities/
│   │       ├── Comercial/
│   │       │   └── Entities/
│   │       ├── Inventario/
│   │       │   └── Entities/
│   │       └── Seguridad/
│   │           └── Entities/
│   │
│   ├── ApiExpanda.Application/
│   │   ├── DTOs/                  # ⚠️ Legacy - migrar a Modules
│   │   ├── Interfaces/            # ⚠️ Legacy - migrar a Modules
│   │   ├── Mappings/              # ⚠️ Legacy - migrar a Modules
│   │   ├── Services/              # ⚠️ Legacy - migrar a Modules
│   │   └── Modules/
│   │       ├── Shared/
│   │       │   ├── DTOs/
│   │       │   ├── Interfaces/
│   │       │   ├── Mappings/
│   │       │   └── Services/Interfaces/
│   │       ├── Catalogos/
│   │       │   ├── DTOs/
│   │       │   ├── Interfaces/
│   │       │   ├── Mappings/
│   │       │   └── Services/Interfaces/
│   │       ├── Comercial/
│   │       ├── Inventario/
│   │       └── Seguridad/
│   │
│   ├── ApiExpanda.Infrastructure/
│   │   ├── Data/                  # ⚠️ Legacy - migrar a Modules
│   │   ├── Repositories/          # ⚠️ Legacy - migrar a Modules
│   │   ├── Services/              # ⚠️ Legacy - migrar a Modules
│   │   └── Modules/
│   │       ├── Shared/
│   │       │   ├── Repositories/
│   │       │   ├── Services/
│   │       │   └── Data/Configurations/
│   │       ├── Catalogos/
│   │       │   ├── Repositories/
│   │       │   ├── Services/
│   │       │   └── Data/Configurations/
│   │       ├── Comercial/
│   │       ├── Inventario/
│   │       └── Seguridad/
│   │
│   └── ApiExpanda.API/
│       └── Controllers/
│           ├── ProductsController.cs    # ⚠️ Legacy - migrar a Modules
│           ├── UsersController.cs       # ⚠️ Legacy - migrar a Modules
│           ├── v1/                      # ⚠️ Legacy - migrar a Modules
│           ├── v2/                      # ⚠️ Legacy - migrar a Modules
│           ├── Shared/
│           ├── Catalogos/
│           ├── Comercial/
│           ├── Inventario/
│           └── Seguridad/
```

## 📦 Módulos Disponibles

### 1. **Shared** (Compartido)
- **Propósito**: Código común a todos los módulos
- **Contenido**: Entidades base, interfaces genéricas, utilidades
- **Estado**: 🆕 Estructura creada

### 2. **Catalogos**
- **Propósito**: Catálogos maestros y clasificaciones
- **Contenido**: Categorías, Productos, Marcas, Unidades de Medida
- **Estado**: ⚠️ Category y Product existen pero fuera de módulo (pendiente migrar)
- **Rutas API**: `/api/v{version}/Catalogos/*`

### 3. **Comercial**
- **Propósito**: Ventas, clientes, facturación
- **Contenido**: Clientes, Ventas, Cotizaciones, Facturas, Pagos
- **Estado**: 🆕 Estructura creada
- **Rutas API**: `/api/v{version}/Comercial/*`

### 4. **Inventario**
- **Propósito**: Control de stock y almacenes
- **Contenido**: Stock, Almacenes, Movimientos, Kardex, Lotes
- **Estado**: 🆕 Estructura creada
- **Rutas API**: `/api/v{version}/Inventario/*`

### 5. **Seguridad**
- **Propósito**: Autenticación, autorización y permisos
- **Contenido**: Usuarios, Roles, Permisos, Compañías, Sesiones
- **Estado**: ⚠️ Users existe pero fuera de módulo (pendiente migrar)
- **Rutas API**: `/api/v{version}/Seguridad/*`

## 📐 Convenciones y Estándares

### Nomenclatura

#### Namespaces
```csharp
// Domain
ApiExpanda.Domain.Modules.{ModuleName}.Entities

// Application
ApiExpanda.Application.Modules.{ModuleName}.DTOs
ApiExpanda.Application.Modules.{ModuleName}.Interfaces
ApiExpanda.Application.Modules.{ModuleName}.Services.Interfaces
ApiExpanda.Application.Modules.{ModuleName}.Mappings

// Infrastructure
ApiExpanda.Infrastructure.Modules.{ModuleName}.Repositories
ApiExpanda.Infrastructure.Modules.{ModuleName}.Services
ApiExpanda.Infrastructure.Modules.{ModuleName}.Data.Configurations

// API
ApiExpanda.API.Controllers.{ModuleName}
```

#### Rutas de API
```
/api/v{version}/{ModuleName}/{ControllerName}/{action}

Ejemplos:
- GET    /api/v1/Catalogos/Categories
- POST   /api/v1/Comercial/Sales
- GET    /api/v1/Inventario/Stock/{productId}
- POST   /api/v1/Seguridad/Users/Login
```

### Dependencias entre Capas

```
API Layer (Controllers)
    ↓ depende de
Application Layer (Services, DTOs)
    ↓ depende de
Domain Layer (Entities)
    ↑ implementa
Infrastructure Layer (Repositories, Data)
```

### Reglas

1. **Domain** no debe depender de ninguna otra capa
2. **Application** solo depende de Domain
3. **Infrastructure** implementa interfaces de Application
4. **API** depende de Application e Infrastructure (solo para DI)
5. Los módulos pueden tener dependencias entre sí, pero deben ser explícitas

## 🔄 Flujo de Desarrollo

### Crear un nuevo módulo

1. **Definir entidades en Domain**
   ```bash
   src/ApiExpanda.Domain/Modules/{ModuleName}/Entities/
   ```

2. **Crear DTOs en Application**
   ```bash
   src/ApiExpanda.Application/Modules/{ModuleName}/DTOs/
   ```

3. **Definir interfaces en Application**
   ```bash
   src/ApiExpanda.Application/Modules/{ModuleName}/Interfaces/
   src/ApiExpanda.Application/Modules/{ModuleName}/Services/Interfaces/
   ```

4. **Crear mappings en Application**
   ```bash
   src/ApiExpanda.Application/Modules/{ModuleName}/Mappings/
   ```

5. **Implementar repositorios en Infrastructure**
   ```bash
   src/ApiExpanda.Infrastructure/Modules/{ModuleName}/Repositories/
   ```

6. **Implementar servicios en Infrastructure**
   ```bash
   src/ApiExpanda.Infrastructure/Modules/{ModuleName}/Services/
   ```

7. **Crear configuraciones de EF Core en Infrastructure**
   ```bash
   src/ApiExpanda.Infrastructure/Modules/{ModuleName}/Data/Configurations/
   ```

8. **Crear controllers en API**
   ```bash
   src/ApiExpanda.API/Controllers/{ModuleName}/
   ```

9. **Registrar en Program.cs**
   ```csharp
   // Repositories
   builder.Services.AddScoped<IEntityRepository, EntityRepository>();
   
   // Services
   builder.Services.AddScoped<IEntityService, EntityService>();
   ```

### Agregar una nueva funcionalidad

1. Identificar el módulo correspondiente
2. Crear/modificar entidad en Domain
3. Crear/modificar DTOs en Application
4. Actualizar interfaces de repositorio y servicio
5. Implementar lógica en servicio (Infrastructure)
6. Actualizar configuración de EF Core si es necesario
7. Crear/actualizar endpoint en controller
8. Ejecutar migración si hay cambios en BD
9. Probar endpoint

## 🔗 Dependencias entre Módulos

### Diagrama de Dependencias

```
┌─────────────┐
│   Shared    │◄─────┐
└─────────────┘      │
                     │
┌─────────────┐      │     ┌─────────────┐
│  Catalogos  │◄─────┼─────┤  Comercial  │
└─────────────┘      │     └─────────────┘
       ▲             │            ▲
       │             │            │
       │             │            │
┌─────────────┐      │     ┌─────────────┐
│ Inventario  │◄─────┼─────┤  Seguridad  │
└─────────────┘      │     └─────────────┘
       │             │            ▲
       └─────────────┴────────────┘
```

### Dependencias Explícitas

- **Catalogos**: Depende de Shared
- **Comercial**: Depende de Shared, Catalogos, Seguridad
- **Inventario**: Depende de Shared, Catalogos, Comercial
- **Seguridad**: Depende de Shared

## 📝 Migración desde estructura Legacy

### Archivos a migrar

#### Domain/Entities → Domain/Modules/Catalogos/Entities
- ✅ Category.cs (pendiente)
- ✅ Product.cs (pendiente)

#### Domain/Entities → Domain/Modules/Seguridad/Entities
- ✅ Users.cs (pendiente)
- ✅ ApplicationUser.cs (pendiente)

#### Application → Application/Modules/Catalogos
- ✅ DTOs/CategoryDto.cs (pendiente)
- ✅ DTOs/CreateCategoryDto.cs (pendiente)
- ✅ DTOs/ProductDto.cs (pendiente)
- ✅ DTOs/CreateProductDto.cs (pendiente)
- ✅ DTOs/UpdateProductDto.cs (pendiente)
- ✅ Interfaces/ICategoryRepository.cs (pendiente)
- ✅ Interfaces/IProductRepository.cs (pendiente)
- ✅ Mappings/CategoryProfile.cs (pendiente)
- ✅ Mappings/ProductProfile.cs (pendiente)
- ✅ Services/Interfaces/ICategoryService.cs (pendiente)
- ✅ Services/Interfaces/IProductService.cs (pendiente)

#### Application → Application/Modules/Seguridad
- ✅ DTOs/UserDto.cs, CreateUserDto.cs, etc. (pendiente)
- ✅ Interfaces/IUserRepository.cs (pendiente)
- ✅ Mappings/UserProfile.cs (pendiente)
- ✅ Services/Interfaces/IUserService.cs (pendiente)

#### Infrastructure → Infrastructure/Modules/Catalogos
- ✅ Repositories/CategoryRepository.cs (pendiente)
- ✅ Repositories/ProductRepository.cs (pendiente)
- ✅ Services/CategoryService.cs (pendiente)
- ✅ Services/ProductService.cs (pendiente)

#### Infrastructure → Infrastructure/Modules/Seguridad
- ✅ Repositories/UserRepository.cs (pendiente)
- ✅ Services/UserService.cs (pendiente)

#### API/Controllers → API/Controllers/Catalogos
- ✅ v1/CategoriesController.cs (pendiente)
- ✅ v2/CategoriesController.cs (pendiente)
- ✅ ProductsController.cs (pendiente)

#### API/Controllers → API/Controllers/Seguridad
- ✅ UsersController.cs (pendiente)

---

## 🚀 Próximos Pasos

1. ✅ Crear estructura de módulos (COMPLETADO)
2. ⚠️ Migrar código existente a módulos
3. 🔲 Implementar módulo Shared con entidades base
4. 🔲 Implementar módulo Seguridad (Roles, Permisos, Compañías)
5. 🔲 Implementar módulo Comercial (Clientes, Ventas)
6. 🔲 Implementar módulo Inventario (Stock, Almacenes)
7. 🔲 Documentar APIs con Swagger por módulo
8. 🔲 Implementar pruebas unitarias por módulo

---

**Última actualización**: 13 de noviembre de 2025

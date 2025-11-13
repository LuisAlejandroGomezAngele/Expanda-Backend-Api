# Estructura de Módulos - Infrastructure Layer

Esta carpeta contiene la implementación de infraestructura organizada por módulos.

## Módulos Disponibles

### 📦 **Shared**
Implementaciones compartidas (auditoría, logging, caché).

### 🏢 **Catalogos**
Repositorios y servicios de catálogos.

### 💼 **Comercial**
Repositorios y servicios del módulo comercial.

### 📊 **Inventario**
Repositorios y servicios del módulo de inventario.

### 🔐 **Seguridad**
Repositorios y servicios del módulo de seguridad.

## Estructura de cada módulo

```
ModuleName/
├── Repositories/                    # Implementación de repositorios
│   └── EntityRepository.cs
├── Services/                        # Implementación de servicios
│   └── EntityService.cs
└── Data/
    └── Configurations/              # Configuraciones de EF Core
        └── EntityConfiguration.cs
```

## Convenciones

- Los repositorios implementan interfaces de Application
- Los servicios contienen lógica de negocio
- Las configuraciones de EF Core usan Fluent API
- Aplicar patrones de diseño según sea necesario
- Mantener transacciones y UnitOfWork cuando aplique

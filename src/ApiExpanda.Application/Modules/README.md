# Estructura de Módulos - Application Layer

Esta carpeta contiene la lógica de aplicación organizada por módulos.

## Módulos Disponibles

### 📦 **Shared**
DTOs, servicios e interfaces compartidos entre módulos.

### 🏢 **Catalogos**
Lógica de aplicación para catálogos y clasificaciones.

### 💼 **Comercial**
Servicios y DTOs del módulo comercial.

### 📊 **Inventario**
Servicios y DTOs del módulo de inventario.

### 🔐 **Seguridad**
Servicios y DTOs del módulo de seguridad.

## Estructura de cada módulo

```
ModuleName/
├── DTOs/                      # Data Transfer Objects
│   ├── EntityDto.cs
│   ├── CreateEntityDto.cs
│   └── UpdateEntityDto.cs
├── Interfaces/                # Interfaces de repositorios
│   └── IEntityRepository.cs
├── Services/
│   └── Interfaces/           # Interfaces de servicios
│       └── IEntityService.cs
└── Mappings/                 # Configuraciones de Mapster
    └── EntityProfile.cs
```

## Convenciones

- Los DTOs no deben contener lógica de negocio
- Las interfaces de servicios definen contratos claros
- Los mappings usan Mapster para conversiones
- Mantener la separación entre comandos y consultas (CQRS opcional)

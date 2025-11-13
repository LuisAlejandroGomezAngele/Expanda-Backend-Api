# Estructura de Módulos - Domain Layer

Esta carpeta contiene la organización modular del dominio de negocio.

## Módulos Disponibles

### 📦 **Shared**
Contiene entidades y lógica compartida entre todos los módulos.
- Entidades base
- Interfaces comunes
- Value Objects compartidos

### 🏢 **Catalogos**
Catálogos y clasificaciones generales del sistema.
- Categorías
- Productos (actualmente aquí, puede moverse a Inventario)
- Clasificaciones
- Tipos de datos maestros

### 💼 **Comercial**
Módulo de gestión comercial y ventas.
- Clientes
- Ventas
- Cotizaciones
- Pedidos
- Facturas

### 📊 **Inventario**
Gestión de inventarios y almacenes.
- Productos (referencia desde Catálogos)
- Stock
- Almacenes
- Movimientos de inventario
- Kardex

### 🔐 **Seguridad**
Módulo de seguridad, autenticación y autorización.
- Usuarios
- Roles
- Permisos
- Compañías
- Sesiones
- Auditoría

## Estructura de cada módulo

```
ModuleName/
├── Entities/          # Entidades del dominio
│   ├── Entity1.cs
│   └── Entity2.cs
└── README.md          # Documentación específica del módulo
```

## Convenciones

- Cada entidad debe heredar de una clase base común si aplica
- Las relaciones entre módulos deben ser explícitas
- Mantener alta cohesión y bajo acoplamiento
- Documentar las dependencias entre módulos

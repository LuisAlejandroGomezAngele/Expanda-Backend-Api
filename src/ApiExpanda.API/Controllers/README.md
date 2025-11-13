# Estructura de Módulos - API Layer (Controllers)

Esta carpeta contiene los controladores organizados por módulos.

## Módulos Disponibles

### 📦 **Shared**
Controllers compartidos o utilitarios.

### 🏢 **Catalogos**
Endpoints de catálogos y clasificaciones.
- `/api/v{version}/Catalogos/Categories`
- `/api/v{version}/Catalogos/Products`

### 💼 **Comercial**
Endpoints del módulo comercial.
- `/api/v{version}/Comercial/Customers`
- `/api/v{version}/Comercial/Sales`
- `/api/v{version}/Comercial/Invoices`

### 📊 **Inventario**
Endpoints del módulo de inventario.
- `/api/v{version}/Inventario/Stock`
- `/api/v{version}/Inventario/Warehouses`
- `/api/v{version}/Inventario/Movements`

### 🔐 **Seguridad**
Endpoints del módulo de seguridad.
- `/api/v{version}/Seguridad/Users`
- `/api/v{version}/Seguridad/Roles`
- `/api/v{version}/Seguridad/Permissions`
- `/api/v{version}/Seguridad/Companies`

## Estructura de cada módulo

```
ModuleName/
├── EntityController.cs
└── v2/                    # Versionado de API
    └── EntityController.cs
```

## Convenciones

- Los controllers deben ser ligeros (delegar a servicios)
- Usar atributos de ruta descriptivos
- Implementar versionado de API cuando sea necesario
- Documentar con Swagger/OpenAPI
- Aplicar atributos de autorización por módulo
- Retornar DTOs, nunca entidades directamente

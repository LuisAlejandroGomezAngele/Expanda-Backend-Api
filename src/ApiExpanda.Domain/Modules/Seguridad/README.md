# Módulo Seguridad

## Descripción
Módulo de seguridad, autenticación, autorización y gestión de accesos.

## Entidades Principales

### 👤 **Usuario (User)**
- Id, Username, Email, Password
- Estado, FechaCreación, ÚltimoAcceso
- Relación: many-to-many con Roles

### 🎭 **Rol (Role)**
- Id, Nombre, Descripción
- Estado, Nivel
- Relación: many-to-many con Permisos

### 🔑 **Permiso (Permission)**
- Id, Código, Nombre, Descripción
- Módulo, Recurso, Acción
- Ejemplos: "catalogos.products.create", "comercial.invoices.read"

### 🏢 **Compañía (Company)**
- Id, RazonSocial, NombreComercial
- RFC/NIT, Dirección, Teléfono
- Logo, Configuraciones
- Relación: one-to-many con Usuarios

### 🔐 **Sesión (Session)**
- Id, UserId, Token, RefreshToken
- FechaInicio, FechaExpiracion
- IP, UserAgent, Dispositivo

### 📋 **AuditoriaAcceso (AccessAudit)**
- Id, UserId, Acción, Recurso
- Fecha, IP, Resultado
- MetadataRequest, MetadataResponse

## Casos de Uso

### Autenticación
- Login con credenciales
- Refresh token
- Logout / Cierre de sesión
- Recuperación de contraseña
- Verificación de email

### Autorización
- Verificar permisos por usuario
- Verificar permisos por rol
- Validar acceso a recursos
- Filtrado por compañía (multi-tenancy)

### Gestión
- CRUD de usuarios
- CRUD de roles
- CRUD de permisos
- CRUD de compañías
- Asignación de roles a usuarios
- Asignación de permisos a roles
- Auditoría de accesos

## Seguridad Implementada

- ✅ JWT Authentication
- ✅ Password hashing (Identity Framework)
- ⚠️ Refresh tokens (pendiente)
- ⚠️ Rate limiting (pendiente)
- ⚠️ Two-factor authentication (pendiente)
- ⚠️ Multi-tenancy por compañía (pendiente)

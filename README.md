# 🛒 API Expanda

API RESTful desarrollada con **ASP.NET Core 9.0** siguiendo **Clean Architecture** con estructura **modular**. Sistema empresarial completo para gestión de comercio, inventarios, seguridad y catálogos.

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Arquitectura](#-arquitectura)
- [Módulos Disponibles](#-módulos-disponibles)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Base de Datos](#-base-de-datos)
- [Migraciones](#-migraciones)
- [Ejecutar el Proyecto](#-ejecutar-el-proyecto)
- [Endpoints Principales](#-endpoints-principales)
- [Autenticación](#-autenticación)
- [Versionado de API](#-versionado-de-api)
- [Colección de Postman](#-colección-de-postman)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Documentación](#-documentación)
- [Variables de Entorno](#-variables-de-entorno)

## ✨ Características

- ✅ **Clean Architecture** con 4 capas (Domain, Application, Infrastructure, API)
- ✅ **Arquitectura Modular** (Shared, Catalogos, Comercial, Inventario, Seguridad)
- ✅ **Service Layer** para separación de lógica de negocio
- ✅ **Autenticación JWT** con ASP.NET Core Identity
- ✅ **Versionado de API** (v1 y v2)
- ✅ **Caché de Respuestas** para optimización de rendimiento
- ✅ **Entity Framework Core** con SQL Server
- ✅ **Mapster** para mapeo de objetos
- ✅ **Swagger/OpenAPI** para documentación interactiva
- ✅ **Repository Pattern** y **Service Pattern**
- ✅ **CORS** habilitado
- ✅ **Gestión de imágenes** de productos
- ✅ **Data Seeding** automático

## 🏗️ Arquitectura

El proyecto implementa **Clean Architecture** con una estructura **modular**:

```
┌─────────────────────────────────────────────────┐
│              API Layer                          │
│  (Controllers organizados por módulos)         │
└──────────────────┬──────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────┐
│         Application Layer                       │
│  (Services, DTOs, Interfaces por módulo)       │
└──────────────────┬──────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────┐
│           Domain Layer                          │
│  (Entities organizadas por módulo)             │
└──────────────────▲──────────────────────────────┘
                   │
┌──────────────────┴──────────────────────────────┐
│       Infrastructure Layer                      │
│  (Repositories, Data, Services impl)           │
└─────────────────────────────────────────────────┘
```

### Documentación Detallada

📚 **[Arquitectura Modular Completa](docs/ARQUITECTURA_MODULAR.md)** - Estructura detallada de módulos

🚀 **[Guía Rápida de Desarrollo](docs/GUIA_RAPIDA_MODULOS.md)** - Cómo crear nuevas entidades paso a paso

📋 **[Plan de Implementación](docs/PLAN_IMPLEMENTACION.md)** - Roadmap y cronograma

## 📦 Módulos Disponibles

### 🔐 **Seguridad**
Autenticación, autorización, roles, permisos y compañías.
- ✅ Usuarios (implementado)
- ⚠️ Roles, Permisos, Compañías (en desarrollo)

### 🏢 **Catálogos**
Catálogos maestros y clasificaciones.
- ✅ Categorías y Productos (implementado)
- 🔲 Marcas, Unidades de Medida, Colores (planeado)

### 💼 **Comercial**
Ventas, clientes y facturación.
- 🔲 Clientes, Ventas, Cotizaciones (planeado)

### 📊 **Inventario**
Control de stock y almacenes.
- 🔲 Almacenes, Stock, Movimientos, Kardex (planeado)

### 📦 **Shared**
Componentes compartidos entre módulos.
- 🔲 Entidades base, Value Objects, Utilidades (planeado)

## 🛠 Tecnologías Utilizadas

- **Framework**: .NET 9.0
- **ORM**: Entity Framework Core 9.0
- **Base de Datos**: SQL Server 2022
- **Autenticación**: JWT Bearer + ASP.NET Core Identity
- **Mapeo**: Mapster
- **Documentación**: Swagger/Swashbuckle
- **Contenedores**: Docker (SQL Server)
- **Control de Versiones**: Git

### Paquetes NuGet Principales

```xml
- BCrypt.Net-Next (4.0.3)
- Mapster (7.4.0)
- Microsoft.EntityFrameworkCore.SqlServer (9.0.10)
- Microsoft.AspNetCore.Authentication.JwtBearer (9.0.3)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (9.0.3)
- Asp.Versioning.Mvc (8.1.0)
- Swashbuckle.AspNetCore (9.0.6)
```

## 📦 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (para SQL Server)
- [Git](https://git-scm.com/)
- Un IDE como [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)

**Opcional:**
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- [Postman](https://www.postman.com/downloads/)

## 🚀 Instalación y Configuración

### 1. Clonar el Repositorio

```powershell
git clone https://github.com/LuisAlejandroGomezAngele/ApiExpanda.git
cd ApiExpanda
```

### 2. Restaurar Dependencias

```powershell
dotnet restore
```

### 3. Configurar la Cadena de Conexión

Edita el archivo `ApiExpanda/appsettings.json` y actualiza la cadena de conexión según tu entorno:

```json
{
  "ConnectionStrings": {
    "ConexionSql": "Server=localhost;Database=ApiExpandaNET8;User ID=SA;Password=Admin123*;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### 4. Configurar la Clave Secreta JWT

En el mismo archivo `appsettings.json`, actualiza la clave secreta (en producción, usa variables de entorno):

```json
{
  "AppSettings": {
    "SecretKey": "EstaEsUnaClaveMuySecretaParaElApiExpanda"
  }
}
```

## 🗄️ Base de Datos

### Levantar SQL Server con Docker

El proyecto incluye un archivo `docker-compose.yaml` para levantar SQL Server fácilmente:

```powershell
# Levantar SQL Server en segundo plano
docker-compose up -d

# Verificar que el contenedor está corriendo
docker ps

# Ver logs del contenedor
docker logs sqlserver2022
```

**Detalles del contenedor:**
- **Puerto**: 1433
- **Usuario**: SA
- **Contraseña**: Admin123*
- **Imagen**: mcr.microsoft.com/mssql/server:2022-latest

### Detener SQL Server

```powershell
# Apagar el contenedor
docker-compose down

# Apagar y eliminar volúmenes (⚠️ elimina todos los datos)
docker-compose down -v
```

### Conectarse a SQL Server

**Usando SSMS o Azure Data Studio:**
- **Server**: localhost,1433
- **Authentication**: SQL Server Authentication
- **Login**: SA
- **Password**: Admin123*

## 🔄 Migraciones

### Crear una Nueva Migración

```powershell
# Navegar al directorio del proyecto
cd ApiExpanda

# Crear migración
dotnet ef migrations add NombreDeLaMigracion
```

### Aplicar Migraciones

```powershell
# Aplicar todas las migraciones pendientes
dotnet ef database update

# Aplicar migración específica
dotnet ef database update NombreDeLaMigracion
```

### Ver Migraciones Disponibles

```powershell
dotnet ef migrations list
```

### Revertir Migraciones

```powershell
# Revertir a una migración específica
dotnet ef database update NombreDeLaMigracionAnterior

# Revertir todas las migraciones (elimina la base de datos)
dotnet ef database update 0
```

### Eliminar la Última Migración

```powershell
# Eliminar la migración (solo si no se ha aplicado)
dotnet ef migrations remove
```

### Eliminar la Base de Datos

```powershell
# Eliminar completamente la base de datos
dotnet ef database drop

# Forzar eliminación sin confirmación
dotnet ef database drop --force
```

### Script SQL de Migraciones

```powershell
# Generar script SQL de todas las migraciones
dotnet ef migrations script -o migrations.sql

# Generar script de una migración específica
dotnet ef migrations script MigracionInicial MigracionFinal -o migration.sql
```

## ▶️ Ejecutar el Proyecto

### Modo Desarrollo

```powershell
# Desde el directorio raíz
cd ApiExpanda
dotnet run

# O usando watch (recarga automática)
dotnet watch run
```

El API estará disponible en:
- **HTTP**: http://localhost:5083
- **HTTPS**: https://localhost:7234

### Swagger UI

Una vez que el proyecto esté corriendo, accede a Swagger:

- **V1**: https://localhost:7234/swagger/v1/swagger.json
- **V2**: https://localhost:7234/swagger/v2/swagger.json
- **UI**: https://localhost:7234/swagger

### Compilar para Producción

```powershell
# Compilar en Release
dotnet build -c Release

# Publicar
dotnet publish -c Release -o ./publish

# Ejecutar publicación
cd publish
dotnet ApiExpanda.dll
```

## 🌐 Endpoints Principales

### Autenticación

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/Users/Register` | Registrar nuevo usuario |
| POST | `/api/Users/Login` | Iniciar sesión (obtener token JWT) |

### Productos

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/api/Products` | Listar todos los productos | No |
| GET | `/api/Products/{id}` | Obtener producto por ID | No |
| POST | `/api/Products` | Crear nuevo producto | Sí (JWT) |
| PUT | `/api/Products/{id}` | Actualizar producto | Sí (JWT) |
| DELETE | `/api/Products/{id}` | Eliminar producto | Sí (JWT) |
| POST | `/api/Products/{id}/upload-image` | Subir imagen de producto | Sí (JWT) |

### Categorías V1

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/api/v1/Categories` | Listar categorías | No |
| GET | `/api/v1/Categories/{id}` | Obtener categoría por ID | No |
| POST | `/api/v1/Categories` | Crear categoría | Sí (JWT) |
| PUT | `/api/v1/Categories/{id}` | Actualizar categoría | Sí (JWT) |
| DELETE | `/api/v1/Categories/{id}` | Eliminar categoría | Sí (JWT) |

### Categorías V2

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/api/v2/Categories` | Listar categorías (paginado) | No |

## 🔐 Autenticación

La API utiliza **JWT Bearer Token** para autenticación.

### 1. Registrar Usuario

```json
POST /api/Users/Register
{
  "username": "usuario",
  "email": "usuario@example.com",
  "password": "Password123",
  "confirmPassword": "Password123"
}
```

### 2. Iniciar Sesión

```json
POST /api/Users/Login
{
  "username": "usuario",
  "password": "Password123"
}
```

**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-11-13T10:30:00Z",
  "user": {
    "id": "123",
    "username": "usuario",
    "email": "usuario@example.com"
  }
}
```

### 3. Usar el Token

En las peticiones protegidas, agrega el header:

```
Authorization: Bearer <tu-token-aqui>
```

**Ejemplo en Swagger:**
1. Haz clic en el botón **"Authorize"** 🔒
2. Ingresa tu token (sin el prefijo "Bearer")
3. Haz clic en **"Authorize"**

## 📊 Versionado de API

La API soporta versionado mediante URL:

- **V1**: `/api/v1/...` - Versión estable
- **V2**: `/api/v2/...` - Versión con mejoras (ej: paginación)

**Configuración:**
- Versión por defecto: **v1**
- Se reportan las versiones disponibles en las cabeceras de respuesta

## 📮 Colección de Postman

El proyecto incluye una colección de Postman lista para usar:

📄 `ApiExpanda/API-Expanda.postman_collection.json`

### Importar en Postman

1. Abre Postman
2. Click en **Import**
3. Selecciona el archivo `API-Expanda.postman_collection.json`
4. La colección estará lista con todos los endpoints

## 📁 Estructura del Proyecto

```
ApiExpanda/
├── Controllers/           # Controladores de la API
│   ├── v1/               # Controladores versión 1
│   └── v2/               # Controladores versión 2
├── Models/               # Modelos de dominio
│   └── Dtos/            # Data Transfer Objects
│       └── Responses/   # DTOs de respuesta
├── Repository/           # Implementación de repositorios
│   └── IRepository/     # Interfaces de repositorios
├── Data/                 # Contexto de base de datos y seeding
├── Mapping/              # Configuración de Mapster
├── Migrations/           # Migraciones de EF Core
├── Constants/            # Constantes de la aplicación
├── Properties/           # Configuración de lanzamiento
└── wwwroot/             # Archivos estáticos
    └── ProductsImages/  # Imágenes de productos
```

## 🔧 Variables de Entorno

Para producción, se recomienda usar variables de entorno en lugar de `appsettings.json`:

### Windows (PowerShell)

```powershell
$env:ConnectionStrings__ConexionSql="Server=tu-servidor;Database=tu-bd;..."
$env:AppSettings__SecretKey="TuClaveSecretaMuySegura"
```

### Linux/Mac

```bash
export ConnectionStrings__ConexionSql="Server=tu-servidor;Database=tu-bd;..."
export AppSettings__SecretKey="TuClaveSecretaMuySegura"
```

### Docker

```yaml
environment:
  - ConnectionStrings__ConexionSql=Server=sql;Database=...
  - AppSettings__SecretKey=TuClaveSecreta
```

## 📝 Notas Adicionales

### Cache de Respuestas

La API implementa caché de respuestas con perfiles configurables:
- **Default30**: 30 segundos
- **Default20**: 20 segundos

### CORS

CORS está habilitado para todos los orígenes en desarrollo. En producción, configura los orígenes permitidos en `Program.cs`.

### Imágenes de Productos

Las imágenes se almacenan en `wwwroot/ProductsImages/`. Asegúrate de que el directorio tenga permisos de escritura.

### Data Seeding

Al iniciar la aplicación por primera vez, se cargan datos de prueba automáticamente (categorías y usuarios de ejemplo).

## 🤝 Contribuir

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👤 Autor

**Luis Alejandro Gómez Angele**

- GitHub: [@LuisAlejandroGomezAngele](https://github.com/LuisAlejandroGomezAngele)

## 📞 Soporte

Si tienes alguna pregunta o problema, por favor abre un [issue](https://github.com/LuisAlejandroGomezAngele/ApiExpanda/issues) en GitHub.

---

⭐ Si este proyecto te fue útil, considera darle una estrella en GitHub!

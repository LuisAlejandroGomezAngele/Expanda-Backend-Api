# Módulo Catálogos

## Descripción
Catálogos maestros y clasificaciones generales del sistema.

## Entidades Principales

### 📁 **Categoría (Category)**
- ✅ Ya implementada
- Id, Nombre, Descripción, Imagen
- Estado, FechaCreación

### 🏷️ **Producto (Product)**
- ✅ Ya implementada (considerar mover a Inventario)
- Id, Nombre, Descripción, Precio
- CategoryId, Imagen, Stock

### 🔖 **Marca (Brand)**
- Id, Nombre, Descripción
- Logo, PaísOrigen
- Estado

### 📏 **UnidadMedida (UnitOfMeasure)**
- Id, Código, Nombre
- Símbolo, Tipo (longitud, peso, volumen, etc.)
- FactorConversión

### 🎨 **Color (Color)**
- Id, Nombre, CódigoHex
- CódigoRGB

### 📐 **Talla (Size)**
- Id, Código, Nombre
- Categoría (ropa, calzado, etc.)
- Orden

### 💰 **TipoCambio (ExchangeRate)**
- Id, MonedaOrigen, MonedaDestino
- Tasa, FechaVigencia
- Fuente

### 🌍 **País (Country)**
- Id, Código, Nombre
- CódigoISO2, CódigoISO3
- Bandera

### 📍 **Estado/Provincia (State)**
- Id, PaísId, Nombre
- Código

### 🏙️ **Ciudad (City)**
- Id, EstadoId, Nombre
- CódigoPostal

## Casos de Uso

- Gestión de catálogos maestros
- Consulta de clasificaciones
- Búsqueda y filtrado de catálogos
- Sincronización de catálogos externos
- Importación/exportación de catálogos

## Notas

- Los catálogos suelen ser de lectura frecuente
- Considerar implementar caché
- Algunos catálogos pueden ser jerárquicos (categorías, ubicaciones)

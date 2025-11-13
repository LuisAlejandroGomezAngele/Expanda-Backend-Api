# Módulo Inventario

## Descripción
Gestión de inventarios, almacenes, movimientos y control de stock.

## Entidades Principales

### 📦 **ProductoInventario (InventoryProduct)**
- Id, ProductoId (referencia a Catálogos)
- SKU, CodigoBarras
- StockMinimo, StockMaximo
- PuntoPedido
- CostoPromedio, UltimoCosto
- Estado

### 🏭 **Almacén (Warehouse)**
- Id, Código, Nombre
- Dirección, ResponsableId
- Tipo (Principal, Sucursal, Tránsito)
- Estado, CompañíaId

### 📊 **Stock**
- Id, ProductoId, AlmacénId
- CantidadDisponible, CantidadReservada
- CantidadEnTránsito
- UbicaciónFísica (pasillo, estante, nivel)
- FechaÚltimaActualización

### 🔄 **MovimientoInventario (InventoryMovement)**
- Id, Folio, Fecha
- ProductoId, AlmacénOrigenId, AlmacénDestinoId
- TipoMovimiento (Entrada, Salida, Transferencia, Ajuste)
- Cantidad, Costo
- Motivo, Referencia
- UsuarioId, Estado

### 📋 **DetalleMovimiento (MovementDetail)**
- Id, MovimientoId, ProductoId
- Cantidad, CostoUnitario
- Lote, FechaVencimiento
- NumeroSerie (para productos serializados)

### 🏷️ **Lote (Batch)**
- Id, ProductoId, NúmeroLote
- FechaProducción, FechaVencimiento
- Cantidad, CantidadDisponible
- Estado

### 📝 **OrdenCompra (PurchaseOrder)**
- Id, Folio, Fecha
- ProveedorId, AlmacénDestinoId
- Subtotal, Impuestos, Total
- Estado (Pendiente, Parcial, Recibida, Cancelada)
- FechaEstimadaEntrega

### 📥 **RecepciónMercancía (GoodsReceipt)**
- Id, OrdenCompraId, Fecha
- AlmacénId, UsuarioRecibeId
- Observaciones, Estado

### 🔍 **Auditoría/Inventario Físico (PhysicalInventory)**
- Id, Folio, Fecha
- AlmacénId, UsuarioId
- Estado (EnProgreso, Completado, Cancelado)
- Observaciones

### 📊 **Kardex**
- Registro histórico de movimientos por producto
- Fecha, TipoMovimiento, Documento
- Entrada, Salida, Saldo
- CostoUnitario, CostoTotal

## Casos de Uso

### Control de Stock
- Consultar stock por producto y almacén
- Reservar stock para ventas
- Liberar stock de reservas canceladas
- Alertas de stock bajo
- Productos sin movimiento

### Movimientos
- Entrada por compra
- Salida por venta
- Transferencia entre almacenes
- Ajuste de inventario (merma, robo, error)
- Devoluciones

### Trazabilidad
- Kardex por producto
- Historial de movimientos
- Rastreo por lote
- Rastreo por número de serie

### Valuación
- Cálculo de costo promedio
- Valorización de inventario
- Reportes de rotación
- Análisis ABC

### Auditoría
- Conteo físico
- Conciliación física vs sistema
- Ajustes por diferencias
- Reporte de auditoría

## Métodos de Valuación

- ✅ Promedio ponderado (implementar)
- 🔲 PEPS (FIFO)
- 🔲 UEPS (LIFO)
- 🔲 Costo estándar

## Integraciones

- ➡️ Catálogos: Referencia a productos
- ➡️ Comercial: Afectación por ventas
- ➡️ Compras: Entrada por compras (módulo futuro)

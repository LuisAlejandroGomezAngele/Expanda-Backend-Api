# Módulo Comercial

## Descripción
Gestión de ventas, clientes, cotizaciones y facturación.

## Entidades Principales

### 👥 **Cliente (Customer)**
- Id, TipoCliente (Persona/Empresa)
- Nombre, RFC/NIT, Email, Teléfono
- Dirección, Ciudad, Estado, País
- LímiteCrédito, DíasCrédito
- Estado, FechaRegistro
- Relación: one-to-many con Ventas

### 🛒 **Venta (Sale)**
- Id, Folio, Fecha
- ClienteId, UsuarioId
- Subtotal, Impuestos, Descuento, Total
- Estado (Pendiente, Completada, Cancelada)
- FormaPago, MetodoPago
- Relación: one-to-many con DetallesVenta

### 📝 **DetalleVenta (SaleDetail)**
- Id, VentaId, ProductoId
- Cantidad, PrecioUnitario
- Descuento, Subtotal
- Impuestos

### 💵 **Cotización (Quotation)**
- Id, Folio, Fecha, Vigencia
- ClienteId, UsuarioId
- Subtotal, Impuestos, Descuento, Total
- Estado (Borrador, Enviada, Aceptada, Rechazada)
- Observaciones
- Relación: one-to-many con DetallesCotización

### 📄 **Factura (Invoice)**
- Id, Serie, Folio
- VentaId, ClienteId
- FechaEmisión, FechaVencimiento
- Subtotal, IVA, Total
- Estado (Emitida, Pagada, Vencida, Cancelada)
- UUID (Timbre Fiscal - México)
- XMLFactura, PDFFactura

### 💳 **Pago (Payment)**
- Id, FacturaId, Fecha
- Monto, FormaPago
- Referencia, Banco
- Estado (Pendiente, Aplicado, Rechazado)

### 📦 **Pedido (Order)**
- Id, Folio, Fecha
- ClienteId, CotizaciónId
- Estado (Nuevo, EnProceso, Enviado, Entregado)
- DirecciónEnvío
- FechaEstimadaEntrega

### 🚚 **Envío (Shipment)**
- Id, PedidoId
- Paquetería, GuíaRastreo
- FechaEnvío, FechaEntrega
- Estado

## Casos de Uso

### Ventas
- Crear punto de venta (POS)
- Procesar venta rápida
- Aplicar descuentos y promociones
- Calcular impuestos
- Generar ticket/comprobante

### Cotizaciones
- Crear cotización
- Enviar cotización al cliente
- Convertir cotización a pedido
- Seguimiento de cotizaciones

### Facturación
- Generar factura desde venta
- Timbrar factura (CFDI - México)
- Enviar factura por email
- Cancelar factura
- Reporte de facturas

### Clientes
- CRUD de clientes
- Historial de compras
- Estado de cuenta
- Análisis de clientes frecuentes

## Integraciones Futuras

- 🔲 Facturación electrónica (PAC en México)
- 🔲 Pago en línea (Stripe, PayPal, MercadoPago)
- 🔲 Envíos (FedEx, DHL, Estafeta)
- 🔲 CRM externo

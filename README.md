# Xolit API - Gestión de Reservas

## Tecnologías Utilizadas

### **Backend**
- **.NET 8.0**: Framework principal para el desarrollo del backend, asegurando alto rendimiento y escalabilidad.
- **Entity Framework Core**: ORM para interactuar con la base de datos de manera sencilla y eficiente.
- **Pomelo.EntityFrameworkCore.MySql**: Proveedor específico para bases de datos MySQL.
- **NUnit y Moq**: Herramientas para escribir y ejecutar pruebas unitarias, simulando comportamientos y dependencias.
- **MediatR**: Implementación del patrón CQRS para separar lógica de comandos y consultas.
- **AutoMapper**: Facilita el mapeo entre entidades y DTOs.

---

## Características Principales

### **Usuarios**
- **Creación de Usuarios**: Endpoint para registrar nuevos usuarios en el sistema.
- **Inicio de Sesión**: Validación segura de credenciales para autenticación.

### **Espacios**
- **Gestión de Espacios**: Creación y eliminación de espacios disponibles para reservas.
- **Validaciones de Solapamientos**: Asegura que las reservas no se superpongan en el mismo espacio.

### **Reservas**
- **Creación de Reservas**: Incluye validaciones de horarios, duraciones mínimas/máximas y solapamientos.
- **Consulta de Horarios Disponibles**: Permite verificar intervalos de tiempo libres para reservas en un espacio específico.
- **Historial de Reservas por Usuario**: Consultar las reservas realizadas por un usuario.

### **Pruebas Unitarias**
- **Cobertura Amplia**: Tests para los comandos y consultas principales del sistema.
- **Mocking**: Uso de Moq para simular interacciones con el repositorio y servicios externos.

---

## Instalación

### **Requisitos Previos**
1. Tener instalado **.NET SDK 8.0**.
2. Contar con una instancia de base de datos MySQL.
3. Configurar correctamente las variables de entorno necesarias para la conexión a la base de datos.

### **Pasos de Instalación**
1. **Clonar el repositorio:**
   ```bash
   git clone url-repositorio
   cd Xolit.API.Reservation
### **Pasos de Instalación**
1. **Restaurar paquetes:**
   ```bash
   dotnet restore
### **Configurar la base de datos**
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=<servidor>;Database=<base_de_datos>;User=<usuario>;Password=<contraseña>;"
    }
}

### **Ejecutar el proyecto**
dotnet run

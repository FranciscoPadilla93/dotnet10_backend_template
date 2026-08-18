# LUPA API - Boilerplate .NET 10

Plantilla base (*boilerplate*) para el desarrollo de APIs RESTful empresariales construida sobre **.NET 10**. Incorpora autenticación JWT, un sistema dinámico de autorización basado en permisos, procesamiento en segundo plano, auditoría integrada y generación de reportes avanzados.

---

## 🛠️ Tecnologías y Librerías Principal

* **Framework:** .NET 10 Web API
* **Base de Datos & ORM:** Entity Framework Core 10 (SQL Server)
* **Autenticación & Seguridad:** JWT Bearer, BCrypt.Net-Next
* **Reporte y Exportación:** QuestPDF, ClosedXML, ScottPlot
* **Servicios de Correo:** MailKit (Worker en segundo plano mediante `IHostedService`)
* **Documentación:** OpenAPI (Microsoft.AspNetCore.OpenApi)

---

## 🏗️ Estructura del Proyecto

```text
LUPA.Api/
├── Common/            # Clases compartidas, atributos y proveedores de autorización.
├── Configuration/     # Clases de opciones fuertemente tipadas (JwtOptions, EmailOptions).
├── Controllers/       # Endpoints de la API REST.
├── Data/              # DbContext y configuraciones de Entity Framework Core.
├── DTOs/              # Objetos de Transferencia de Datos (Data Transfer Objects).
├── Entities/          # Modelos de dominio y tablas de base de datos.
├── Extensions/        # Métodos de extensión para DI, middleware y bootstrapping.
├── Interfaces/        # Contratos de servicios y repositorios.
├── Mappings/          # Perfiles de mapeo entre entidades y DTOs.
├── Middlewares/       # Middleware de excepciones y manejo global de respuestas.
├── Migrations/        # Migraciones de EF Core.
├── Repositories/      # Patrón Repositorio / Capa de acceso a datos.
├── Requests/          # Modelos de entrada para peticiones HTTP.
├── Responses/         # Estructuras unificadas de respuesta HTTP.
├── Services/          # Lógica de negocio (Audit, Email, Menus, Roles, Users, etc.).
└── Validators/        # Validaciones de reglas de negocio o entradas.

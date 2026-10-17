# RSS Feed Reader

Aplicación de ejemplo para gestionar suscripciones a feeds RSS con una API en ASP.NET Core y una interfaz en Blazor WebAssembly.

## ¿Qué es este proyecto?

Este repositorio está dividido en dos partes:

- Backend: una API REST para guardar y consultar suscripciones RSS.
- Frontend: una interfaz web para agregar URLs de feeds y ver la lista de suscripciones.

La idea es que el frontend consuma la API para mostrar y registrar feeds, mientras que el backend mantiene la información en memoria por ahora.

## Estructura del proyecto

- backend/RSSFeedReader.Api: API REST con Swagger.
- frontend/RSSFeedReader.UI: aplicación Blazor WebAssembly.
- RSSFeedReader.sln: solución que agrupa ambos proyectos.

## Tecnologías principales

- .NET 8
- ASP.NET Core Web API
- Blazor WebAssembly
- Swagger / OpenAPI

## Requisitos previos

Instala el SDK de .NET 8 antes de ejecutar el proyecto.

- Visual Studio 2022 o VS Code con C# Dev Kit
- .NET 8 SDK

## Ejecutar el backend

Desde la raíz del proyecto:

```bash
cd backend/RSSFeedReader.Api
dotnet restore
dotnet run
```

La API quedará disponible en:

- http://localhost:5151
- Swagger: http://localhost:5151/swagger

Endpoints principales:

- GET /api/subscriptions
- POST /api/subscriptions

## Ejecutar el frontend

Abre otra terminal y ejecuta:

```bash
cd frontend/RSSFeedReader.UI
dotnet restore
dotnet run
```

La aplicación quedará disponible en:

- http://localhost:5213

La UI está configurada para apuntar a la API en:

- http://localhost:5151/api/

## Ejecutar la solución completa

También puedes compilar la solución completa desde la raíz:

```bash
dotnet build RSSFeedReader.sln
```

## Funcionalidad actual

Actualmente la app permite:

- Agregar una URL de feed RSS
- Listar todas las suscripciones cargadas
- Consumir la API desde la UI
- Ver la API documentada con Swagger

> Nota: la información se guarda en memoria en el backend, por lo que se reinicia cuando se detiene la aplicación.

## Flujo normal de uso

1. Inicia el backend.
2. Inicia el frontend.
3. Abre la URL del frontend en el navegador.
4. Ingresa una URL de un feed RSS.
5. La UI la envía al backend y la lista la muestra en pantalla.

## Siguientes mejoras posibles

- Persistencia en base de datos
- Consulta real de contenido RSS
- Eliminación y edición de suscripciones
- Autenticación de usuarios
- Carga de feeds en segundo plano

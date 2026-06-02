# Ecommerce-Backend

Este repositorio contiene dos proyectos principales:

- `MiApi/` — un API de ejemplo mínimo generado con ASP.NET Core.
- `MiApp/` — la aplicación principal del backend con arquitectura limpia, JWT, roles y EF Core.

> El proyecto principal que debes usar y documentar es `MiApp`.

## Estructura del repositorio

- `MiApi/`
  - `MiApi.csproj`
  - `Controllers/`
  - `Models/`
  - `Services/`
  - API simple de demostración

- `MiApp/`
  - `MiApp.sln` — solución principal
  - `src/MiApp.Domain/` — entidades y reglas de dominio
  - `src/MiApp.Application/` — casos de uso, lógica de aplicación y MediatR
  - `src/MiApp.Infrastructure/` — datos, EF Core, JWT y dependencias externas
  - `src/MiApp.WebApi/` — API web, controllers y configuración

## Proyecto principal: `MiApp`

`MiApp` está diseñado como un backend para un e-commerce con las siguientes características:

- Arquitectura limpia (separación de dominio, aplicación, infraestructura y presentación)
- Autenticación JWT
- Roles `Admin` y `User`
- Entidad `Product` con creación y listado de productos activos
- SQLite como base de datos
- Seed inicial de usuarios y roles en la migración
- Políticas de autorización basadas en roles

## Requisitos previos

- .NET 8 SDK
- PowerShell o terminal compatible
- No es necesario instalar SQLite por separado si se usa el proveedor EF Core SQLite

## Cómo comenzar con `MiApp`

1. Abrir una terminal en la carpeta raíz del repositorio.
2. Ejecutar:
   ```powershell
   cd "Ecommerce-Backend\MiApp"
   dotnet restore
   dotnet build MiApp.sln
   dotnet ef database update --project src/MiApp.Infrastructure/MiApp.Infrastructure.csproj --startup-project src/MiApp.WebApi/MiApp.WebApi.csproj
   dotnet run --project src/MiApp.WebApi/MiApp.WebApi.csproj
   ```

3. Abrir Swagger en:
   ```text
   http://localhost:5073/swagger
   ```

> Si necesitas volver a aplicar la semilla de roles, elimina `src/MiApp.WebApi/ecommerce.db` y ejecuta nuevamente el comando de migración.

## Configuración JWT

La configuración se encuentra en `src/MiApp.WebApi/appsettings.json`.

Ejemplo de valores:

```json
"Jwt": {
  "Issuer": "MiApp",
  "Audience": "MiApp.Client",
  "Key": "REEMPLAZA_CON_UNA_CLAVE_SECRETA_LARGA",
  "ExpirationHours": "1"
}
```

### Nota importante

Cambia `Jwt:Key` por una clave larga y segura antes de usar el backend en producción.

## Endpoints principales de `MiApp`

- `POST /api/auth/login` — iniciar sesión y recibir un token JWT
- `POST /api/auth/register` — registrar un nuevo usuario
- `GET /api/products` — listar los productos activos (`Admin` o `User`)
- `POST /api/products` — crear producto (`Admin` solamente)

## Roles y autorización

- `Admin` — puede crear productos y realizar acciones administrativas
- `User` — puede consumir endpoints protegidos de lectura
- `AuthController` permite el acceso anónimo a login y registro
- `ProductsController` usa políticas de rol en JWT

## Usuarios seed

La base de datos inicial incluye:

- Admin:
  - Email: `admin@example.com`
  - Password: `Admin123!`
  - Role: `Admin`

- Usuario:
  - Email: `user@example.com`
  - Password: `User123!`
  - Role: `User`

## Cómo probar con Swagger

1. Iniciar sesión en `POST /api/auth/login`.
2. Copiar el token JWT.
3. Hacer clic en `Authorize` en Swagger.
4. Pegar el token con el prefijo `Bearer `.
5. Probar los endpoints protegidos.

## Comandos útiles

```powershell
# Restaurar y compilar
cd "C:\Users\Nacho\Desktop\inacio\Estoy podrido\Ecommerce-Backend\MiApp"
dotnet restore
dotnet build MiApp.sln

# Aplicar migraciones
dotnet ef database update --project src/MiApp.Infrastructure/MiApp.Infrastructure.csproj --startup-project src/MiApp.WebApi/MiApp.WebApi.csproj

# Ejecutar la API
dotnet run --project src/MiApp.WebApi/MiApp.WebApi.csproj
```

## Referencias adicionales

- El README específico del proyecto está en `MiApp/README.md`.
- Si deseas usar el proyecto `MiApi/`, revisa su carpeta correspondiente.

## Nota final

Este repositorio contiene dos proyectos, pero la implementación principal de backend y los requisitos de JWT/roles están en `MiApp`. El `README.md` dentro de `MiApp` tiene los pasos detallados para ejecutar el backend completo.

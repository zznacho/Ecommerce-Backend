# MiApp

Backend ASP.NET Core con arquitectura limpia, JWT, EF Core y roles.

## Requisitos previos

- .NET 8 SDK
- SQLite (no es obligatorio instalarlo de forma independiente, ya que EF Core usa el proveedor SQLite)
- PowerShell o terminal compatible

## Estructura importante

- `MiApp.sln` — solución principal
- `src/MiApp.WebApi` — API web
- `src/MiApp.Infrastructure` — capa de infraestructura, EF Core y JWT
- `src/MiApp.Application` — lógica de aplicación y casos de uso
- `src/MiApp.Domain` — entidades de dominio

## Configuración de JWT

La configuración JWT está en `src/MiApp.WebApi/appsettings.json`.

Ejemplo:

```json
"Jwt": {
  "Issuer": "MiApp",
  "Audience": "MiApp.Client",
  "Key": "REEMPLAZA_CON_UNA_CLAVE_SECRETA_LARGA",
  "ExpirationHours": "1"
}
```

> Cambia `Jwt:Key` por una clave larga y segura antes de usar en producción.

## Base de datos y migraciones

El proyecto usa SQLite y el archivo de base de datos generado es:

- `src/MiApp.WebApi/ecommerce.db`

Para crear o actualizar la base de datos:

```powershell
cd "C:\Users\Nacho\Desktop\inacio\Estoy podrido\Ecommerce-Backend\MiApp"
dotnet restore
dotnet build MiApp.sln
dotnet ef database update --project src/MiApp.Infrastructure/MiApp.Infrastructure.csproj --startup-project src/MiApp.WebApi/MiApp.WebApi.csproj
```

Si ya existe la base de datos y quieres volver a aplicar el seed de roles, elimina primero el archivo `src/MiApp.WebApi/ecommerce.db` y luego ejecuta el comando anterior.

## Ejecutar la API

```powershell
cd "C:\Users\Nacho\Desktop\inacio\Estoy podrido\Ecommerce-Backend\MiApp"
dotnet run --project src/MiApp.WebApi/MiApp.WebApi.csproj
```

La API escuchará en `http://localhost:5073`.

## Swagger

Cuando la API esté en ejecución, abre Swagger en:

- `http://localhost:5073/swagger`

Desde Swagger puedes probar:

1. `POST /api/auth/register`
2. `POST /api/auth/login`
3. `GET /api/products`
4. `POST /api/products`

## Autenticación y roles

### Endpoints públicos

- `POST /api/auth/login` — iniciar sesión
- `POST /api/auth/register` — registro de usuario

### Endpoints protegidos

- `GET /api/products` — requiere rol `Admin` o `User`
- `POST /api/products` — requiere solo rol `Admin`
- `GET /weatherforecast` — requiere rol `Admin` o `User`

### Políticas de rol

- `AdminPolicy` — acceso exclusivo para el rol `Admin`
- `UserPolicy` — acceso exclusivo para el rol `User`
- `AdminOrUserPolicy` — acceso para `Admin` o `User`

## Credenciales seed

La base de datos contiene estos usuarios iniciales:

- Admin:
  - Email: `admin@example.com`
  - Password: `Admin123!`
  - Role: `Admin`

- Usuario:
  - Email: `user@example.com`
  - Password: `User123!`
  - Role: `User`

## Cómo probar con Swagger

1. Ir a `POST /api/auth/login`
2. Enviar:
   ```json
   {
     "email": "admin@example.com",
     "password": "Admin123!"
   }
   ```
3. Copiar el token de la respuesta.
4. Hacer clic en `Authorize` en Swagger.
5. Pegar el token con el prefijo `Bearer `:
   ```text
   Bearer eyJ..."
   ```
6. Probar `GET /api/products` o `POST /api/products`.

## Ejemplo rápido con `curl`

Obtener token:

```bash
curl -X POST "http://localhost:5073/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin123!"}'
```

Usar token en un endpoint protegido:

```bash
curl -X GET "http://localhost:5073/api/products" \
  -H "Authorization: Bearer <TOKEN>"
```

Crear producto como admin:

```bash
curl -X POST "http://localhost:5073/api/products" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <TOKEN>" \
  -d '{"name":"Nuevo producto","description":"Demo","price":12.5,"stock":10,"isActive":true}'
```

## Casos de uso implementados

- `CreateProductCommand` — comando para creación de productos
- `GetActiveProductsQuery` — consulta para listar productos activos
- `LoginUseCase` — autenticación JWT
- `RegisterUseCase` — registro de usuarios

## Notas finales

- `AuthController` deja libre el registro y login.
- `ProductsController` y `WeatherForecastController` requieren JWT.
- Para cambiar el secreto JWT, edita `src/MiApp.WebApi/appsettings.json`.
- Si cambias la seed de usuarios o roles, elimina `src/MiApp.WebApi/ecommerce.db` y vuelve a ejecutar la migración.

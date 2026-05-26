# Seguridad JWT — Instrucciones rápidas

1) Paquetes NuGet necesarios

dotnet add src\MiApp.Application\MiApp.Application.csproj package BCrypt.Net-Next
dotnet add src\MiApp.WebApi\MiApp.WebApi.csproj package Microsoft.AspNetCore.Authentication.JwtBearer

2) Configuración
- Editar `src/MiApp.WebApi/appsettings.json` y reemplazar `Jwt:Key` por una clave segura
  (o exportar variable de entorno `Jwt__Key`).

3) Compilar y ejecutar

dotnet build MiApp.sln
dotnet run --project src\MiApp.WebApi\MiApp.WebApi.csproj

4) Probar endpoints
- Registrar un usuario: POST /api/auth/register { "email","name","password" }
- Hacer login: POST /api/auth/login { "email","password" } → obtener `token`
- Usar header `Authorization: Bearer <token>` para acceder a endpoints protegidos (ej. POST /api/products)

Notas:
- No guardar `Jwt:Key` en repositorios públicos.
- Los paquetes fueron añadidos a los .csproj; si prefieres, ejecuta los comandos de `dotnet add` arriba.

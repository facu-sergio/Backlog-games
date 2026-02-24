# Plan de Implementación: Auth Backend - Gaming Backlog API

## Contexto

Esta es una API ASP.NET Core con Entity Framework Core y PostgreSQL. Es un backlog de juegos personal. Se necesita agregar autenticación JWT simple (sin ASP.NET Identity) para proteger los endpoints. Por ahora habrá un solo usuario (admin).

## Stack actual

- ASP.NET Core
- Entity Framework Core + PostgreSQL
- AutoMapper
- Arquitectura en capas con Repository Pattern + Unit of Work

## Arquitectura del proyecto

La solución `BacklogGames.sln` tiene 3 proyectos:

### 1. `BacklogGames` (Proyecto API / Presentación)
```
BacklogGames/
├── Controllers/
├── Exceptions/
├── Properties/
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── BacklogGames.csproj
```

### 2. `BacklogGames.Bussinnes.Layer` (Capa de Negocio)
```
BacklogGames.Bussinnes.Layer/
├── DTOs/
├── Models/
├── Services/
├── AutoMapperProfile.cs
├── Extensions.cs
└── BacklogGames.Bussinnes.Layer.csproj
```

### 3. `BacklogGames.DataAccess.Layer` (Capa de Acceso a Datos)
```
BacklogGames.DataAccess.Layer/
├── Configurations/
├── Data/              ← Acá está AppDbContext
├── Enums/
├── Migrations/
├── Models/            ← Entidades de DB
├── Repositories/
├── UnitOfWork/
└── BacklogGames.DataAccess.Layer.csproj
```

## Convenciones a respetar

- **DbContext:** `AppDbContext` (en `DataAccess.Layer/Data/`)
- **Entidades de DB** → `BacklogGames.DataAccess.Layer/Models/`
- **Configuraciones EF (Fluent API)** → `BacklogGames.DataAccess.Layer/Configurations/`
- **DTOs** → `BacklogGames.Bussinnes.Layer/DTOs/`
- **Servicios (interfaz + implementación)** → `BacklogGames.Bussinnes.Layer/Services/`
- **Controllers** → `BacklogGames/Controllers/`
- **Registrar servicios en DI** → `BacklogGames/Program.cs`
- Seguir el patrón existente de Repository + UnitOfWork para acceder a datos desde los servicios.
- Respetar el estilo, namespaces y convenciones de los archivos existentes en cada capa.

---

## Paso 1: Instalar paquetes NuGet

En el proyecto API (`BacklogGames`):
- `Microsoft.AspNetCore.Authentication.JwtBearer`

En la capa de negocio (`BacklogGames.Bussinnes.Layer`):
- `BCrypt.Net-Next`
- `System.IdentityModel.Tokens.Jwt`
- `Microsoft.IdentityModel.Tokens`

Verificar si la capa de negocio ya tiene referencia a `Microsoft.Extensions.Configuration`. Si no, agregarla.

---

## Paso 2: Crear la entidad User

Crear `User.cs` en `BacklogGames.DataAccess.Layer/Models/`.

Campos:
- `Id` (int, PK)
- `Username` (string)
- `PasswordHash` (string)
- `CreatedAt` (DateTime, default UTC now)

No usar ASP.NET Identity. Es una tabla simple.

---

## Paso 3: Registrar User en AppDbContext y crear Configuration

### 3a. Agregar `DbSet<User>` en `AppDbContext`

### 3b. Crear `UserConfiguration.cs` en `BacklogGames.DataAccess.Layer/Configurations/`

Seguir el patrón de las configuraciones existentes en esa carpeta.

Configuración:
- Username: único, max 50 chars, required
- PasswordHash: required
- Seed con un usuario inicial: username `"facu"`, el hash se obtiene en el Paso 4

Verificar cómo se registran las configuraciones en `AppDbContext`. Si usa `ApplyConfigurationsFromAssembly` ya se registra sola. Si no, registrarla manualmente en `OnModelCreating`.

---

## Paso 4: Generar hash BCrypt del password

**IMPORTANTE: NO hardcodear passwords en ningún archivo del repo.**

1. Crear un script/programa temporal que use `BCrypt.Net.BCrypt.HashPassword()` para generar un hash.
2. Ejecutarlo y **mostrar el hash generado al usuario**.
3. **ESPERAR confirmación del usuario** antes de continuar.
4. Una vez confirmado, reemplazar el placeholder en `UserConfiguration.cs` con el hash generado.
5. Eliminar el script temporal.

---

## Paso 5: Crear la migración y aplicarla

Crear migración `AddUserTable` y aplicarla. Tener en cuenta que el proyecto de migraciones es `BacklogGames.DataAccess.Layer` y el startup project es `BacklogGames`.

Verificar que la tabla `Users` se creó en PostgreSQL con el usuario seedeado.

---

## Paso 6: Configuración JWT en appsettings.json

Agregar una sección `Jwt` en `BacklogGames/appsettings.json` con:
- `Key`: generar una key segura de al menos 32 caracteres (se puede usar `openssl rand -base64 32`)
- `Issuer`: `"GamingBacklogAPI"`
- `Audience`: `"GamingBacklogApp"`
- `ExpireInDays`: `7`

---

## Paso 7: Crear DTOs para Auth

Crear en `BacklogGames.Bussinnes.Layer/DTOs/`:

**LoginRequestDto:**
- `Username` (string)
- `Password` (string)

**LoginResponseDto:**
- `Token` (string)
- `Expiration` (DateTime)

---

## Paso 8: Crear AuthService

Crear `IAuthService` y `AuthService` en `BacklogGames.Bussinnes.Layer/Services/`.

Revisar cómo los servicios existentes acceden a datos (¿directo por `AppDbContext`? ¿por UnitOfWork? ¿por Repository?) y seguir el mismo patrón.

Método `LoginAsync(LoginRequestDto)`:
1. Buscar usuario por username en la DB
2. Si no existe → retornar null
3. Verificar password con `BCrypt.Net.BCrypt.Verify()`
4. Si no coincide → retornar null
5. Generar JWT con claims: `NameIdentifier` (Id) y `Name` (Username)
6. Leer configuración JWT desde `IConfiguration`
7. Retornar `LoginResponseDto` con token y fecha de expiración

---

## Paso 9: Crear AuthController

Crear `AuthController.cs` en `BacklogGames/Controllers/`.

- Ruta: `api/auth`
- NO debe tener `[Authorize]`
- Inyectar `IAuthService`
- Endpoint: `POST login` que recibe `LoginRequestDto`
  - Si login exitoso → 200 con `LoginResponseDto`
  - Si credenciales inválidas → 401 con mensaje de error
- Seguir el mismo estilo y convenciones de los controllers existentes

---

## Paso 10: Configurar JWT Authentication en Program.cs

### Antes de `builder.Build()`:
- Configurar `AddAuthentication` con esquema `JwtBearer` por defecto
- Configurar `AddJwtBearer` con `TokenValidationParameters` leyendo los valores de la sección `Jwt` del appsettings
- Registrar `IAuthService` / `AuthService` en DI como Scoped

### Después de `app.UseRouting()` y ANTES de `app.MapControllers()`:
- Agregar `app.UseAuthentication()` **antes** de `app.UseAuthorization()`
- El orden es crítico: Authentication siempre antes de Authorization

---

## Paso 11: Proteger los controllers existentes

Agregar `[Authorize]` a TODOS los controllers existentes en `BacklogGames/Controllers/`, **excepto** `AuthController`.

---

## Paso 12: Configurar CORS (si no está configurado)

Verificar si CORS ya está configurado en `Program.cs`. Si no:
- Agregar política que permita el origen del frontend Angular (por defecto `http://localhost:4200`)
- Permitir cualquier header y método
- Aplicar la política en el pipeline

Si ya está configurado, verificar que permita los headers necesarios (especialmente `Authorization`).

---

## Verificación final

Después de implementar todo, verificar:

1. `POST /api/auth/login` con credenciales correctas → devuelve JWT
2. `POST /api/auth/login` con credenciales incorrectas → devuelve 401
3. `GET` a cualquier endpoint protegido sin token → devuelve 401
4. `GET` a cualquier endpoint protegido con header `Authorization: Bearer {token}` → funciona normalmente
5. Token expirado → devuelve 401

---

## Estructura de archivos nuevos a crear
W
```
BacklogGames.DataAccess.Layer/
├── Models/
│   └── User.cs
├── Configurations/
│   └── UserConfiguration.cs

BacklogGames.Bussinnes.Layer/
├── DTOs/
│   ├── LoginRequestDto.cs
│   └── LoginResponseDto.cs
├── Services/
│   ├── IAuthService.cs
│   └── AuthService.cs

BacklogGames/
├── Controllers/
│   └── AuthController.cs
```

## Archivos existentes a modificar

- `BacklogGames.DataAccess.Layer/Data/AppDbContext.cs` → Agregar `DbSet<User>`
- `BacklogGames/Program.cs` → Agregar config JWT, Authentication, Authorization, registrar AuthService en DI
- `BacklogGames/appsettings.json` → Agregar sección `Jwt`
- `BacklogGames/Controllers/*` → Agregar `[Authorize]` a todos los controllers existentes
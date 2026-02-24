# Plan de Implementación: Cambio de Password - Gaming Backlog API

## Contexto

Este plan es una extensión del auth existente (ver `AUTH-BACKEND-PLAN.md`). Se agrega un endpoint protegido para que el usuario logueado pueda cambiar su password. Requiere estar autenticado y conocer el password actual.

## Prerequisito

El auth con JWT debe estar implementado y funcionando (Pasos 1-12 del plan de auth).

---

## Paso 1: Crear DTO para cambio de password

Crear en `BacklogGames.Bussinnes.Layer/DTOs/`:

**ChangePasswordDto:**
- `CurrentPassword` (string, required)
- `NewPassword` (string, required)

---

## Paso 2: Agregar método en AuthService

Agregar a `IAuthService` y a `AuthService` un nuevo método `ChangePasswordAsync`.

Recibe el `ChangePasswordDto` y el `Id` del usuario (se obtiene del claim `NameIdentifier` del token JWT).

Lógica:
1. Buscar usuario por Id en la DB
2. Si no existe → retornar error
3. Verificar `CurrentPassword` contra el hash en DB con BCrypt
4. Si no coincide → retornar error indicando que el password actual es incorrecto
5. Hashear `NewPassword` con BCrypt
6. Actualizar `PasswordHash` en la DB
7. Guardar cambios
8. Retornar éxito

Seguir el mismo patrón de acceso a datos que usa el `LoginAsync` existente (UnitOfWork, Repository, o DbContext directo).

---

## Paso 3: Agregar endpoint en AuthController

Agregar en `AuthController`:

- Endpoint: `PUT change-password`
- Debe tener `[Authorize]`
- Recibe `ChangePasswordDto`
- Obtener el Id del usuario del token JWT (claim `NameIdentifier` del `HttpContext.User`)
- Llamar al método del servicio
- Si password actual incorrecto → 400 con mensaje de error
- Si éxito → 200 con mensaje de confirmación

---

## Verificación

1. `PUT /api/auth/change-password` sin token → 401
2. `PUT /api/auth/change-password` con token + password actual correcto + nuevo password → 200, password actualizado
3. `PUT /api/auth/change-password` con token + password actual incorrecto → 400
4. Después del cambio, login con password viejo → 401
5. Después del cambio, login con password nuevo → 200 + JWT

---

## Archivos nuevos

```
BacklogGames.Bussinnes.Layer/
├── DTOs/
│   └── ChangePasswordDto.cs
```

## Archivos a modificar

- `BacklogGames.Bussinnes.Layer/Services/IAuthService.cs` → Agregar método `ChangePasswordAsync`
- `BacklogGames.Bussinnes.Layer/Services/AuthService.cs` → Implementar `ChangePasswordAsync`
- `BacklogGames/Controllers/AuthController.cs` → Agregar endpoint `PUT change-password`

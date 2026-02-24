# Plan de Implementación: Auth Frontend Angular - Gaming Backlog App

## Contexto

Este plan implementa la autenticación en el frontend Angular del backlog de juegos. El backend ya tiene JWT implementado (ver `AUTH-BACKEND-PLAN.md`). Se necesita: página de login, servicio de auth, interceptor HTTP para enviar el token, y guard para proteger rutas.

Se adjunta una imagen de referencia para el diseño del login. Seguir ese diseño como base.

**IMPORTANTE:** En la imagen de referencia el formulario muestra un campo "Email". Ignorar eso. El campo debe ser **Username**, no Email. El backend espera `Username` y `Password` en el `LoginRequestDto`.

## Prerequisito

El backend con JWT debe estar implementado y funcionando.

## Arquitectura del proyecto Angular

```
FRONTEND/
├── src/
│   ├── app/
│   │   ├── components/          ← Componentes de la app
│   │   │   ├── add-game-dialog/
│   │   │   ├── complete-game-dialog/
│   │   │   ├── completed-games/
│   │   │   ├── game-card/
│   │   │   ├── home/
│   │   │   ├── list-card/
│   │   │   └── list-detail/
│   │   ├── core/
│   │   │   ├── interfaces/      ← Interfaces/modelos
│   │   │   └── services/        ← Servicios existentes
│   │   ├── layout/
│   │   │   ├── header/
│   │   │   └── sidenav/
│   │   ├── app.config.ts
│   │   ├── app.html
│   │   ├── app.routes.ts        ← Rutas
│   │   ├── app.scss
│   │   ├── app.ts
│   │   └── app.spec.ts
│   ├── environments/
│   ├── index.html
│   ├── main.ts
│   └── styles.scss              ← Estilos globales
├── angular.json
├── package.json
└── tsconfig.json
```

## Convenciones a respetar

- **Servicios** → `src/app/core/services/`
- **Interfaces/modelos** → `src/app/core/interfaces/`
- **Componentes de página** → `src/app/components/`
- **Layout (header, sidenav)** → `src/app/layout/`
- **Rutas** → `src/app/app.routes.ts`
- **Config de la app** → `src/app/app.config.ts`
- El guard y el interceptor van en `src/app/core/` (crear subcarpetas `guards/` e `interceptors/` si no existen, o ponerlos sueltos en `core/` según el estilo del proyecto)
- Respetar el estilo, convenciones y patrones de los archivos existentes

---

## Paso 1: Crear AuthService

Crear un servicio de autenticación en Angular.

Responsabilidades:
- Método `login(username, password)`: hace `POST` a `/api/auth/login` con `{ username, password }`
- Si el login es exitoso, guardar el token JWT y la fecha de expiración en `localStorage`
- Método `logout()`: eliminar token de `localStorage` y redirigir al login
- Método `isAuthenticated()`: verificar si hay token en `localStorage` y si no está expirado
- Método `getToken()`: retornar el token almacenado
- Método `changePassword(currentPassword, newPassword)`: hace `PUT` a `/api/auth/change-password`

---

## Paso 2: Crear HTTP Interceptor

Crear un interceptor que se aplique a todas las requests HTTP salientes.

Lógica:
- Si hay token en `localStorage`, agregar el header `Authorization: Bearer {token}` a la request
- Si la request recibe un 401, hacer logout y redirigir al login
- No agregar el header en requests al endpoint de login (es público)

Registrar el interceptor en la configuración de la app.

---

## Paso 3: Crear Auth Guard

Crear un guard para proteger las rutas de la aplicación.

Lógica:
- Si el usuario está autenticado (token válido y no expirado) → permitir acceso
- Si no está autenticado → redirigir a `/login`

Aplicar el guard a todas las rutas excepto `/login`.

---

## Paso 4: Crear página de Login

Crear un componente/página de login en la ruta `/login`.

**Diseño (ver imagen de referencia adjunta, con los cambios indicados abajo):**

- Layout dividido en dos mitades (split screen):
  - **Izquierda:** collage/grid de imágenes de portadas de juegos (decorativo)
  - **Derecha:** formulario de login centrado verticalmente

- Formulario:
  - Ícono de gamepad/controller arriba
  - Título: "Iniciar sesión"
  - Subtítulo: "Ingresa a tu cuenta para ver tu backlog"
  - Campo **Username** (NO email, cambiar respecto a la imagen de referencia)
  - Campo **Contraseña** con botón para mostrar/ocultar password (ojo)
  - Botón "Iniciar sesión →" (azul, ancho completo)

- Manejar estados:
  - Loading: deshabilitar botón mientras se hace la request
  - Error: mostrar mensaje si las credenciales son inválidas
  - Validación: campos requeridos

---

## Paso 5: Crear página de Cambio de Password

Crear un componente/página accesible desde la app (puede ser una ruta tipo `/change-password` o un modal/sección en settings).

Formulario:
- Campo **Contraseña actual**
- Campo **Nueva contraseña**
- Campo **Confirmar nueva contraseña**
- Botón "Cambiar contraseña"

Validaciones:
- Todos los campos requeridos
- Nueva contraseña y confirmación deben coincidir
- Mostrar mensaje de éxito o error según respuesta del backend

---

## Paso 6: Configurar rutas

- `/login` → componente de login (ruta pública, sin guard)
- Todas las demás rutas → protegidas con el Auth Guard
- `/change-password` → protegida con Auth Guard
- Si el usuario ya está logueado y entra a `/login` → redirigir a la página principal

---

## Paso 7: Agregar botón de logout

Agregar en algún lugar visible de la app (navbar, sidebar, menú, según el diseño actual):
- Botón o link de "Cerrar sesión"
- Al hacer click, llamar a `AuthService.logout()`

---

## Verificación final

1. Acceder a cualquier ruta sin estar logueado → redirige a `/login`
2. Login con credenciales correctas → redirige a la página principal, las requests llevan el token
3. Login con credenciales incorrectas → muestra mensaje de error
4. Cambio de password con datos correctos → éxito, el nuevo password funciona en el próximo login
5. Cambio de password con password actual incorrecto → muestra error
6. Botón de logout → limpia token, redirige a `/login`
7. Después de logout, no se puede acceder a rutas protegidas

---

## Archivos nuevos a crear

```
src/app/core/
├── services/
│   └── auth.service.ts
├── interfaces/
│   └── auth.interfaces.ts          ← LoginRequest, LoginResponse, ChangePasswordRequest
├── guards/
│   └── auth.guard.ts
└── interceptors/
    └── auth.interceptor.ts

src/app/components/
├── login/                           ← Página de login
└── change-password/                 ← Página de cambio de password
```

## Archivos existentes a modificar

- `src/app/app.routes.ts` → Agregar rutas de login y change-password, aplicar guard a las rutas existentes
- `src/app/app.config.ts` → Registrar el interceptor
- `src/app/layout/header/` o `src/app/layout/sidenav/` → Agregar botón de logout

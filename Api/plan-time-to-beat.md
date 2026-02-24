# Plan: Integrar Time To Beat de IGDB en BacklogGames

## Contexto

La API de BacklogGames ya consume IGDB para buscar juegos y guardarlos en la base de datos local. Se necesita agregar los datos de **tiempo para completar** un juego (time to beat) que provee IGDB a través del endpoint `game_time_to_beats`.

### Endpoint de IGDB a consumir

```
POST https://api.igdb.com/v4/game_time_to_beats
Headers:
  Client-ID: {clientId}
  Authorization: Bearer {accessToken}
Body:
  fields hastily, normally, completely, count; where game_id = {igdbId};
```

### Campos que devuelve (todos en segundos)

- **hastily**: Tiempo promedio para terminar solo la historia principal (Main Story)
- **normally**: Tiempo promedio mezclando historia + extras (Main + Extras)
- **completely**: Tiempo promedio para completar al 100% (Completionist)
- **count**: Cantidad de submissions/reportes de usuarios
- **game_id**: ID del juego en IGDB

> **Nota:** No todos los juegos tienen datos de time to beat. Si el endpoint devuelve una lista vacía, los campos deben quedar en `null`.

---

## Arquitectura del proyecto

```
BacklogGames (API Layer)
├── Controllers/
│   ├── GamesController.cs
│   └── UserListController.cs
│
BacklogGames.Bussinnes.Layer
├── DTOs/
│   ├── Game/
│   └── Igdb/
├── Services/
│   ├── GameService/
│   ├── IgdbService/
│   └── UserListService/
├── AutoMapperProfile.cs
│
BacklogGames.DataAccess.Layer
├── Data/AppDbContext.cs
├── Models/
│   ├── Game.cs
│   └── ...
├── Repositories/
│   ├── GameRepository/
│   └── ...
├── UnitOfWork/
├── Configurations/
└── Migrations/
```

---

## Pasos de implementación

### Paso 1: Modificar el modelo `Game.cs`

**Archivo:** `BacklogGames.DataAccess.Layer/Models/Game.cs`

Agregar las siguientes propiedades nullable al modelo `Game`:

- `HastilySeconds` (int?) — Tiempo main story en segundos
- `NormallySeconds` (int?) — Tiempo main + extras en segundos
- `CompletelySeconds` (int?) — Tiempo 100% en segundos
- `TimeToBeatCount` (int?) — Cantidad de submissions

Todos deben ser **nullable** (`int?`) porque no todos los juegos tienen esta data en IGDB.

---

### Paso 2: Crear la migración de EF Core

Generar y aplicar una nueva migración para agregar las 4 columnas a la tabla `Games`.

---

### Paso 3: Crear el DTO de respuesta de IGDB para Time To Beat

**Archivo nuevo:** `BacklogGames.Bussinnes.Layer/DTOs/Igdb/IgdbTimeToBeatResponseDto.cs`

Crear un DTO que mapee la respuesta JSON del endpoint `game_time_to_beats` de IGDB. Los nombres de las propiedades deben coincidir con los campos de la API (usar `JsonPropertyName` o `PropertyNameCaseInsensitive`):

- `GameId` (int) — mapeado de `game_id`
- `Hastily` (int?) — segundos
- `Normally` (int?) — segundos
- `Completely` (int?) — segundos
- `Count` (int?) — cantidad de submissions

---

### Paso 4: Agregar método en `IgdbService`

**Archivo:** `BacklogGames.Bussinnes.Layer/Services/IgdbService/IgdbService.cs`

Agregar un nuevo método público al servicio (y a su interfaz `IIgdbService`):

```
Task<IgdbTimeToBeatResponseDto?> GetTimeToBeatAsync(int igdbGameId)
```

**Lógica del método:**

1. Llamar a `EnsureAccessTokenAsync()` (ya existe en el servicio)
2. Construir el query: `fields hastily, normally, completely, count, game_id; where game_id = {igdbGameId};`
3. Hacer POST a `"game_time_to_beats"` (relativo al BaseAddress que ya está configurado como `https://api.igdb.com/v4/`)
4. Incluir los headers `Client-ID` y `Authorization: Bearer` (mismo patrón que `SearchGamesByNameAsync`)
5. Deserializar la respuesta como `List<IgdbTimeToBeatResponseDto>`
6. Retornar el **primer elemento** de la lista, o `null` si la lista está vacía

**Importante:** Seguir exactamente el mismo patrón de HttpRequestMessage que ya usa `SearchGamesByNameAsync`. No crear un HttpClient nuevo.

---

### Paso 5: Actualizar la interfaz `IIgdbService`

**Archivo:** `BacklogGames.Bussinnes.Layer/Services/IgdbService/IIgdbService.cs`

Agregar la firma del nuevo método:

```
Task<IgdbTimeToBeatResponseDto?> GetTimeToBeatAsync(int igdbGameId);
```

---

### Paso 6: Modificar `GameService.AddGameToListAsync`

**Archivo:** `BacklogGames.Bussinnes.Layer/Services/GameService/` (el servicio que contiene `AddGameToListAsync`)

En el bloque donde se crea un nuevo `Game` (cuando `existingGames == null`), **después de crear el objeto Game y antes de guardarlo**, agregar:

1. Llamar a `_igdbService.GetTimeToBeatAsync(dto.GameInfo.IgdbId)`
2. Si el resultado no es null, asignar los valores al objeto Game:
   - `game.HastilySeconds = timeToBeat.Hastily`
   - `game.NormallySeconds = timeToBeat.Normally`
   - `game.CompletelySeconds = timeToBeat.Completely`
   - `game.TimeToBeatCount = timeToBeat.Count`
3. Si el resultado es null, no hacer nada (los campos ya son nullable, quedan en null)

**Importante:** El `GameService` necesita recibir `IIgdbService` por inyección de dependencias en su constructor. Verificar que esté inyectado, y si no lo está, agregarlo.

---

### Paso 7: Actualizar `GameDto` para exponer los datos de Time To Beat

**Archivo:** `BacklogGames.Bussinnes.Layer/DTOs/Game/GameDto.cs`

Este DTO es el que se usa en el endpoint `GET {id}/with-games` para devolver los juegos de una lista. Actualmente tiene estos campos:

```csharp
public class GameDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GameStatusId { get; set; }
    public string GameStatusName { get; set; } = string.Empty;
    public string? CoverUrl { get; set; }
    public long? FirstReleaseDate { get; set; }
    public string? Summary { get; set; }
    public double? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

**Agregar** las siguientes propiedades nullable:

- `HastilySeconds` (int?) — Tiempo main story en segundos
- `NormallySeconds` (int?) — Tiempo main + extras en segundos
- `CompletelySeconds` (int?) — Tiempo 100% en segundos
- `TimeToBeatCount` (int?) — Cantidad de submissions

**Actualizar el perfil de AutoMapper** (`AutoMapperProfile.cs`) para que mapee estos nuevos campos de `Game` → `GameDto`. Si el mapeo ya usa `CreateMap<Game, GameDto>()` sin configuración manual, debería funcionar automáticamente siempre que los nombres de las propiedades coincidan entre el modelo y el DTO.

### Paso 7b: Verificar el flujo del endpoint `GET {id}/with-games`

**Archivo del controller:** `BacklogGames/Controllers/UserListController.cs`

El endpoint `GetGamesByListId` llama a `_userListService.GetGamesByListIdAsync(id)` que ya retorna los juegos de una lista. Al haber agregado los campos de time to beat al modelo `Game` y al `GameDto`, este endpoint **debería devolver la nueva info automáticamente** sin cambios adicionales, siempre y cuando:

1. El servicio `GetGamesByListIdAsync` haga el mapeo de `Game` → `GameDto` via AutoMapper
2. El mapeo de AutoMapper esté actualizado (paso anterior)

**Verificar** que el método `GetGamesByListIdAsync` en `UserListService` no esté haciendo un mapeo manual que excluya los nuevos campos. Si hace mapeo manual, agregar los 4 campos nuevos.

---

### Paso 8: Registrar dependencias (si aplica)

**Archivo:** `BacklogGames/Program.cs`

Verificar que `IIgdbService` esté registrado en el contenedor de DI. Si `GameService` no tenía esta dependencia antes, asegurarse de que la inyección funcione correctamente.

---

## Resumen de archivos a modificar/crear

| Acción | Archivo |
|--------|---------|
| **Modificar** | `DataAccess.Layer/Models/Game.cs` — agregar 4 propiedades nullable |
| **Crear** | Nueva migración de EF Core |
| **Crear** | `Bussinnes.Layer/DTOs/Igdb/IgdbTimeToBeatResponseDto.cs` |
| **Modificar** | `Bussinnes.Layer/Services/IgdbService/IIgdbService.cs` — agregar firma del método |
| **Modificar** | `Bussinnes.Layer/Services/IgdbService/IgdbService.cs` — agregar método `GetTimeToBeatAsync` |
| **Modificar** | `Bussinnes.Layer/Services/GameService/` — llamar a IGDB al crear Game nuevo |
| **Modificar** | `Bussinnes.Layer/DTOs/Game/GameDto.cs` — agregar 4 propiedades de time to beat |
| **Modificar** | `Bussinnes.Layer/AutoMapperProfile.cs` — verificar/actualizar mapeo Game → GameDto |
| **Verificar** | `Bussinnes.Layer/Services/UserListService/` — que el mapeo en `GetGamesByListIdAsync` incluya los nuevos campos |
| **Verificar** | `Program.cs` — DI del IIgdbService en GameService |

---

## Consideraciones

- **No bloquear el guardado si falla IGDB:** Envolver la llamada a `GetTimeToBeatAsync` en un try-catch. Si IGDB falla o está caído, el juego se guarda igual sin los datos de time to beat. Loguear el error.
- **Los tiempos vienen en segundos:** El frontend puede convertirlos a horas/minutos para mostrar (ej: `hastily / 3600` = horas).
- **Rate limit de IGDB:** 4 requests por segundo. Como esta llamada se hace solo al agregar un juego nuevo, no debería ser un problema.
- **No hace falta un endpoint nuevo en los controllers** para esta funcionalidad. Los datos se obtienen y persisten automáticamente al agregar un juego.

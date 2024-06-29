using Examen1.Dtos.TareasDto;
using Examen1.Database.Entities;
using Examen1.Services.interfaces;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Examen1.Services
{
    public class TareasServices : ITareaServices
    {
        public readonly string _JSON_FILE;

        public TareasServices()
        {
            _JSON_FILE = "SeedData/tareas.json";
        }

        public async Task<List<TareaDto>> GetTareasListAsync()
        {
            return await ReadTareaFromFileAsync();
        }

        public async Task<TareaDto> GetTareaByIdAsync(Guid id)
        {
            var tareas = await ReadTareaFromFileAsync();
            TareaDto tarea = tareas.FirstOrDefault(c => c.Id == id);
            return tarea;
        }

        public async Task<bool> CreateAsync(TareaCreateDto dto)
        {
            var tareas = await ReadTareaFromFileAsync(); // Leer las tareas actuales desde el archivo JSON

            var nuevaTarea = new TareaDto
            {
                Id = Guid.NewGuid(),  // Generar un nuevo Id único para la nueva tarea
                Descripcion = dto.Descripcion,
                Estado = dto.Estado,
                Prioridad = dto.Prioridad,
                TiempoHoras = dto.TiempoHoras,
            };

            tareas.Add(nuevaTarea);  // Agregar la nueva tarea a la lista de tareas

            var tareasEntities = tareas.Select(x => new TareaEntity
            {
                Id = x.Id,
                Descripcion = x.Descripcion,
                Estado = x.Estado,
                Prioridad = x.Prioridad,
                TiempoHoras = x.TiempoHoras,
            }).ToList();

            await WriteTareaToFileAsync(tareasEntities);  // Escribir las tareas actualizadas de vuelta al archivo JSON

            return true;
        }

        public async Task<bool> EditAsync(TareaEditDto dto, Guid id)
        {
            var tareasdtos = await ReadTareaFromFileAsync();

            var existingProduct = tareasdtos.FirstOrDefault(c => c.Id == id);

            if (existingProduct is null)
            {
                return false;
            }

            //TODO

            for (int i = 0; i < tareasdtos.Count; i++)
            {
                if (tareasdtos[i].Id == id)
                {
                    tareasdtos[i].Descripcion = dto.Descripcion;
                    tareasdtos[i].Estado = dto.Estado;
                    tareasdtos[i].Prioridad = dto.Prioridad;
                    tareasdtos[i].TiempoHoras = dto.TiempoHoras;
                }
            }

            var tareas = tareasdtos.Select(x => new TareaEntity
            {
                Id = Guid.NewGuid(),
                Descripcion = x.Descripcion,
                Estado = x.Estado,
                Prioridad = x.Prioridad,
                TiempoHoras = x.TiempoHoras,
            }).ToList();

            await WriteTareaToFileAsync(tareas);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tareasDto = await ReadTareaFromFileAsync();
            var tareaDelete = tareasDto.FirstOrDefault(x => x.Id == id);

            if (tareaDelete is null)
            {
                return false;
            }

            tareasDto.Remove(tareaDelete);

            var products = tareasDto.Select(x => new TareaEntity
            {
                Id = Guid.NewGuid(),
                Descripcion = x.Descripcion,
                Estado = x.Estado,
                Prioridad = x.Prioridad,
                TiempoHoras = x.TiempoHoras,
            }).ToList();

            await WriteTareaToFileAsync(products);

            return true;
        }
        private async Task<List<TareaDto>> ReadTareaFromFileAsync()
        {
            if (!File.Exists(_JSON_FILE))
            {
                return new List<TareaDto>();
            }

            var json = await File.ReadAllTextAsync(_JSON_FILE);

            var tareas = JsonConvert.DeserializeObject<List<TareaEntity>>(json);

            var dtos = tareas.Select(x => new TareaDto
            {
                Id = x.Id,
                Descripcion = x.Descripcion,
                Estado = x.Estado,
                Prioridad = x.Prioridad,
                TiempoHoras = x.TiempoHoras,
            }).ToList();

            return dtos;
        }

        private async Task WriteTareaToFileAsync(List<TareaEntity> tareas)
        {
            var json = JsonConvert.SerializeObject(tareas, Newtonsoft.Json.Formatting.Indented);

            await File.WriteAllTextAsync(_JSON_FILE, json);
        }

        public async Task<bool> ActualizarEstadoAsync(Guid id, string nuevoEstado)
        {
            var tareasdtos = await ReadTareaFromFileAsync();

            var tarea = tareasdtos.FirstOrDefault(t => t.Id == id);

            if (tarea == null)
            {
                return false; // Tarea no encontrada
            }

            tarea.Estado = nuevoEstado; // Actualizar solo el estado de la tarea

            // Convertir a entidades (si es necesario)
            var tareasEntities = tareasdtos.Select(t => new TareaEntity
            {
                Id = t.Id,
                Descripcion = t.Descripcion,
                Estado = t.Estado,
                Prioridad = t.Prioridad,
                TiempoHoras = t.TiempoHoras,
            }).ToList();

            await WriteTareaToFileAsync(tareasEntities); // Guardar los cambios en el archivo JSON

            return true;
        }


        public async Task<int> CalcularTiempoTotalEstimadoAsync()
        {
            var tareasdtos = await ReadTareaFromFileAsync();

            // Filtrar las tareas pendientes
            var tareasPendientes = tareasdtos.Where(t => t.Estado.ToLower() == "pendiente").ToList();

            // Calcular la suma del tiempo estimado
            int tiempoTotalEstimado = tareasPendientes.Sum(t => t.TiempoHoras);

            return tiempoTotalEstimado;
        }

    }
}

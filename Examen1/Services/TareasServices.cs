using Examen1.Dtos.TareasDto;
using Examen1.Database.Entities;
using Examen1.Services.interfaces;
using Newtonsoft.Json;

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
            var tareasdtos = await ReadTareaFromFileAsync();

            var tareadto = new TareaDto
            {
                Id = Guid.NewGuid(),
                Descripcion = dto.Descripcion,
                Estado = dto.Estado,
                Prioridad = dto.Prioridad,
                TiempoHoras = dto.TiempoHoras,
            };

            tareasdtos.Add(tareadto);

            var tareas = tareasdtos.Select(x => new TareaEntity
            {
                Id = Guid.NewGuid(),
                Descripcion = dto.Descripcion,
                Estado = dto.Estado,
                Prioridad = dto.Prioridad,
                TiempoHoras = dto.TiempoHoras,
            }).ToList();

            await WriteTareaToFileAsync(tareas);

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

    }
}

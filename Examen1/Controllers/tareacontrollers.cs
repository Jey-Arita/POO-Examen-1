using Examen1.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Examen1.Dtos.TareasDto;
using Examen1.Database.Entities;
using Examen1.Services;


namespace Examen1.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    public class tareacontrollers: ControllerBase
    {
        private readonly ITareaServices _tareasServices;

        public tareacontrollers(ITareaServices tareasServices)
        {
            this._tareasServices = tareasServices;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tareasServices.GetTareasListAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var tarea = await _tareasServices.GetTareaByIdAsync(id);

            if (tarea == null)
            {
                return NotFound(new { Message = $"No se encontro la tarea: {id}" });
            }
            return Ok(tarea);
        }


        [HttpPost]
        public async Task<ActionResult> Create(TareaCreateDto tarea)
        {

            await _tareasServices.CreateAsync(tarea);

            return StatusCode(201);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(TareaEditDto dto, Guid id)
        {
            var result = await _tareasServices.EditAsync(dto, id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(Guid id)
        {
            var category = await _tareasServices.GetTareaByIdAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            await _tareasServices.DeleteAsync(id);

            return Ok();
        }


        [HttpGet("tiempo-total-pendientes")]
        public async Task<ActionResult<int>> CalcularTiempoTotalPendientes()
        {
            try
            {
                var tiempoTotalEstimado = await _tareasServices.CalcularTiempoTotalEstimadoAsync();
                return Ok(tiempoTotalEstimado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al calcular el tiempo total estimado de tareas pendientes: {ex.Message}");
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Examen1.Dtos.TareasDto
{
    public class TareaDto
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string Prioridad { get; set; }
        public int TiempoHoras { get; set; }
    }

}

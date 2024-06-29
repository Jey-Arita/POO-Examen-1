using System.ComponentModel.DataAnnotations;

namespace Examen1.Database.Entities
{
    public class TareaEntity
    {
        //Tarea (ID, Descripción, Estado, Prioridad, Tiempo estimado en horas).
        public Guid Id { get; set; }

        [Display(Name = "Desciption")]
        [MinLength(10, ErrorMessage = "La {0} descripcion debe tener almenos {1} caracteres")]
        public string Descripcion { get; set; }

        public string Estado { get; set; }

        public string Prioridad { get; set; }

        public int TiempoHoras { get; set; }
    }
}

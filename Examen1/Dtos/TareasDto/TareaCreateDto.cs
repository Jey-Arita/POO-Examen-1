namespace Examen1.Dtos.TareasDto
{
    public class TareaCreateDto
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string Prioridad { get; set; }
        public int TiempoHoras { get; set; }
    }
}

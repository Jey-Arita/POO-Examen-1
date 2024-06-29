using Examen1.Dtos.TareasDto;

namespace Examen1.Services.interfaces
{
    public interface ITareaServices
    {
        Task<List<TareaDto>> GetTareasListAsync();
        Task<TareaDto> GetTareaByIdAsync(Guid id);
        Task<bool> CreateAsync(TareaCreateDto dto);
        Task<bool> EditAsync(TareaEditDto dto, Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
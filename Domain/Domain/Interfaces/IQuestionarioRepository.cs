namespace Domain.Interfaces;

using Domain.Entities;

public interface IQuestionarioRepository
{
    Task<Questionario?> GetByIdAsync(int id);
    Task<List<Questionario>> GetAllAsync();
    Task<List<Questionario>> GetAtivosAsync();
    Task<Questionario> AddAsync(Questionario questionario);
    Task UpdateAsync(Questionario questionario);
    Task DeleteAsync(int id);
}

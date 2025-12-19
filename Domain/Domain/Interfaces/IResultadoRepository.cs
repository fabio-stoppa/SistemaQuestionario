namespace Domain.Interfaces;

using Domain.Entities;

public interface IResultadoRepository
{
    Task<List<ResultadoSumarizado>> GetByQuestionarioIdAsync(int questionarioId);
    Task AddOrUpdateAsync(ResultadoSumarizado resultado);
    Task DeleteByPerguntaIdAsync(int perguntaId);
}

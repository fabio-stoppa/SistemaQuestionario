namespace Domain.Interfaces;

using Domain.Entities;

public interface IRespostaRepository
{
    Task<Resposta> AddAsync(Resposta resposta);
    Task<List<Resposta>> GetByQuestionarioIdAsync(int questionarioId);
    Task<int> GetTotalRespostasByPerguntaIdAsync(int perguntaId);
}

using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RespostaRepository : IRespostaRepository
{
    private readonly ApplicationDbContext _context;

    public RespostaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Resposta> AddAsync(Resposta resposta)
    {
        _context.Respostas.Add(resposta);
        await _context.SaveChangesAsync();
        return resposta;
    }

    public async Task<List<Resposta>> GetByQuestionarioIdAsync(int questionarioId)
    {
        return await _context.Respostas
            .Include(r => r.Pergunta)
            .Where(r => r.Pergunta.QuestionarioId == questionarioId)
            .ToListAsync();
    }

    public async Task<int> GetTotalRespostasByPerguntaIdAsync(int perguntaId)
    {
        return await _context.Respostas
            .Where(r => r.PerguntaId == perguntaId)
            .CountAsync();
    }
}

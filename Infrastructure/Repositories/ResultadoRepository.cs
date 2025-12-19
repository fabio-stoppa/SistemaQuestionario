using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ResultadoRepository : IResultadoRepository
{
    private readonly ApplicationDbContext _context;

    public ResultadoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ResultadoSumarizado>> GetByQuestionarioIdAsync(int questionarioId)
    {
        return await _context.ResultadosSumarizados
            .Include(r => r.Pergunta)
            .Include(r => r.Alternativa)
            .Where(r => r.Pergunta.QuestionarioId == questionarioId)
            .ToListAsync();
    }

    public async Task AddOrUpdateAsync(ResultadoSumarizado resultado)
    {
        var existing = await _context.ResultadosSumarizados
            .FirstOrDefaultAsync(r => r.PerguntaId == resultado.PerguntaId &&
                                    r.AlternativaId == resultado.AlternativaId);

        if (existing != null)
        {
            existing.TotalRespostas = resultado.TotalRespostas;
            existing.Percentual = resultado.Percentual;
            existing.DataProcessamento = resultado.DataProcessamento;
            _context.ResultadosSumarizados.Update(existing);
        }
        else
        {
            _context.ResultadosSumarizados.Add(resultado);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByPerguntaIdAsync(int perguntaId)
    {
        var resultados = await _context.ResultadosSumarizados
            .Where(r => r.PerguntaId == perguntaId)
            .ToListAsync();

        _context.ResultadosSumarizados.RemoveRange(resultados);
        await _context.SaveChangesAsync();
    }
}

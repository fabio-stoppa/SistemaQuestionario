using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuestionarioRepository : IQuestionarioRepository
{
    private readonly ApplicationDbContext _context;

    public QuestionarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Questionario?> GetByIdAsync(int id)
    {
        return await _context.Questionarios
            .Include(q => q.Perguntas.OrderBy(p => p.Ordem))
            .ThenInclude(p => p.Alternativas.OrderBy(a => a.Ordem))
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<List<Questionario>> GetAllAsync()
    {
        return await _context.Questionarios
            .Include(q => q.Perguntas)
            .ThenInclude(p => p.Alternativas)
            .OrderByDescending(q => q.DataCriacao)
            .ToListAsync();
    }

    public async Task<List<Questionario>> GetAtivosAsync()
    {
        var hoje = DateTime.Now;
        return await _context.Questionarios
            .Include(q => q.Perguntas.OrderBy(p => p.Ordem))
            .ThenInclude(p => p.Alternativas.OrderBy(a => a.Ordem))
            .Where(q => q.Ativo &&
                       (q.DataInicio == null || q.DataInicio <= hoje) &&
                       (q.DataFim == null || q.DataFim >= hoje))
            .OrderByDescending(q => q.DataCriacao)
            .ToListAsync();
    }

    public async Task<Questionario> AddAsync(Questionario questionario)
    {
        _context.Questionarios.Add(questionario);
        await _context.SaveChangesAsync();
        return questionario;
    }

    public async Task UpdateAsync(Questionario questionario)
    {
        _context.Questionarios.Update(questionario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var questionario = await _context.Questionarios.FindAsync(id);
        if (questionario != null)
        {
            _context.Questionarios.Remove(questionario);
            await _context.SaveChangesAsync();
        }
    }
}

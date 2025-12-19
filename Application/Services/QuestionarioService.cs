namespace Application.Services;

using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;

public class QuestionarioService
{
    private readonly IQuestionarioRepository _repository;
    private readonly ICacheService _cache;

    public QuestionarioService(IQuestionarioRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<List<QuestionarioDto>> GetAllAsync()
    {
        var questionarios = await _repository.GetAllAsync();
        return questionarios.Select(MapToDto).ToList();
    }

    public async Task<List<QuestionarioDto>> GetAtivosAsync()
    {
        var cacheKey = "questionarios:ativos";
        var cached = await _cache.GetAsync<List<QuestionarioDto>>(cacheKey);
        
        if (cached != null)
            return cached;

        var questionarios = await _repository.GetAtivosAsync();
        var result = questionarios.Select(MapToDto).ToList();
        
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    public async Task<QuestionarioDto?> GetByIdAsync(int id)
    {
        var cacheKey = $"questionario:{id}";
        var cached = await _cache.GetAsync<QuestionarioDto>(cacheKey);
        
        if (cached != null)
            return cached;

        var questionario = await _repository.GetByIdAsync(id);
        if (questionario == null)
            return null;

        var result = MapToDto(questionario);
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public async Task<QuestionarioDto> CreateAsync(CreateQuestionarioDto dto)
    {
        var questionario = new Questionario
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataCriacao = DateTime.Now,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            Ativo = true,
            Perguntas = dto.Perguntas.Select(p => new Pergunta
            {
                Texto = p.Texto,
                Tipo = Enum.Parse<TipoPergunta>(p.Tipo),
                Ordem = p.Ordem,
                Obrigatoria = p.Obrigatoria,
                Alternativas = p.Alternativas.Select(a => new Alternativa
                {
                    Texto = a.Texto,
                    Ordem = a.Ordem
                }).ToList()
            }).ToList()
        };

        var created = await _repository.AddAsync(questionario);
        await _cache.RemoveAsync("questionarios:ativos");
        
        return MapToDto(created);
    }

    private QuestionarioDto MapToDto(Questionario q)
    {
        return new QuestionarioDto
        {
            Id = q.Id,
            Titulo = q.Titulo,
            Descricao = q.Descricao,
            DataCriacao = q.DataCriacao,
            DataInicio = q.DataInicio,
            DataFim = q.DataFim,
            Ativo = q.Ativo,
            Perguntas = q.Perguntas.Select(p => new PerguntaDto
            {
                Id = p.Id,
                Texto = p.Texto,
                Tipo = p.Tipo.ToString(),
                Ordem = p.Ordem,
                Obrigatoria = p.Obrigatoria,
                Alternativas = p.Alternativas.Select(a => new AlternativaDto
                {
                    Id = a.Id,
                    Texto = a.Texto,
                    Ordem = a.Ordem
                }).ToList()
            }).ToList()
        };
    }
}

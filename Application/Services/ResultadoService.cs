namespace Application.Services;

using Domain.Interfaces;
using Application.DTOs;

public class ResultadoService
{
    private readonly IResultadoRepository _repository;
    private readonly IQuestionarioRepository _questionarioRepository;
    private readonly ICacheService _cache;

    public ResultadoService(
        IResultadoRepository repository,
        IQuestionarioRepository questionarioRepository,
        ICacheService cache)
    {
        _repository = repository;
        _questionarioRepository = questionarioRepository;
        _cache = cache;
    }

    public async Task<List<ResultadoDto>> GetResultadosAsync(int questionarioId)
    {
        var cacheKey = $"resultados:{questionarioId}";
        var cached = await _cache.GetAsync<List<ResultadoDto>>(cacheKey);
        
        if (cached != null)
            return cached;

        var questionario = await _questionarioRepository.GetByIdAsync(questionarioId);
        if (questionario == null)
            return new List<ResultadoDto>();

        var resultados = await _repository.GetByQuestionarioIdAsync(questionarioId);
        
        var resultadosDto = questionario.Perguntas.Select(p => new ResultadoDto
        {
            PerguntaId = p.Id,
            PerguntaTexto = p.Texto,
            TotalRespostas = resultados.Where(r => r.PerguntaId == p.Id).Sum(r => r.TotalRespostas),
            Alternativas = p.Alternativas.Select(a =>
            {
                var resultado = resultados.FirstOrDefault(r => r.AlternativaId == a.Id);
                return new ResultadoAlternativaDto
                {
                    AlternativaId = a.Id,
                    AlternativaTexto = a.Texto,
                    TotalRespostas = resultado?.TotalRespostas ?? 0,
                    Percentual = resultado?.Percentual ?? 0
                };
            }).ToList()
        }).ToList();

        await _cache.SetAsync(cacheKey, resultadosDto, TimeSpan.FromMinutes(2));
        return resultadosDto;
    }
}

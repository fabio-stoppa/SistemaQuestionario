using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Worker;

public class ProcessadorRespostasWorker : BackgroundService
{
    private readonly ILogger<ProcessadorRespostasWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageQueueService _messageQueue;

    public ProcessadorRespostasWorker(
        ILogger<ProcessadorRespostasWorker> logger,
        IServiceProvider serviceProvider,
        IMessageQueueService messageQueue)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _messageQueue = messageQueue;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker iniciado. Aguardando mensagens...");

        _messageQueue.Subscribe<RespostaMessage>("respostas-queue", async (message) =>
        {
            try
            {
                _logger.LogInformation($"Processando respostas do questionário {message.QuestionarioId}");
                await ProcessarResultadosAsync(message.QuestionarioId);
                _logger.LogInformation($"Questionário {message.QuestionarioId} processado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar questionário {message.QuestionarioId}");
            }
        });

        return Task.CompletedTask;
    }

    private async Task ProcessarResultadosAsync(int questionarioId)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var questionarioRepo = scope.ServiceProvider.GetRequiredService<IQuestionarioRepository>();
        var respostaRepo = scope.ServiceProvider.GetRequiredService<IRespostaRepository>();
        var resultadoRepo = scope.ServiceProvider.GetRequiredService<IResultadoRepository>();
        var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

        var questionario = await questionarioRepo.GetByIdAsync(questionarioId);
        if (questionario == null)
        {
            _logger.LogWarning($"Questionário {questionarioId} não encontrado");
            return;
        }

        foreach (var pergunta in questionario.Perguntas)
        {
            var totalRespostas = await respostaRepo.GetTotalRespostasByPerguntaIdAsync(pergunta.Id);

            if (pergunta.Tipo == TipoPergunta.MultiplaEscolha)
            {
                var respostasPergunta = await respostaRepo.GetByQuestionarioIdAsync(questionarioId);
                foreach (var alternativa in pergunta.Alternativas)
                {
                    var totalAlternativa = respostasPergunta
                        .Where(r => r.PerguntaId == pergunta.Id)
                        .Count(r => r.AlternativaId == alternativa.Id);

                    var percentual = totalRespostas > 0 
                        ? (decimal)totalAlternativa / totalRespostas * 100 
                        : 0;

                    var resultado = new ResultadoSumarizado
                    {
                        PerguntaId = pergunta.Id,
                        AlternativaId = alternativa.Id,
                        TotalRespostas = totalAlternativa,
                        Percentual = Math.Round(percentual, 2),
                        DataProcessamento = DateTime.Now
                    };

                    await resultadoRepo.AddOrUpdateAsync(resultado);
                }
            }
            else
            {
                var resultado = new ResultadoSumarizado
                {
                    PerguntaId = pergunta.Id,
                    AlternativaId = null,
                    TotalRespostas = totalRespostas,
                    Percentual = 100, // Total of text responses is always 100% of text responses
                    DataProcessamento = DateTime.Now
                };

                await resultadoRepo.AddOrUpdateAsync(resultado);
            }
        }

        // Invalidar cache
        await cache.RemoveAsync($"resultados:{questionarioId}");
        
        _logger.LogInformation($"Resultados do questionário {questionarioId} atualizados");
    }
}

public class RespostaMessage
{
    public int QuestionarioId { get; set; }
    public DateTime DataProcessamento { get; set; }
}

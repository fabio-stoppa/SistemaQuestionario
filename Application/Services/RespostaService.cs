namespace Application.Services;

using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;

public class RespostaService
{
    private readonly IRespostaRepository _repository;
    private readonly IMessageQueueService _messageQueue;

    public RespostaService(IRespostaRepository repository, IMessageQueueService messageQueue)
    {
        _repository = repository;
        _messageQueue = messageQueue;
    }

    public async Task SubmitRespostasAsync(SubmitRespostasDto dto, string? ipAddress)
    {
        foreach (var respostaDto in dto.Respostas)
        {
            var resposta = new Resposta
            {
                PerguntaId = respostaDto.PerguntaId,
                AlternativaId = respostaDto.AlternativaId,
                TextoResposta = respostaDto.TextoResposta,
                DataResposta = DateTime.Now,
                IpAddress = ipAddress
            };

            await _repository.AddAsync(resposta);
        }

        // Publicar mensagem para processamento assíncrono
        await _messageQueue.PublishAsync("respostas-queue", new
        {
            QuestionarioId = dto.QuestionarioId,
            DataProcessamento = DateTime.Now
        });
    }
}

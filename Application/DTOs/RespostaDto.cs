namespace Application.DTOs;

public class RespostaDto
{
    public int PerguntaId { get; set; }
    public int? AlternativaId { get; set; }
    public string? TextoResposta { get; set; }
}

public class SubmitRespostasDto
{
    public int QuestionarioId { get; set; }
    public List<RespostaDto> Respostas { get; set; } = new();
}

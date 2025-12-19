namespace Domain.Entities;

public class Resposta
{
    public int Id { get; set; }
    public int PerguntaId { get; set; }
    public int? AlternativaId { get; set; }
    public string? TextoResposta { get; set; }
    public DateTime DataResposta { get; set; }
    public string? IpAddress { get; set; }
    public Pergunta Pergunta { get; set; } = null!;
    public Alternativa? Alternativa { get; set; }
}

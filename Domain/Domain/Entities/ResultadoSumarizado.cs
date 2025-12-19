namespace Domain.Entities;

public class ResultadoSumarizado
{
    public int Id { get; set; }
    public int PerguntaId { get; set; }
    public int? AlternativaId { get; set; }
    public int TotalRespostas { get; set; }
    public decimal Percentual { get; set; }
    public DateTime DataProcessamento { get; set; }
    public Pergunta Pergunta { get; set; } = null!;
    public Alternativa? Alternativa { get; set; }
}

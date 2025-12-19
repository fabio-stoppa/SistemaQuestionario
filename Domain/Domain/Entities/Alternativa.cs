namespace Domain.Entities;

public class Alternativa
{
    public int Id { get; set; }
    public int PerguntaId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public Pergunta Pergunta { get; set; } = null!;
}

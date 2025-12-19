namespace Domain.Entities;

public class Pergunta
{
    public int Id { get; set; }
    public int QuestionarioId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public TipoPergunta Tipo { get; set; }
    public int Ordem { get; set; }
    public bool Obrigatoria { get; set; }
    public Questionario Questionario { get; set; } = null!;
    public List<Alternativa> Alternativas { get; set; } = new();
    public List<Resposta> Respostas { get; set; } = new();
}

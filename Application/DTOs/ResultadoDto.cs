namespace Application.DTOs;

public class ResultadoDto
{
    public int PerguntaId { get; set; }
    public string PerguntaTexto { get; set; } = string.Empty;
    public int TotalRespostas { get; set; }
    public List<ResultadoAlternativaDto> Alternativas { get; set; } = new();
}

public class ResultadoAlternativaDto
{
    public int AlternativaId { get; set; }
    public string AlternativaTexto { get; set; } = string.Empty;
    public int TotalRespostas { get; set; }
    public decimal Percentual { get; set; }
}

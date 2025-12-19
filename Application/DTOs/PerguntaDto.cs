namespace Application.DTOs;

public class PerguntaDto
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool Obrigatoria { get; set; }
    public List<AlternativaDto> Alternativas { get; set; } = new();
}

public class CreatePerguntaDto
{
    public string Texto { get; set; } = string.Empty;
    public string Tipo { get; set; } = "MultiplaEscolha";
    public int Ordem { get; set; }
    public bool Obrigatoria { get; set; }
    public List<CreateAlternativaDto> Alternativas { get; set; } = new();
}

namespace Application.DTOs;

public class AlternativaDto
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int Ordem { get; set; }
}

public class CreateAlternativaDto
{
    public string Texto { get; set; } = string.Empty;
    public int Ordem { get; set; }
}

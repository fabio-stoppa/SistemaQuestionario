using System.Net.Http.Json;
using Application.DTOs;

namespace Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<QuestionarioDto>> GetQuestionariosAtivosAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<QuestionarioDto>>("api/questionarios/ativos") 
               ?? new List<QuestionarioDto>();
    }

    public async Task<List<QuestionarioDto>> GetQuestionariosAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<QuestionarioDto>>("api/questionarios") 
               ?? new List<QuestionarioDto>();
    }

    public async Task<QuestionarioDto?> GetQuestionarioByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<QuestionarioDto>($"api/questionarios/{id}");
    }

    public async Task<QuestionarioDto?> CreateQuestionarioAsync(CreateQuestionarioDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/questionarios", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<QuestionarioDto>();
    }

    public async Task SubmitRespostasAsync(SubmitRespostasDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/respostas", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ResultadoDto>> GetResultadosAsync(int questionarioId)
    {
        return await _httpClient.GetFromJsonAsync<List<ResultadoDto>>($"api/resultados/{questionarioId}") 
               ?? new List<ResultadoDto>();
    }
}

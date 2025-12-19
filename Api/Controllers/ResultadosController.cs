using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultadosController : ControllerBase
{
    private readonly ResultadoService _service;

    public ResultadosController(ResultadoService service)
    {
        _service = service;
    }

    [HttpGet("{questionarioId}")]
    public async Task<ActionResult<List<ResultadoDto>>> GetResultados(int questionarioId)
    {
        var resultados = await _service.GetResultadosAsync(questionarioId);
        return Ok(resultados);
    }
}

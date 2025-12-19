using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RespostasController : ControllerBase
{
    private readonly RespostaService _service;

    public RespostasController(RespostaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitRespostasDto dto)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        await _service.SubmitRespostasAsync(dto, ipAddress);
        return Ok(new { message = "Respostas enviadas com sucesso!" });
    }
}

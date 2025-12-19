using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionariosController : ControllerBase
{
    private readonly QuestionarioService _service;

    public QuestionariosController(QuestionarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<QuestionarioDto>>> GetAll()
    {
        var questionarios = await _service.GetAllAsync();
        return Ok(questionarios);
    }

    [HttpGet("ativos")]
    public async Task<ActionResult<List<QuestionarioDto>>> GetAtivos()
    {
        var questionarios = await _service.GetAtivosAsync();
        return Ok(questionarios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionarioDto>> GetById(int id)
    {
        var questionario = await _service.GetByIdAsync(id);
        if (questionario == null)
            return NotFound();

        return Ok(questionario);
    }

    [HttpPost]
    public async Task<ActionResult<QuestionarioDto>> Create([FromBody] CreateQuestionarioDto dto)
    {
        var questionario = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = questionario.Id }, questionario);
    }
}

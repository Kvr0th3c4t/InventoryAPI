using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CategoriaController : ControllerBase
{
    private readonly CategoriaService _categoriaService;

    public CategoriaController(CategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public ActionResult<List<Categoria>> GetAll()
    {
        var categorias = _categoriaService.GetAll();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public ActionResult<Categoria> GetCategoriaById(int id)
    {
        var categoria = _categoriaService.GetCategoriaById(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult<Categoria> Create([FromBody] CreateCategoriaDto dto)
    {
        try
        {
            var categoria = _categoriaService.Create(dto);
            return CreatedAtAction(nameof(GetCategoriaById), new { id = categoria.Id }, categoria);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public ActionResult<Categoria> UpdateCategoria(int id, [FromBody] UpdateCategoriaDto dto)
    {
        try
        {
            var categoria = _categoriaService.Update(id, dto);

            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCategoria(int id)
    {
        bool eliminado = _categoriaService.Delete(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}
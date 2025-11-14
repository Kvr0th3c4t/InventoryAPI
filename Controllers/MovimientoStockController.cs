using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Services;
using InventoryAPI.Dtos.MovimientoStockDtos;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosStockController : ControllerBase
{
    private readonly MovimientoStockService _movimientoStock;

    public MovimientosStockController(MovimientoStockService movimientoStock)
    {
        _movimientoStock = movimientoStock;
    }

    [HttpGet]
    public ActionResult<List<MovimientoStockResponseDto>> GetAll()
    {
        var movimientos = _movimientoStock.GetAll();
        return Ok(movimientos);
    }

    [HttpGet("{id}")]
    public ActionResult<MovimientoStockResponseDto> GetMovimientoById(int id)
    {
        var movimiento = _movimientoStock.GetMovimientoById(id);

        if (movimiento == null)
            return NotFound();

        return Ok(movimiento);
    }

    [HttpPost]
    public ActionResult<MovimientoStockResponseDto> Create([FromBody] CreateMovimientoStockDto dto)
    {
        try
        {
            var movimiento = _movimientoStock.Create(dto);
            return CreatedAtAction(nameof(GetMovimientoById), new { id = movimiento.Id }, movimiento);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)  
        {
            return BadRequest(ex.Message);  
        }
    }

}
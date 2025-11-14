using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProveedorController : ControllerBase
{
    private readonly ProveedorService _proveedorService;

    public ProveedorController(ProveedorService proveedorService)
    {
        _proveedorService = proveedorService;
    }

    [HttpGet]
    public ActionResult<List<ProveedorResponseDto>> GetAll()
    {
        var proveedores = _proveedorService.GetAll();
        return Ok(proveedores);
    }

    [HttpGet("{id}")]
    public ActionResult<ProveedorResponseDto> GetProveedorById(int id)
    {
        var proveedor = _proveedorService.GetProveedorById(id);

        if (proveedor == null)
            return NotFound();

        return Ok(proveedor);
    }

    [HttpPost]
    public ActionResult<ProveedorResponseDto> Create([FromBody] CreateProveedorDto dto)
    {
        try
        {
            var proveedor = _proveedorService.Create(dto);
            return CreatedAtAction(nameof(GetProveedorById), new { id = proveedor.Id }, proveedor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public ActionResult<ProveedorResponseDto> UpdateProveedor(int id, [FromBody] UpdateProveedorDto dto)
    {
        try
        {
            var proveedor = _proveedorService.Update(id, dto);

            if (proveedor == null)
                return NotFound();

            return Ok(proveedor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteProveedor(int id)
    {
        try
        {
            bool eliminado = _proveedorService.Delete(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
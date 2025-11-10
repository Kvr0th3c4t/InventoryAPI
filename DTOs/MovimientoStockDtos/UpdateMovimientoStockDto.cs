namespace InventoryAPI.Dtos.MovimientoStockDtos;

using InventoryAPI.Enums;

public class UpdateMovimientoStockDto
{
    public int? ProveedorId { get; set; }
    public TipoMovimiento? Tipo { get; set; }
    public int? Cantidad { get; set; }
    public string? Razon { get; set; }
}
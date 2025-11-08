namespace InventoryAPI.Dtos.ProductoDtos;

public class CreateProductoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int StockActual { get; set; }
    public int StockMinimo { get; set; }
    public decimal Precio { get; set; }
}
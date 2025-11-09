namespace InventoryAPI.Dtos.ProductoDtos;

public class UpdateProductoDto
{
    public string? Nombre { get; set; }
    public string? SKU { get; set; }
    public int? CategoriaId { get; set; }
    public string? Descripcion { get; set; }
    public int? StockActual { get; set; }
    public int? StockMinimo { get; set; }
    public decimal? Precio { get; set; }

}
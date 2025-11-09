namespace InventoryAPI.Dtos.ProductoDtos;

public class ResponseProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public int StockActual { get; set; }
    public decimal Precio { get; set; }
}
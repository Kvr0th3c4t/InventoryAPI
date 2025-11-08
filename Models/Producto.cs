namespace InventoryAPI.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int StockActual { get; set; }
    public int StockMinimo { get; set; }
    public decimal Precio { get; set; }
    public DateTime FechaCreacion { get; set; }
}


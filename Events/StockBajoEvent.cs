namespace InventoryAPI.Events;

public class StockBajoEvent
{
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public int StockActual { get; set; }
    public int StockMinimo { get; set; }
    public DateTime FechaEvento { get; set; }
}
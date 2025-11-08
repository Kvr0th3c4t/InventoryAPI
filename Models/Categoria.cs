using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public ICollection<Producto>? Productos { get; set; }
}
namespace InventoryAPI.Dtos.ProveedorDtos;

public class CreateProveedorDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }

}
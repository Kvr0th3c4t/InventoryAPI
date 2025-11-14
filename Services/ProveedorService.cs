using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Microsoft.Identity.Client;

namespace InventoryAPI.Services;

public class ProveedorService
{
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IMovimientoStockRepository _movimientoStockRepository;

    public ProveedorService(IProveedorRepository proveedorRepository, IProductoRepository productoRepository, IMovimientoStockRepository movimientoStockRepository)
    {
        _proveedorRepository = proveedorRepository;
        _productoRepository = productoRepository;
        _movimientoStockRepository = movimientoStockRepository;
    }

    public List<ProveedorResponseDto> GetAll()
    {
        var proveedores = _proveedorRepository.GetAll();

        return proveedores
            .Select(proveedor => MapToResponseDto(proveedor))
            .ToList();
    }

    public ProveedorResponseDto? GetProveedorById(int id)
    {
        var proveedor = _proveedorRepository.GetById(id);

        if (proveedor == null)
        {
            return null;
        }

        return MapToResponseDto(proveedor);
    }

    public ProveedorResponseDto Create(CreateProveedorDto dto)
    {
        var proveedor = new Proveedor
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Telefono = dto.Telefono
        };

        var proveedorCreado = _proveedorRepository.Add(proveedor);

        var response = MapToResponseDto(proveedorCreado);

        return response;
    }

    public ProveedorResponseDto? Update(int id, UpdateProveedorDto dto)
    {
        var proveedor = _proveedorRepository.GetById(id);

        if (proveedor == null) return null;

        if (dto.Nombre != null) proveedor.Nombre = dto.Nombre;
        if (dto.Email != null) proveedor.Email = dto.Email;
        if (dto.Telefono != null) proveedor.Telefono = dto.Telefono;

        var proveedorModificado = _proveedorRepository.Update(proveedor);

        var response = MapToResponseDto(proveedorModificado);

        return response;
    }

    public bool Delete(int id)
    {
        var proveedor = _proveedorRepository.GetById(id);


        if (proveedor == null)
            return false;

        var tieneProductos = _productoRepository.GetAll()
            .Any(p => p.ProveedorId == id);

        if (tieneProductos)
        {
            throw new InvalidOperationException(
                "No se puede eliminar: el proveedor tiene productos asociados");
        }


        var tieneMovimientos = _movimientoStockRepository.GetAll()
            .Any(m => m.ProveedorId == id);


        if (tieneMovimientos)
        {
            throw new InvalidOperationException(
                "No se puede eliminar: el proveedor tiene movimientos de stock asociados");
        }

        return _proveedorRepository.Delete(id);
    }

    private ProveedorResponseDto MapToResponseDto(Proveedor proveedor)
    {
        return new ProveedorResponseDto
        {
            Id = proveedor.Id,
            Nombre = proveedor.Nombre,
            Email = proveedor.Email,
            Telefono = proveedor.Telefono
        };
    }
}
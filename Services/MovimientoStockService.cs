using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Enums;
using InventoryAPI.Events;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace InventoryAPI.Services;

public class MovimientoStockService
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IEventPublisher _eventPublisher;

    public MovimientoStockService(IMovimientoStockRepository movimientoStockRepository, IProductoRepository productoRepository, IProveedorRepository proveedorRepository, IEventPublisher eventPublisher)
    {
        _movimientoStockRepository = movimientoStockRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
        _eventPublisher = eventPublisher;
    }
    public List<MovimientoStockResponseDto> GetAll()
    {
        var movimientos = _movimientoStockRepository.GetAll();

        return movimientos
            .Select(movimiento => MapToResponseDto(movimiento))
            .ToList();
    }

    public MovimientoStockResponseDto? GetMovimientoById(int id)
    {
        var movimiento = _movimientoStockRepository.GetById(id);

        if (movimiento == null)
        {
            return null;
        }

        return MapToResponseDto(movimiento);
    }

    public MovimientoStockResponseDto Create(CreateMovimientoStockDto dto)
    {
        var productoExiste = _productoRepository.GetById(dto.ProductoId);

        if (productoExiste == null)
        {
            throw new InvalidOperationException("No existe el producto");
        }
        if (dto.Tipo == TipoMovimiento.Entrada)
        {
            if (!dto.ProveedorId.HasValue)
                throw new ArgumentException("Para movimientos de entrada es obligatorio el ProveedorId");

            var proveedorExiste = _proveedorRepository.GetById(dto.ProveedorId.Value);
            if (proveedorExiste == null)
                throw new InvalidOperationException("El proveedor no existe");
        }

        if (dto.Cantidad <= 0)  // ← También cambié < por <=, la cantidad debe ser mayor a 0
        {
            throw new ArgumentException("La cantidad debe ser mayor a 0");
        }

        if (dto.Tipo == TipoMovimiento.Salida || dto.Tipo == TipoMovimiento.AjusteNegativo)
        {
            if (productoExiste.StockActual < dto.Cantidad)
            {
                throw new InvalidOperationException(
                    $"Stock insuficiente. Stock actual: {productoExiste.StockActual}, " +
                    $"cantidad solicitada: {dto.Cantidad}");
            }
        }

        switch (dto.Tipo)
        {
            case TipoMovimiento.Entrada:
            case TipoMovimiento.AjustePositivo:
                productoExiste.StockActual += dto.Cantidad;
                break;

            case TipoMovimiento.Salida:
            case TipoMovimiento.AjusteNegativo:
                productoExiste.StockActual -= dto.Cantidad;
                break;
        }

        _productoRepository.Update(productoExiste);

        if (productoExiste.StockActual < productoExiste.StockMinimo)
        {
            var evento = new StockBajoEvent
            {
                ProductoId = productoExiste.Id,
                ProductoNombre = productoExiste.Nombre,
                StockActual = productoExiste.StockActual,
                StockMinimo = productoExiste.StockMinimo,
                FechaEvento = DateTime.Now
            };

            _eventPublisher.Publish(evento);
        }

        var movimiento = new MovimientoStock
        {
            ProductoId = dto.ProductoId,
            ProveedorId = dto.ProveedorId,
            Tipo = dto.Tipo,
            Cantidad = dto.Cantidad,
            Razon = dto.Razon,
            FechaMovimiento = DateTime.Now
        };

        var movimientoCreado = _movimientoStockRepository.Add(movimiento);

        var response = MapToResponseDto(movimientoCreado);

        return response;
    }


    private MovimientoStockResponseDto MapToResponseDto(MovimientoStock movimiento)
    {
        var producto = _productoRepository.GetById(movimiento.ProductoId);
        var proveedor = movimiento.ProveedorId.HasValue
            ? _proveedorRepository.GetById(movimiento.ProveedorId.Value)
            : null;

        return new MovimientoStockResponseDto
        {
            Id = movimiento.Id,
            ProductoId = movimiento.ProductoId,
            ProductoNombre = producto?.Nombre,
            ProveedorId = movimiento.ProveedorId,
            ProveedorNombre = proveedor?.Nombre,
            Tipo = movimiento.Tipo,
            Cantidad = movimiento.Cantidad,
            Fecha = movimiento.FechaMovimiento,
            Razon = movimiento.Razon
        };
    }

}
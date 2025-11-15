using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;

namespace InventoryAPI.Services;

public class CategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public List<Categoria> GetAll()
    {
        return _repository.GetAll();
    }

    public Categoria? GetCategoriaById(int id)
    {
        return _repository.GetById(id);
    }

    public Categoria Create(CreateCategoriaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CategoriaNombre))
            throw new ArgumentException("La categoría debe tener un nombre");

        var categoria = new Categoria
        {
            Nombre = dto.CategoriaNombre,
            Descripcion = dto.Descripcion
        };

        _repository.Add(categoria);

        return categoria;
    }

    public Categoria? Update(int id, UpdateCategoriaDto dto)
    {
        var categoria = _repository.GetById(id);

        if (categoria == null)
            return null;

        if (dto.Nombre != null) categoria.Nombre = dto.Nombre;
        if (dto.Descripcion != null) categoria.Descripcion = dto.Descripcion;

        _repository.Update(categoria);

        return categoria;
    }

    public bool Delete(int id)
    {
        return _repository.Delete(id);
    }


}
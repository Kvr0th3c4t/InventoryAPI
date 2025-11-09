namespace InventoryAPI.Services.Interfaces;

public interface IEventPublisher
{
    void Publish<T>(T evento) where T : class;
}
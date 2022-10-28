using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.Interfaces;

public interface IUpdateInventoryUseCase
{
    Task ExecuteAsync(Inventory inventory);
}
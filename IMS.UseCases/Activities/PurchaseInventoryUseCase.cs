using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;
        private readonly IInventoryRepository inventoryRepository;

        public PurchaseInventoryUseCase(IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository)
        {
            this.inventoryTransactionRepository = inventoryTransactionRepository;
            this.inventoryRepository = inventoryRepository;
        }
        public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
        {
            //insert a record in the transaction table
            inventoryTransactionRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

            //update inventory with increased quantity
            inventory.Quantity += quantity;
            await this.inventoryRepository.UpdateInventoryAsync(inventory);
        }
    }
}

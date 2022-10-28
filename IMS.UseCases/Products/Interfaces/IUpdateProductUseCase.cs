using IMS.CoreBusiness;

namespace IMS.UseCases.Products.Interfaces
{
    public interface IUpdateProductUseCase
    {
        Task ExecuteAsync(Product product);
    }
}
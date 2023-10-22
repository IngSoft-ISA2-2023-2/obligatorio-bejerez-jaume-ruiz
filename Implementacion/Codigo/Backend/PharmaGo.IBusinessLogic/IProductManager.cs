using ExportationModel.ExportDomain;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;

namespace PharmaGo.IBusinessLogic
{
    public interface IProductManager
    {
        Product Create(Product product, string token);
        void Delete(int id);
        IEnumerable<Product> GetAll(ProductSearchCriteria productSearchCriteria);
        Product Update(int id, Product product);
    }
}

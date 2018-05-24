using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Infrastructure;

namespace ProductControl.BLL.Interfaces
{
    public interface IProductService
    {
        OperationResult CreateProduct(ProductDTO productDto);
        ProductDTO GetProductById(int? id);
        ProductDTO GetProductByName(string name);
        OperationResult EditProduct (ProductDTO productDto);
        List<ProductDTO> GetAllProducts();
        void Dispose();
    }
}

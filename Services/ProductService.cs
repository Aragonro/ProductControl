using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductControl.BLL.Interfaces;
using ProductControl.Dal.Interfaces;
using ProductControl.BLL.DTO;
using ProductControl.Dal.Entities;
using Microsoft.AspNet.Identity;
using ProductControl.BLL.Infrastructure;

namespace ProductControl.BLL.Services
{
    public class ProductService : IProductService
    {
        IUnitOfWork Database { get; set; }

        public ProductService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationResult CreateProduct(ProductDTO productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product is null");
            }
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, Product>()).CreateMapper();
            //Product product = mapper.Map<ProductDTO, Product>(productDto);
            Product product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price
            };
            product.Sensors = new List<Sensor>();
            Database.Products.Create(product);
            Database.Save();
            return new OperationResult("Product was created");
        }

        public ProductDTO GetProductById(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Product id is null");
            }

            Product product = Database.Products.Get(id.Value);
            if (product == null)
            {
                throw new Exception("Product is not found");
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
            ProductDTO productDTO = mapper.Map<Product, ProductDTO>(product);
            return productDTO;
        }

        public ProductDTO GetProductByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Product name is null");
            }

            Product product = Database.Products.Find(i=>i.Name==name).FirstOrDefault();
            if (product == null)
            {
                throw new Exception("Product is not found");
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
            ProductDTO productDTO = mapper.Map<Product, ProductDTO>(product);
            return productDTO;
        }

        public List<ProductDTO> GetAllProducts()
        {


            List<Product> products = Database.Products.GetAll().ToList();
            List<ProductDTO> productsDTO = new List<ProductDTO>();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
            foreach (var product in products)
            {
               productsDTO.Add(mapper.Map<Product, ProductDTO>(product));
            }
            return productsDTO;
        }

        public OperationResult EditProduct(ProductDTO productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product is null");
            }

            Product product = Database.Products.Get(productDto.Id);

            if (product == null)
            {
                throw new Exception("Product is not found");

            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, Product>()).CreateMapper();
            product = mapper.Map<ProductDTO, Product>(productDto);
            Database.Products.Update(product);
            Database.Save();
            return new OperationResult("Product was edited");
        }

        public void Dispose()
        {
            Database.Dispose();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Interfaces;
using ProductControl.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProductControl.Controllers
{
    public class ProductController : Controller
    {
        private IProductService productrService;
        private IUserService userService;

        public ProductController(IProductService service, IUserService userService)
        {
            productrService = service;
            this.userService = userService;
        }

        [HttpGet]
        public JsonResult Details(int? id)
        {
            var product = productrService.GetProductById(id);
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var product = productrService.GetAllProducts();
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Create(ProductModel model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            if (!Validator.TryValidateObject(model, context, results, true))
            {
                return Json("Bad data");
            }

            ApplicationUserDTO checkUser = new ApplicationUserDTO
            {
                Email = model.Email,
                SecurityStamp = model.SecurityStamp
            };
            try
            {
                ApplicationUserDTO user = await userService.GetUserByEmailAndSecurityStamp(checkUser);
                if (user.Role != "admin")
                {
                    return Json("Only Admin can add product");
                }
                else
                {
                    ProductDTO product = new ProductDTO { Name = model.Name, Price = model.Price };
                    return Json(productrService.CreateProduct(product).Result);
                }
            }
            catch { 
                return Json("Email or Token is wrong");
            }
;
        }

        [HttpPost]
        public JsonResult Edit(ProductDTO product)
        {
            return Json(productrService.EditProduct(product).Result);
        }
    }
}
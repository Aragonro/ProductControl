using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Interfaces;
using ProductControl.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ProductControl.Controllers
{
    public class OrderController :Controller
    {
        private IOrderService orderService;
        private IUserService userService;
        private IProductService productService;
        private ISensorService sensorService;
        private IMonitoringService monitoringService;

        public OrderController(IOrderService service, IUserService userService, IProductService productService, ISensorService sensorService, IMonitoringService monitoringService)
        {
            orderService = service;
            this.userService=userService;
            this.productService = productService;
            this.sensorService = sensorService;
            this.monitoringService = monitoringService;
        }

        public JsonResult Details(int? id)
        {
            var order = orderService.GetOrderById(id);
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAllOrdersForAdmin(CheckModel model)
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
                    return Json("Only Admin can get orders");
                }
                else
                {
                    List<GetEmptyOrder> orders = new List<GetEmptyOrder>();
                    List<OrderDTO> ordersDTO = orderService.GetOrdersWithoutCourier().ToList();
                    foreach(var orderDTO in ordersDTO)
                    {
                        SensorDTO sensor;
                        try
                        {
                            sensor = sensorService.GetSensorById(orderDTO.SensorId);
                        }
                        catch
                        {
                            return Json("Bad with get sensor");
                        }
                        MonitoringDTO monitoringDTo;
                        try
                        {
                            monitoringDTo = monitoringService.GetMonitoringsBySensorId(sensor.Id);
                        }
                        catch
                        {
                            return Json("Bad with get monitoring");
                        }
                        ApplicationUserDTO customerUser;
                        try
                        {
                            customerUser = await userService.GetUserById(monitoringDTo.ApplicationUserId);
                        }
                        catch
                        {
                            return Json("Bad with get user");
                        }
                        ProductDTO productDTO;
                        try
                        {
                            productDTO = productService.GetProductById(sensor.ProductId);
                        }
                        catch
                        {
                            return Json("Bad with get product");
                        }
                        GetEmptyOrder order = new GetEmptyOrder
                        {
                            CountProduct=sensor.CountProduct,
                            CustomerEmail = customerUser.Email,
                            DeliveryAddress = orderDTO.DeliveryAddress,
                            Price=orderDTO.Price,
                            ProductName=productDTO.Name,
                            Id=orderDTO.Id
                        };
                        orders.Add(order);

                    }
                   
                    return Json(orders, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Email or Token is wrong");
            }
        }
        public async Task<JsonResult> GetAllOrdersForObserver(CheckModel model)
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
                if (user.Role != "user")
                {
                    return Json("Only User can get sensors");
                }
                else
                {
                    try
                    {
                        List<OrderDTO> orders = orderService.GetOrdersDelivering().ToList();
                        return Json(orders, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json("Bad with get orders");
                    }
                }
            }
            catch
            {
                return Json("Email or Token is wrong");
            }
        }
        [HttpPost]
        public async Task<JsonResult> SetCourier(SetCourierModel model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            if (!Validator.TryValidateObject(model, context, results, true))
            {
                return Json("Bad data");
            }

            ApplicationUserDTO checkUser = new ApplicationUserDTO
            {
                Email = model.EmailAdmin,
                SecurityStamp = model.SecurityStamp
            };
            try
            {
                ApplicationUserDTO user = await userService.GetUserByEmailAndSecurityStamp(checkUser);
                if (user.Role != "admin")
                {
                    return Json("Only Admin can set courier");
                }
                else
                {
                    ApplicationUserDTO courier;
                    try
                    {
                        courier = await userService.GetUserByEmail(model.CourierEmail);
                    }
                    catch
                    {
                        return Json("Bad with get courier");
                    }
                    if (courier.Role != "courier")
                    {
                        return Json("Only courier can delivery products");
                    }
                    OrderDTO order = orderService.GetOrderById(model.Id);
                    order.DeliveryDate = model.DeliveryDate;
                    order.ApplicationUserId = courier.Id;
                    try
                    {
                        return Json(orderService.EditOrder(order).Result, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json("Bad with edit order");
                    }
                }
            }
            catch
            {
                return Json("Email or Token is wrong");
            }
        }

        [HttpPost]
        public JsonResult Create(OrderDTO order)
        {
            return Json(orderService.CreateOrder(order).Result);
        }
    }
}

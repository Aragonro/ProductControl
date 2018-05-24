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
    public class SensorController : Controller
    {
        private ISensorService sensorService;
        private IUserService userService;
        private IProductService productService;
        private IMonitoringService monitoringService;
        private IOrderService orderService;

        public SensorController(ISensorService service, IUserService userService, IProductService productService, IMonitoringService monitoringService, IOrderService orderService)
        {
            sensorService = service;
            this.userService = userService;
            this.productService = productService;
            this.monitoringService = monitoringService;
            this.orderService = orderService;
        }

        public JsonResult Details(int? id)
        {
            var sensor = sensorService.GetSensorById(id);
            return Json(sensor, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSensorsByUserEmailAndSecurityStamp(CheckModel model)
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
                    return Json("Only User can have sensors");
                }
                else
                {
                    var sensors = sensorService.GetSensorsByUserId(user.Id).ToList();
                    List<GetSensor> getSensors = new List<GetSensor>();
                    foreach(var sensor in sensors)
                    {
                        var mapper = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<SensorDTO, GetSensor>()).CreateMapper();
                        GetSensor getSensor = mapper.Map<SensorDTO, GetSensor>(sensor);
                        ProductDTO product = productService.GetProductById(sensor.ProductId);
                        getSensor.ProductName = product.Name;
                        MonitoringDTO monitoring = monitoringService.GetMonitoringsBySensorId(sensor.Id);
                        ApplicationUserDTO observer = await userService.GetUserById(monitoring.ApplicationUserId);
                        getSensor.ObserverEmail = observer.Email;
                        getSensors.Add(getSensor);
                    }
                    return Json(getSensors, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Email or Token is wrong");
            }
        }
        

        public async Task<JsonResult> GetSensorById(SensorId model)
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
                    return Json("Only User can have sensors");
                }
                else
                {
                    var sensor = sensorService.GetSensorById(model.Id);
                    if (sensor.ApplicationUserId != user.Id)
                    {
                        return Json("You doesn't have access to this sensor");
                    }
                    var mapper = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<SensorDTO, GetSensor>()).CreateMapper();
                    GetSensor getSensor = mapper.Map<SensorDTO, GetSensor>(sensor);
                    ProductDTO product = productService.GetProductById(sensor.ProductId);
                    getSensor.ProductName = product.Name;
                    MonitoringDTO monitoring = monitoringService.GetMonitoringsBySensorId(sensor.Id);
                    ApplicationUserDTO observer = await userService.GetUserById(monitoring.ApplicationUserId);
                    getSensor.ObserverEmail = observer.Email;

                    return Json(getSensor, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Email or Token is wrong");
            }
        }

        public JsonResult GetSensorsWithoutUser()
        {
            var sensors = sensorService.GetSensorsWithoutUser();
            return Json(sensors, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Create(AddSensorModel model)
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
                    return Json("Only Admin can add sensor");
                }
               
                else
                {
                    try
                    {
                        ApplicationUserDTO owner = await userService.GetUserByEmail(model.Email );
                        try
                        {
                            ProductDTO product = productService.GetProductByName(model.Product);
                            SensorDTO sensor = new SensorDTO
                            {
                                Name = model.Name,
                                IsWorking = false,
                                IsProduct = false,
                                CountProduct = 1,
                                DeliveryAddress = model.DeliveryAddress,
                                AutoDelivery = false,
                                ApplicationUserId = owner.Id,
                                ProductId =product.Id
                            };

                            return Json(sensorService.CreateSensor(sensor).Result);
                        }
                        catch
                        {
                            return Json("Product is not exists");
                        }
                    }
                    catch
                    {
                        return Json("Wrang Email");
                    }
                }
            }
            catch
            {
                return Json("Email or Token is wrong");
            }
        }

        [HttpPost]
        public JsonResult Edit(SensorDTO sensor)
        {
            return Json(sensorService.EditSensor(sensor).Result);
        }
        [HttpPost]
        public JsonResult SensorIsEmpty(int? id)
        {
            if (id == null)
            {
                return Json("Need id Sensor");
            } 
            try
            {
                SensorDTO sensorDTO = sensorService.GetSensorById(id.Value);
                if (!sensorDTO.IsProduct)
                {
                    return Json("Sensor has not worked");
                }
                sensorDTO.IsProduct = false;
                if (sensorDTO.AutoDelivery)
                {
                    ProductDTO productDTO;
                    try
                    {
                        productDTO = productService.GetProductById(sensorDTO.ProductId);
                    }
                    catch
                    {
                        return Json("Bad with get product");
                    }
                    OrderDTO orderDto = new OrderDTO
                    {
                        Delivered = false,
                        DeliveryAddress = sensorDTO.DeliveryAddress,
                        Price = productDTO.Price * sensorDTO.CountProduct,
                        SensorId = sensorDTO.Id,
                        DeliveryDate = DateTime.Now
                    };
                    try
                    {
                        orderService.CreateOrder(orderDto);
                    }
                    catch
                    {
                        return Json("Bad with Create order");
                    }
                }
                try
                {
                    return Json(sensorService.EditSensor(sensorDTO).Result);
                }
                catch
                {
                    return Json("Bad with Edit Sensor");
                }
            }
            catch
            {
                return Json("Bad with get Sensor");
            }
        }
        [HttpPost]
        public JsonResult SensorIsNotEmpty(int? id)
        {
            if (id == null)
            {
                return Json("Need id Sensor");
            }
            try
            {
                SensorDTO sensorDTO = sensorService.GetSensorById(id.Value);
                if (sensorDTO.IsProduct)
                {
                    return Json("Sensor has worked");
                }
                sensorDTO.IsProduct = true;
                try
                {
                    return Json(sensorService.EditSensor(sensorDTO).Result);
                }
                catch
                {
                    return Json("Bad with Edit Sensor");
                }
            }
            catch
            {
                return Json("Bad with get Sensor");
            }
        }
        [HttpPost]
        public async Task<JsonResult> SiteEdit(EditSensorModel model)
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
                if (user.Role != "user")
                {
                    return Json("Only User can have sensors");
                }
                else
                {
                    try
                    {
                        var sensor = sensorService.GetSensorById(model.Id);
                        if (sensor.ApplicationUserId != user.Id)
                        {
                            return Json("You doesn't have access to this sensor");
                        }
                        try
                        {
                            ApplicationUserDTO userObserver = await userService.GetUserByEmail(model.ObserverEmail);
                            try
                            {
                                MonitoringDTO monitoring = monitoringService.GetMonitoringsBySensorId(sensor.Id);
                                monitoring.ApplicationUserId = userObserver.Id;
                                monitoringService.EditMonitoring(monitoring);
                                try
                                {
                                    sensor.IsWorking = model.IsWorking;
                                    sensor.AutoDelivery = model.AutoDelivery;
                                    sensor.DeliveryAddress = model.DeliveryAddress;
                                    sensor.Name = model.Name;
                                    sensor.CountProduct = model.CountProduct;
                                    return Json(sensorService.EditSensor(sensor).Result, JsonRequestBehavior.AllowGet);
                                }
                                catch
                                {
                                    return Json("Bad with update Sensor", JsonRequestBehavior.AllowGet);
                                }
                            }
                            catch
                            {
                                return Json("Bad with update Monitoring");
                            }
                        }
                        catch
                        {
                            return Json("Observer Email is wrong");
                        }
                    }
                    catch
                    {
                        return Json("Wrong Sensor Id");
                    }
                   
                }
            }
            catch
            {
                return Json("Email or Token is wrong");
            }
        }
    }
}
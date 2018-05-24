using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Interfaces;
using System.Threading.Tasks;
using ProductControl.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductControl.Controllers
{
    public class MonitoringController : Controller
    {
        private IMonitoringService monitoringService;
        private ISensorService sensorService;
        private IUserService userService;
        private IProductService productService;

        public MonitoringController(IMonitoringService service, ISensorService sensorService, IUserService userService, IProductService productService)
        {
            monitoringService = service;
            this.sensorService = sensorService;
            this.userService = userService;
            this.productService = productService;
        }

        public JsonResult Details(int? id)
        {
            var monitorng = monitoringService.GetMonitoringById(id);
            return Json(monitorng, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetMonitoringsByUserEmailAndSecurityStamp(CheckModel model)
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
                    return Json("Only User can monitoring", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    try
                    {
                        var monitorngs = monitoringService.GetMonitoringsByUserId(user.Id).ToList();
                        List<GetSensor> getSensors = new List<GetSensor>();
                        foreach (var monitoring in monitorngs)
                        {
                            try
                            {
                                SensorDTO sensor = sensorService.GetSensorById(monitoring.SensorId);
                                try
                                {
                                    ApplicationUserDTO ownerUser = await userService.GetUserById(sensor.ApplicationUserId);
                                    try
                                    {
                                        var mapper = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<SensorDTO, GetSensor>()).CreateMapper();
                                        GetSensor getSensor = mapper.Map<SensorDTO, GetSensor>(sensor);
                                        ProductDTO product = productService.GetProductById(sensor.ProductId);
                                        getSensor.ProductName = product.Name;
                                        getSensor.ObserverEmail = ownerUser.Email;
                                        getSensors.Add(getSensor);
                                    }
                                    catch
                                    {
                                        return Json("Bad with get product", JsonRequestBehavior.AllowGet);
                                    }
                                }
                                catch
                                {
                                    return Json("Bad with get owner user", JsonRequestBehavior.AllowGet);
                                }
                            }
                            catch
                            {
                                return Json("Bad with get sensor", JsonRequestBehavior.AllowGet);
                            }
                        }
                        return Json(getSensors, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json("Bad with monitorings", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json("Email or Token is wrong", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(MonitoringDTO monitoring)
        {
            return Json(monitoringService.CreateMonitoring(monitoring).Result);
        }

        [HttpPost]
        public JsonResult Edit(MonitoringDTO monitoring)
        {
            return Json(monitoringService.EditMonitoring(monitoring).Result);
        }
    }
}
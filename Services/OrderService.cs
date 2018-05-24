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
    public class OrderService : IOrderService
    {
        IUnitOfWork Database { get; set; }

        public OrderService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationResult CreateOrder(OrderDTO orderDto)
        {
            if (orderDto == null)
            {
                throw new ArgumentNullException(nameof(orderDto), "Order is null");
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, Order>()).CreateMapper();
            Order order = mapper.Map<OrderDTO, Order>(orderDto);
            Database.Orders.Create(order);
            Database.Save();
            return new OperationResult("Order was created");
        }
        public OrderDTO GetOrderById(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Order id is null");
            }

            Order order = Database.Orders.Get(id.Value);
            if (order == null)
            {
                throw new Exception("Order is not found");
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()).CreateMapper();
            OrderDTO orderDTO = mapper.Map<Order, OrderDTO>(order);
            return orderDTO;
        }
        public IEnumerable<OrderDTO> GetOrdersWithoutCourier()
        {
            List<OrderDTO> ordersDTO = new List<OrderDTO>();
            var orders = Database.Orders.Find(i => i.ApplicationUserId == null).ToList();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()).CreateMapper();
            foreach (var order in orders)
            {
                OrderDTO orderDTO = mapper.Map<Order, OrderDTO>(order);
                ordersDTO.Add(orderDTO);
            }
            return ordersDTO;
        }
        public IEnumerable<OrderDTO> GetOrdersDelivering()
        {
            List<OrderDTO> ordersDTO = new List<OrderDTO>();
            var orders = Database.Orders.Find(i => i.ApplicationUserId != null && !i.Delivered).ToList();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()).CreateMapper();
            foreach (var order in orders)
            {
                OrderDTO orderDTO = mapper.Map<Order, OrderDTO>(order);
                ordersDTO.Add(orderDTO);
            }
            return ordersDTO;
        }
        public OperationResult EditOrder(OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                throw new ArgumentNullException(nameof(orderDTO), "Order is null");
            }

            Order order = Database.Orders.Get(orderDTO.Id);

            if (order == null)
            {
                throw new Exception("Order is not found");

            }
            order.ApplicationUserId = orderDTO.ApplicationUserId;
            order.Delivered = orderDTO.Delivered;
            order.DeliveryAddress = orderDTO.DeliveryAddress;
            order.DeliveryDate = orderDTO.DeliveryDate;
            order.Price = orderDTO.Price;
            order.SensorId = orderDTO.SensorId;
            Database.Orders.Update(order);
            Database.Save();
            return new OperationResult("Order was edited");
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}

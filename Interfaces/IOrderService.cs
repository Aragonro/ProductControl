using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductControl.BLL.DTO;
using ProductControl.BLL.Infrastructure;

namespace ProductControl.BLL.Interfaces
{
    public interface IOrderService
    {
        OperationResult CreateOrder(OrderDTO orderDto);
        OrderDTO GetOrderById(int? id);
        IEnumerable<OrderDTO> GetOrdersWithoutCourier();
        OperationResult EditOrder(OrderDTO orderDTO);
        IEnumerable<OrderDTO> GetOrdersDelivering();
        void Dispose();
    }
}

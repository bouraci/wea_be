using EFModels.Enums;
using WEA_BE.DTO;

namespace WEA_BE.Services
{
    public interface IOrderService
    {
        bool AddOrder(string username, List<int> bookIds, PaymentMethod paymentMethod);
        List<OrderDto> GetOrders(string username);
    }
}
using EFModels.Enums;

namespace WEA_BE.DTO;

public class OrderAddRequest
{
    public List<int> bookIds { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}

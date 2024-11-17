using WEA_BE.DTO;

namespace WEA_BE.Services;

public interface IUserService
{
    UserDetailDto? GetUserDetail(string userName);
    bool UpdateUser(string userName, string? address, string? billingAddress, bool? processData, bool? isMale, int? age, List<string> FavouriteGerners, string? refferal);
}
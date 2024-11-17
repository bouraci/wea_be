using AutoMapper;
using EFModels.Data;
using WEA_BE.DTO;

namespace WEA_BE.Services;

public class UserService : IUserService
{
    private readonly DatabaseContext _ctx;
    private readonly IMapper _mapper;
    public UserService(DatabaseContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }
    public bool UpdateUser(string userName, string? address, string? billingAddress, bool? processData, bool? isMale, int? age, List<string> FavouriteGerners, string? refferal)
    {
        var user = _ctx.Users.SingleOrDefault(x => x.UserName == userName);
        if (user == null) return false;
        user.Address = address;
        user.BillingAddress = billingAddress;
        user.ProcessData = processData;
        user.IsMale = isMale;
        if (age is not null)
        {
            if (age < 0) return false;
        }
        user.Age = age;
        user.FavouriteGerners = FavouriteGerners.Any() ? string.Join(',', FavouriteGerners) : null;
        user.Refferal = refferal;
        _ctx.SaveChanges();
        return true;
    }

    public UserDetailDto? GetUserDetail(string userName)
    {
        var user = _ctx.Users.SingleOrDefault(x => x.UserName == userName);
        if (userName is null) return null;
        var dto = _mapper.Map<UserDetailDto>(user);
        return dto;
    }
}

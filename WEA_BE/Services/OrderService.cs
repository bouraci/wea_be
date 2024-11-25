﻿using AutoMapper;
using EFModels.Data;
using EFModels.Enums;
using EFModels.Models;
using Microsoft.EntityFrameworkCore;
using WEA_BE.DTO;

namespace WEA_BE.Services;

public class OrderService : IOrderService
{
    private readonly DatabaseContext _ctx;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;
    public OrderService(DatabaseContext ctx, IMapper mapper, IAuditService auditService)
    {
        _ctx = ctx;
        _mapper = mapper;
        _auditService = auditService;
    }
    private bool CanUserOrder(User user)
    {
        if (user.Address is not null &&
               user.BillingAddress is not null &&
               user.ProcessData.HasValue &&
               user.IsMale.HasValue &&
               user.BirthDay.HasValue)
        {
            return (bool)user.ProcessData;
        }
        return false;
    }
    public bool AddOrder(string username, List<int> bookIds)
    {
        var user = _ctx.Users.AsQueryable().Include(x => x.Address).Include(x => x.BillingAddress).SingleOrDefault(x => x.UserName == username);
        if (user is null) return false;
        if (!CanUserOrder(user)) return false;
        List<Book> books = new();
        foreach (int id in bookIds)
        {
            var book = _ctx.Books.SingleOrDefault(x => x.Id == id);
            if (book is not null && book.Price is not null && !book.IsHidden)
            {
                books.Add(book);
            }
        }
        double totalprice = books.Sum(x => (double)x.Price);
        var order = new Order()
        {
            User = user,
            Books = books,
            Created = DateTime.Now,
            totalPrice = totalprice
        };
        _ctx.Orders.Add(order);
        _ctx.SaveChanges();


        _auditService.LogAudit("", _mapper.Map<OrderDto>(order), LogType.AddOrder, user.UserName);

        return true;
    }

    public List<OrderDto> GetOrders(string username)
    {
        var user = _ctx.Users.SingleOrDefault(x => x.UserName == username);
        if (user is null) return null;
        List<Order> orders = _ctx.Orders.AsQueryable().Include(x => x.Books).Include(x => x.User).Include(x => x.Books).Where(x => x.User == user).ToList();
        var dtos = _mapper.Map<List<OrderDto>>(orders);
        return dtos;
    }
}
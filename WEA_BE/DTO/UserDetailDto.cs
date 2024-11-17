﻿namespace WEA_BE.DTO;

public class UserDetailDto
{
    public AddressDto? Address { get; set; }
    public AddressDto? BillingAddress { get; set; }
    public bool? ProcessData { get; set; }
    public bool? IsMale { get; set; } //Genesis 1:27
    public int? Age { get; set; }
    public List<string> FavouriteGerners { get; set; }
    public string? Referral { get; set; }
}
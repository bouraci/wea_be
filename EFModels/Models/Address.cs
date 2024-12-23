﻿using System.ComponentModel.DataAnnotations;

namespace EFModels.Models;

public class Address
{
    [Key]
    public int Id { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Intex.Models.Cart;
namespace Intex.Models;
public partial class Orders
{
    [BindNever] 
    public int? TransactionId { get; set; }

    [BindNever]
    public ICollection<CartLine> Lines { get; set; }
            = new List<CartLine>();

    [Required]
    public int? CustomerId { get; set; }
    [Required]
    public DateTime? Date { get; set; }
    [Required]
    public string? DayOfWeek { get; set; }
    [Required]
    public byte? Time { get; set; }
    [Required]
    public string? EntryMode { get; set; }
    [Required]
    public short? Amount { get; set; }
    [Required]
    public string? TypeOfTransaction { get; set; }
    [Required]
    public string? CountryOfTransaction { get; set; }
    [Required]
    public string? ShippingAddress { get; set; }
    [Required]
    public string? Bank { get; set; }
    [Required]
    public string? TypeOfCard { get; set; }
    public string? Fraud { get; set; }
}
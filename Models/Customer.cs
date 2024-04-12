using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class Customer
{
    public short? CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? CountryOfResidence { get; set; }

    public string? Gender { get; set; }

    public float? Age { get; set; }
    public string? Email { get; set; }
}

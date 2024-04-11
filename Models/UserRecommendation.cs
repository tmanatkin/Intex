using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class UserRecommendation
{
    public short? CustomerId { get; set; }
    public byte? ProductId { get; set; }
    public byte? Rating { get; set; }
    public byte? RecId1 { get; set; }
    public string? RecName1 { get; set; }
    public string? RecImg1 { get; set; }
    public byte? RecId2 { get; set; }
    public string? RecName2 { get; set; }
    public string? RecImg2 { get; set; }
    public byte? RecId3 { get; set; }
    public string? RecName3 { get; set; }
    public string? RecImg3 { get; set; }
    public byte? RecId4 { get; set; }
    public string? RecName4 { get; set; }
    public string? RecImg4 { get; set; }
    public byte? RecId5 { get; set; }
    public string? RecName5 { get; set; }
    public string? RecImg5 { get; set; }

}

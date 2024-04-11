using CsvHelper;
using System.Collections.Generic;
using System.Formats.Asn1;
using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class ItemRecommendation
{
        public byte? ProductId { get; set; }
        public string? ProductName { get; set; }
        public byte? Rec1Id { get; set; }
        public string? Rec1Name { get; set; }
        public byte? Rec2Id { get; set; }
        public string? Rec2Name { get; set; }
        public byte? Rec3Id { get; set;}
        public string? Rec3Name { get; set;}

}
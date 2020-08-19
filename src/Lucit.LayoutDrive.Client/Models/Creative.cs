using System;
using Newtonsoft.Json;

namespace Lucit.LayoutDrive.Client.Models
{
    public class Creative
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Src { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

        [JsonProperty("weight_pct")]
        public decimal WeightPercentage { get; set; }

        [JsonProperty("creative_datetime")]
        public DateTime DateTime { get; set; }
    }
}
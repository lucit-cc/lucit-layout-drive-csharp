using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lucit.LayoutDrive.Client.Models
{
    public class LucitLocation
    {
        [JsonProperty("location_id")]
        public string Id { get; set; }

        [JsonProperty("location_name")]
        public string Name { get; set; }

        [JsonProperty("lucit_layout_digital_board_id")]
        public string DigitalBoardId { get; set; }

        public List<Creative> Items { get; set; } = new List<Creative>();
    }
}
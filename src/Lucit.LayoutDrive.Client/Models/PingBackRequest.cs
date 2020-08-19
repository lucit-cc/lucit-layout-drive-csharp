using System;

namespace Lucit.LayoutDrive.Client.Models
{
    public class PingBackRequest
    {
        public int ItemId { get; set; }
        public string LocationId { get; set; }
        public DateTime PlayDateTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
﻿using System;

namespace Lucit.LayoutDrive.Client.Models
{
    public class PingBackRequest
    {
        public int ItemId { get; set; }

        /// <summary>
        /// The location id (digital display id)
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// The play date time from your system in UTC
        /// </summary>
        public DateTime PlayDateTime { get; set; }

        /// <summary>
        /// The duration that the item was displayed for
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
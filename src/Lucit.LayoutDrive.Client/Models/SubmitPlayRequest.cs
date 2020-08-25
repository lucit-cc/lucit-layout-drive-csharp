using System;

namespace Lucit.LayoutDrive.Client.Models
{
    public class SubmitPlayRequest
    {
        public string CreativeId { get; set; }

        /// <summary>
        /// The lucit_layout_digital_board_id that the creative ran on
        /// </summary>
        public int DigitalBoardId { get; set; }

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
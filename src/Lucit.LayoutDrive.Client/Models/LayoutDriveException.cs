using System;

namespace Lucit.LayoutDrive.Client.Models
{
    public class LayoutDriveException : Exception
    {
        public string RawResponse { get; set; }
        public LayoutDriveException(string message)
        : base(message)
        {
        }


        public LayoutDriveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
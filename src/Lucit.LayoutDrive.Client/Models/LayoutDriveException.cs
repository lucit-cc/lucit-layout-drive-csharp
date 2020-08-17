using System;

namespace Lucit.LayoutDrive.Client.Models
{
    public class LayoutDriveException : Exception
    {
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
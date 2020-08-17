using System;

namespace Lucit.LayoutDrive.Client.Models
{
    internal class ResponseWrapper<T>
    {
        public ResponseWrapper()
        {
        }

        public bool IsSuccess => Exception == null;


        public Exception Exception { get; set; }

        public T Data { get; set; }
    }
}
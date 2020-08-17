using System;
using System.Collections.Generic;
using System.Linq;

namespace Lucit.LayoutDrive.Client.Extensions
{
    internal static class StringExtensions
    {
        public static string Serialize(this Dictionary<string, string> data)
        {
            return string.Join("&", data.Select(x => $"{Uri.EscapeDataString(x.Key ?? string.Empty)}={Uri.EscapeDataString(x.Value ?? string.Empty)}"));

        }
    }
}
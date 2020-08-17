using System;

namespace Lucit.LayoutDrive.Client.Constants
{
    internal static class Routes
    {
        public static readonly Func<int, string> Creatives = exportId => $"/inventory-exports/{exportId}";
        
        public const string PingBack = "/analytics/track/lucit-drive-pingback";
    }
}
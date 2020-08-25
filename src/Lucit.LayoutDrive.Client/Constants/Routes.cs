using System;

namespace Lucit.LayoutDrive.Client.Constants
{
    internal static class Routes
    {
        public static readonly Func<string, string> Creatives = exportId => $"/inventory-exports/{exportId}/pull";
        
        public const string PingBack = "/analytics/track/lucit-drive-pingback";
        public const string Play = "/analytics/track/lucit-drive-play";
    }
}
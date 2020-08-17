namespace Lucit.LayoutDrive.Client.Models
{
    public class CreativesFilter
    {
        public int ExportId { get; set; }
        public int? LocationId { get; set; }

        public CreativesFilter(int exportId)
        {
            ExportId = exportId;
        }
    }
}
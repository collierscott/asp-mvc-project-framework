using Project.Domain.Models.Entities;

namespace Project.Application.Models.Demos
{

    public class SeriesItem : Entity<string>
    {

        public string Name { get; set; }
        public string FacilityId { get; set; }
        public int NumLots { get; set; }
        public int NumDie { get; set; }
        public int NumWafers { get; set; }
        public string WaferUnit { get; set; }
        public string DieUnit { get; set; }
        public string LotUnit { get; set; }
        public string Category { get; set; }
        public int CategorySortOrder { get; set; }
        public double Value { get; set; }
        public string SeriesColor { get; set; }
        public int SortOrder { get; set; }
         
    }

}
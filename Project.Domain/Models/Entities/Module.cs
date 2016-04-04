using System.Collections.Generic;

namespace Project.Domain.Models.Entities {

    public class Module : Entity<string>
    {

        public Facility Facility { get; set; }

        public ICollection<EquipmentFamily> EquipmentFamilies { get; set; }

        public string FacilityId { get; set; }
        public string Name { get; set; }

        public Module() {
            
            EquipmentFamilies = new HashSet<EquipmentFamily>();

        }

    }

}

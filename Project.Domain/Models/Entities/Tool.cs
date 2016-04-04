using System.Collections.Generic;

namespace Project.Domain.Models.Entities {

    public class Tool : Entity<string>
    {

        public EquipmentFamily EquipmentFamily { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public EquipmentMaintFamily EquipmentMaintFamily { get; set; }
        public EquipmentGroup EquipmentGroup { get; set; }

        public ICollection<Chamber> Chambers { get; set; }
        public ICollection<Port> Ports { get; set; }

        public string FacilityId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string EquipmentFamilyId { get; set; }
        public string EquipmentTypeId { get; set; }
        public string EquipmentMaintFamilyId { get; set; }
        public string EquipmentGroupId { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
        public string TextColor { get; set; }
        public string State { get; set; }
        public string StateDisplay { get; set; }

        public Tool() {

            Chambers = new HashSet<Chamber>();
            Ports = new HashSet<Port>();

        }

    }
}

using System.Collections.Generic;

namespace Project.Domain.Models.Entities {

    public class EquipmentGroup : EquipmentBase {

        public string EquipmentClass { get; set; }

        public Module Module { get; set; }
        public ICollection<Tool> Tools { get; set; }

        public EquipmentGroup() {

            Tools = new HashSet<Tool>();

        }

    }

}

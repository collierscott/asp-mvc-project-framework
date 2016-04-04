using System.Collections.Generic;

namespace Project.Domain.Models.Entities {

    public class EquipmentType : EquipmentBase {

        public Module Module { get; set; }
        public ICollection<Tool> Tools { get; set; }

        public EquipmentType() {

            Tools = new HashSet<Tool>();

        }

    }
}

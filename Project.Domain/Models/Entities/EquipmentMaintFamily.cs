using System.Collections.Generic;

namespace Project.Domain.Models.Entities {

    public class EquipmentMaintFamily : EquipmentBase {

        public Module Module { get; set; }
        public ICollection<Tool> Tools { get; set; }

        public EquipmentMaintFamily() {

            Tools = new HashSet<Tool>();

        }

    }

}

using System;
using System.Collections.Generic;

namespace Project.Domain.Models.Entities {

    public class EquipmentFamily : EquipmentBase {

        public Module Module { get; set; }
        public String ModuleName { get; set; }
        public int ToolCount { get; set; }

        public ICollection<Tool> Tools { get; set; }

        public EquipmentFamily() {

            Tools = new HashSet<Tool>();

        }

    }
}

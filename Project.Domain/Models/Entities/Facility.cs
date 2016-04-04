using System.Collections.Generic;

namespace Project.Domain.Models.Entities {

    public class Facility : Entity<string>
    {

        public string Name { get; set; }
        public ICollection<Module> Modules { get; set; }
        public int SortOrder { get; set; }

        public Facility() {

            Modules = new HashSet<Module>();

        }

    }
}

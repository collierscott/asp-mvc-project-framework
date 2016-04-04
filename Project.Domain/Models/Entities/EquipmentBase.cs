namespace Project.Domain.Models.Entities {
    
    public abstract class EquipmentBase : Entity<string> {

        public string FacilityId { get; set; }
        public string ModuleId { get; set; }
        public string Name { get; set; }

    }

}

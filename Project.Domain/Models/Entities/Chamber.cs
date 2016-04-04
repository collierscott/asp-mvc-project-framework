namespace Project.Domain.Models.Entities {

    public class Chamber : Entity<string>
    {

        public Tool Tool { get; set; }

        public string FacilityId { get; set; }
        public string ModuleId { get; set; }
        public string ToolsetId { get; set; }
        public string ToolId { get; set; }

        public string Style { get; set; }
        public string Notes { get; set; }
        public string TextColor { get; set; }
        public string State { get; set; }

    }

}

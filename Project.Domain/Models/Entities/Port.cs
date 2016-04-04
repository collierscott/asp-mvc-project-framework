namespace Project.Domain.Models.Entities {

    public class Port : Entity<string> {

        public Tool Tool { get; set; }

        public string FacilityId { get; set; }
        public string ModuleId { get; set; }
        public string ToolsetId { get; set; }
        public string ToolId { get; set; }

        public string Name { get; set; }
        public string AutoDisplay { get; set; }
        public string AutoUser { get; set; }
        public bool IsAutoPort { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }

    }
}

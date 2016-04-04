using Project.Domain.Models.Entities;

namespace Project.Application.Models.Menus
{

    public class BreadcrumbItem : Entity<string>
    {

        public int ParentId { get; set; }
        public int ApplicatonId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string CssClass { get; set; }
        public string Url { get; set; }
        public string OpenInNewWindow { get; set; }
        public int SortOrder { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public object Attributes { get; set; }
        public object Parameters { get; set; }

        public BreadcrumbItem()
        {
            Parameters = null;
            Attributes = null;
        }

    }

}

using System.Collections.Generic;
using Project.Domain.Models.Entities;

namespace Project.Application.Models.Menus
{

    public class MenuItem : Entity<string>
    {

        public string Name { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public string Target { get; set; }
        public bool IsActive { get; set; }

        public object Attributes { get; set; }
        public object Parameters { get; set; }

        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public bool HasChildren { get; set; }
        public IEnumerable<MenuItem> Children { get; set; }

        public int SortOrder { get; set; }

        public MenuItem()
        {
            Children = new List<MenuItem>();
            IsEnabled = true;
            IsVisible = true;
            Parameters = null;
            Attributes = null;
        }

    }

}

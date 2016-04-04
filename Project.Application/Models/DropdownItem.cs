using Project.Domain.Models.Entities;

namespace Project.Application.Models
{

    public class DropdownItem : Entity<string>
    {

        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public int SortOrder { get; set; }
         
    }

}
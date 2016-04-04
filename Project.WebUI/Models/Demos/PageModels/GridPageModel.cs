using System.Collections.Generic;
using Project.Domain.Models.Entities;
namespace Project.WebUI.Models.Demos.PageModels
{

    public class GridPageModel
    {

        private IList<GridItem> _grid;
        public Facility Facility { get; set; }

        public IList<GridItem> Grid
        {
            get
            {
                var items = new List<GridItem>();

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var module in Facility.Modules)
                {

                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var toolset in module.EquipmentFamilies)
                    {

                        var item = new GridItem
                        {
                            Module = module.Name,
                            Toolset = toolset.Name,
                            ToolCount = toolset.Tools.Count
                        };

                        items.Add(item);

                    }

                }

                _grid = items;
                return _grid;
            }
            set { _grid = value; }
        }

        public GridPageModel()
        {
            Grid = new List<GridItem>();
        }

    }

}
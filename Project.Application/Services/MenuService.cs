using Project.Application.Models.Menus;
using Project.Application.Repositories.Abstract;
using Project.Application.Services.Abstract;
using log4net;
using System.Collections.Generic;

namespace Project.Application.Services
{

    public class MenuService : IMenuService
    {

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // ReSharper disable once NotAccessedField.Local
        private IMenuRepository _repository;

        public MenuService(IMenuRepository repo)
        {
            _repository = repo;
        }

        public IEnumerable<MenuItem> GetTopMenuItems(object parameters)
        {

            var items = new List<MenuItem>();

            var item = new MenuItem
            {
                Id = "0",
                Name = "Home",
                Text = "Home",
                Title = "Go to the Home page",
                ControllerName = "Home",
                ActionName = "Index",
                Parameters = parameters,
                SortOrder = 0
            };

            items.Add(item);

            item = new MenuItem
            {
                Id = "1",
                Name = "Demos",
                Text = "Demos",
                Title = "Go to Demos",
                ControllerName = "Demos",
                ActionName = "Index",
                Parameters = parameters,
                SortOrder = 1
            };


            var children = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = "0",
                    Name = "Highcharts",
                    Text = "Highcharts",
                    Title = "Go to Highcharts Example",
                    ControllerName = "Demos",
                    ActionName = "CreateChart",
                    Parameters = parameters,
                    SortOrder = 0
                },
                new MenuItem
                {
                    Id = "1",
                    Name = "Highcharts",
                    Text = "Highcharts (Stacked Bar)",
                    Title = "Go to Highcharts Example",
                    ControllerName = "Demos",
                    ActionName = "CreateStackedbarChart",
                    Parameters = parameters,
                    SortOrder = 1
                },
                new MenuItem
                {
                    Id = "2",
                    Name = "DataTable",
                    Text = "DataTable",
                    Title = "Go to DataTable Example",
                    ControllerName = "Demos",
                    ActionName = "DataTableGrid",
                    Parameters = parameters,
                    SortOrder = 2
                }
            };

            item.Children = children;

            items.Add(item);

            return items;

        }

    }

}

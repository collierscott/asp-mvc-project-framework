using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Infrastructure.Data;
using Infrastructure.Data.Utilities;
using Moq;
using Project.Application.Models;
using Project.Application.Models.Charting;
using Project.Application.Queries.Demos;
using Project.Application.Repositories.Abstract;
using Project.Application.Utilities;
using Project.Domain.Models.Entities;

namespace Project.Application.Repositories
{
    public class FakerRepository : GenericRepository, IProjectRepository
    {

        private IList<Facility> _facilities;
             
        public FakerRepository(string context)
            : base(string.Empty)
        {

            _facilities = new List<Facility>();

            for (int f = 0; f < 1; ++f)
            {
                var facility = new Facility
                {
                    Id = "DEMO1",
                    Name = "DEMO1"
                };

                for (int m = 0; m < 5; ++m)
                {
                    var module = new Module
                    {
                        Id = Faker.Name.First(),
                        Facility = facility,
                        FacilityId = facility.Id,
                        Name = Faker.Name.First()
                    };

                    var limit = new Random(10).Next(5, 15);

                    for (int e = 0; e < limit; ++e)
                    {
                        var eqp = new EquipmentFamily
                        {
                            Id = Faker.Name.Last(),
                            Name = Faker.Name.Last(),
                            FacilityId = facility.Id,
                            Module = module,
                            ModuleId = module.Id,
                            ModuleName = module.Name
                        };

                        var toolLimit = new Random(10).Next(5, 15);

                        for (int t = 0; t < toolLimit; ++t)
                        {
                            var tool = new Tool
                            {
                                Id = "Tool" + Faker.RandomNumber.Next(100),
                                EquipmentFamily = eqp,
                                EquipmentFamilyId = eqp.Id,
                                FacilityId = facility.Id,
                                ModuleName = module.Name,
                                ModuleId = module.Id,
                            };

                            eqp.Tools.Add(tool);

                        }

                        module.EquipmentFamilies.Add(eqp);
                    }

                    facility.Modules.Add(module);
                }

                _facilities.Add(facility);
            }
        }

        public Facility FindFacility(string value)
        {
            var facility = (
                from fac in _facilities
                where fac.Id.Equals(value, StringComparison.OrdinalIgnoreCase)
                select fac
                ).FirstOrDefault();
            return facility;
        }

        public IDataReader BuildModules(string facility, string m = null)
        {
            var fac = FindFacility(facility);

            var modules = (from module in fac.Modules
                where string.IsNullOrWhiteSpace(m) || module.Id.Equals(m, StringComparison.OrdinalIgnoreCase)
                select module
                );

            var values = (from module in modules
                from equipmentFamily in module.EquipmentFamilies
                from tool in equipmentFamily.Tools
                select new Dictionary<string, object>
                {
                    {"FACILITY", fac.Id},
                    { "MODULE", module.Id},
                    { "MODULE_DISPLAY", module.Name},
                    { "TOOLSET", equipmentFamily.Id},
                    { "TOOLSET_DISPLAY", equipmentFamily.Id},
                    { "TOOL", tool.Id},
                    { "TOOL_DISPLAY", tool.Name}
                }).ToList();

            return MockExecuteReader(values).Object;
        }
        public IEnumerable<Facility> GetAllFacilities()
        {
            return _facilities;
        }
        public Module FindModule(string facility, string value)
        {
            var f = (
                from fac in _facilities
                where fac.Id.Equals(facility, StringComparison.OrdinalIgnoreCase)
                select fac
                ).FirstOrDefault();

            var m = (from mod in f.Modules
                where mod.Id.Equals(value, StringComparison.OrdinalIgnoreCase)
                select mod).FirstOrDefault();

            return m;
        }
        public IEnumerable<ChartItem> GetStackedBarSeries(string facility)
        {
            var query = new GetStackBarSeriesQuery(facility);
            var items = GetAll<ChartItem>(query).ToList();
            return items;
        }
        public IEnumerable<ChartItem> GetStackedBarItems(string facility)
        {
            var query = new GetStackedBarItemsQuery(facility);
            return GetAll<ChartItem>(query).ToList();
        }
        public IDataReader GetDemosChartItems(string filter)
        {
            var query = new GetDemosChartItemsQuery(filter);

            Messages.Add(query.Message);

            return GetDataReader(query);
        }
        public IEnumerable<DropdownItem> GetWaferSizeDropdownItems(string facility)
        {
            var query = new GetWaferSizesQuery(facility);
            return GetAll<DropdownItem>(query);
        }
        public IEnumerable<DropdownItem> GetRouteGroupDropdownItems(string facility, string waferSize)
        {
            if (ItemHasNoValue.Check(waferSize))
            {
                waferSize = String.Empty;
            }
            else
            {
                waferSize = QueryHelper.AddSingleQuotes(waferSize);
            }

            var query = new GetRouteGroupsQuery(facility, waferSize);

            return GetAll<DropdownItem>(query);
        }
        public IEnumerable<DropdownItem> GetRouteFamilyDropdownItems(string facility, string waferSize, string routeGroup)
        {
            if (ItemHasNoValue.Check(waferSize))
            {
                waferSize = String.Empty;
            }
            else
            {
                waferSize = QueryHelper.AddSingleQuotes(waferSize);
            }

            if (ItemHasNoValue.Check(routeGroup))
            {
                routeGroup = String.Empty;
            }
            else
            {
                routeGroup = QueryHelper.AddSingleQuotes(routeGroup);
            }

            var query = new GetRouteFamiliesQuery(facility, waferSize, routeGroup);

            return GetAll<DropdownItem>(query);
        }
        public IEnumerable<DropdownItem> GetSeriesDropdownItems(string facility, string waferSize, string routeGroup, string routeFamily)
        {
            waferSize = ItemHasNoValue.Check(waferSize) ? String.Empty : QueryHelper.AddSingleQuotes(waferSize);
            routeGroup = ItemHasNoValue.Check(routeGroup) ? String.Empty : QueryHelper.AddSingleQuotes(routeGroup);
            routeFamily = ItemHasNoValue.Check(routeFamily) ? String.Empty : QueryHelper.AddSingleQuotes(routeFamily);

            var query = new GetSeriesQuery(facility, waferSize, routeGroup, routeFamily);

            return GetAll<DropdownItem>(query);
        }

        private static Mock<IDataReader> MockExecuteReader(List<Dictionary<string, object>> returnValues)
        {
            var reader = new Mock<IDataReader>();
            int count = 0;
            reader.Setup(x => x.Read()).Returns(() => count < returnValues.Count).Callback(() => count++);

            foreach (var returnValue in returnValues)
            {
                foreach (var key in returnValue.Keys)
                {
                    reader.SetupGet(x => x[key]).Returns(() => returnValue[key]);
                }
            }

            return reader;
        }
    }
}
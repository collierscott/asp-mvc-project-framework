using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Project.Domain.Models.Entities;
using Infrastructure.Data.Utilities;
using log4net;

namespace Project.WebUI.Models.Demos.PageModels
{

    public class ChartPageModel
    {

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Highcharts _chart;

        public Facility Facility { get; set; }

        public Highcharts Chart
        {
            get
            {
                var modules = new string[Facility.Modules.Count];

                var points = new DotNet.Highcharts.Options.Point[Facility.Modules.Count];

                int count = 0;

                foreach (var module in Facility.Modules)
                {

                    modules[count] = module.Name;

                    // ReSharper disable once RedundantAssignment
                    var toolsets = new List<EquipmentFamily>();


                    if (module.EquipmentFamilies != null)
                    {

                        toolsets = module.EquipmentFamilies.OrderBy(o => o.Name, new AlphaNumComparator()).ToList();

                        var toolsetCount = toolsets.Count;

                        var toolsetNames = new string[toolsets.Count];
                        var toolsetTools = new DotNet.Highcharts.Options.Point[toolsets.Count];

                        int t = 0;

                        foreach (var toolset in toolsets)
                        {

                            toolsetNames[t] = toolset.Name;

                            if (toolset.Tools != null)
                            {

                                var toolCount = toolset.Tools.Count;

                                var toolPoint = new DotNet.Highcharts.Options.Point
                                {
                                    Y = toolCount
                                };

                                toolsetTools[t] = toolPoint;
                                t++;

                            }
                        }

                        var toolsetData = new Data(toolsetTools);

                        var point = new DotNet.Highcharts.Options.Point
                        {

                            Y = toolsetCount,
                            Color = Color.CadetBlue,

                            Drilldown = new Drilldown
                            {
                                Name = modules[count],
                                Color = Color.Green,
                                Categories = toolsetNames,
                                Data = toolsetData
                            }

                        };

                        points[count] = point;
                        count++;
                    }

                }

                string[] categories = modules;

                const string name = "Modules";

                var data = new Data(points);

                ChartTypes defaultSeriesType;

                if (!Enum.TryParse("Column", out defaultSeriesType))
                {
                    defaultSeriesType = ChartTypes.Line;
                }

                _chart = new Highcharts("chart")
                    .InitChart(new Chart
                    {
                        DefaultSeriesType = defaultSeriesType
                    })
                    .SetTitle(new Title { Text = "Number of Toolsets per Module" })
                    .SetXAxis(
                        new XAxis
                        {
                            Categories = categories,
                            Labels = new XAxisLabels
                            {
                                Rotation = 270

                            }
                        })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Number of Toolsets" } })
                    .SetLegend(new Legend { Enabled = false })
                    .SetTooltip(new Tooltip { Formatter = "TooltipFormatter" })
                    .SetCredits(new Credits { Enabled = false })
                    .SetPlotOptions(new PlotOptions
                    {
                        Column = new PlotOptionsColumn
                        {
                            Cursor = Cursors.Pointer,
                            Point = new PlotOptionsColumnPoint { Events = new PlotOptionsColumnPointEvents { Click = "ColumnPointClick" } },
                            DataLabels = new PlotOptionsColumnDataLabels
                            {
                                Enabled = true,
                                Color = Color.FromName("colors[0]"),
                                Formatter = "function() { return this.y; }",
                                Style = "fontWeight: 'bold'"
                            }
                        }
                    })
                    .SetSeries(new Series
                    {
                        Name = "Modules",
                        Data = data,
                        Color = Color.White
                    })
                    .SetExporting(new Exporting { Enabled = true })
                    .AddJavascripFunction(
                        "TooltipFormatter",
                        @"var point = this.point, s = this.x +':<b>'+ this.y +'</b><br/>';
                      if (point.drilldown) {
                        s += 'Click to view '+ point.category;
                      } else {
                        s += '';
                      }
                      return s;"
                    )
                    .AddJavascripFunction(
                        "ColumnPointClick",
                        @"var drilldown = this.drilldown;
                      if (drilldown) { // drill down
                        setChart(drilldown.name, drilldown.categories, drilldown.data.data, drilldown.color);
                      } else { // restore
                        setChart(name, categories, data.data);
                      }"
                    )
                    .AddJavascripFunction(
                        "setChart",
                        @"chart.xAxis[0].setCategories(categories);
                      chart.series[0].remove();
                      chart.addSeries({
                         name: name,
                         data: data,
                         color: color || 'gray'
                      });",
                        "name", "categories", "data", "color"
                    )
                    .AddJavascripVariable("colors", "Highcharts.getOptions().colors")
                    .AddJavascripVariable("name", "'{0}'".FormatWith(name))
                    .AddJavascripVariable("categories", JsonSerializer.Serialize(categories))
                    .AddJavascripVariable("data", JsonSerializer.Serialize(data));

                return _chart;

            }
            set
            {
                _chart = value;
            }
        }

        public ChartPageModel()
        {
            Chart = new Highcharts("Chart");
        }

    }

}
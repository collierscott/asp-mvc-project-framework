using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Project.Application.Models.Charting;
using log4net;

namespace Project.WebUI.Models.Demos.PageModels
{
    public class StackedChartPageModel
    {

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Highcharts _chart;

        public IList<ChartItem> ChartItems { get; set; }

        public StackedChartPageModel()
        {
            ChartItems = new List<ChartItem>();
            _chart = new Highcharts("chart");
        }

        public Highcharts Chart
        {

            get
            {

                var series = new List<Series>();

                var seriesNames = (from row in ChartItems
                                   orderby row.SeriesSortOrder descending 
                                   select row.SeriesName
                ).Distinct().ToList();

                var categories = (from rows in ChartItems
                                  orderby rows.XLabelSortOrder
                                  select rows.XLabel
                ).Distinct().ToList();

                foreach (var item in seriesNames)
                {

                    var items = ChartItems.Where(m => m.SeriesName.Equals(item, StringComparison.OrdinalIgnoreCase)).ToList();

                    var points = new List<Point>();

                    if (items.Any())
                    {

                        var s = new Series
                        {
                            Name = item,
                            Color = System.Drawing.ColorTranslator.FromHtml(items.First().SeriesColor)
                        };

                        //_log.Debug(item + " " + items.First().SeriesColor);

                        foreach (var cat in categories)
                        {

                            //_log.Debug(item + " " + cat + " " + items.Count);

                            var point = new Point();

                            if (!string.IsNullOrWhiteSpace(cat))
                            {

                                var dataItem =
                                    items.Where(f => f.XLabel.Equals(cat, StringComparison.OrdinalIgnoreCase))
                                        .ToList();

                                if (dataItem.Any())
                                {
                                    var di = dataItem.First();
                                    point.Y = di.YValue;

                                    s.Color = System.Drawing.ColorTranslator.FromHtml(di.SeriesColor);
                                } else
                                {
                                    point.Y = 0;
                                }

                            } else
                            {
                                point.Y = null;
                            }

                            points.Add(point);

                        }

                        s.Data = new Data(points.ToArray());

                        series.Add(s);

                    } 

                }

                _chart = new Highcharts("chart")
                    .InitChart(
                        new Chart
                        {
                            DefaultSeriesType = ChartTypes.Column,
                            //Width = 960,
                            Height = 650,
                            SpacingBottom = 50
                        })
                    .SetTitle(new Title
                    {
                        Text = "Reason Chart by Process",
                        Align = HorizontalAligns.Left
                    })
                    .SetCredits(new Credits {Enabled = false})
                    .SetXAxis(
                    new XAxis
                    {
                        Categories = categories.ToArray(),
                        Title = new XAxisTitle {Text = "Processes", Rotation = 0},
                        Labels = new XAxisLabels
                        {
                            Rotation = -90,
                            Align = HorizontalAligns.Right
                        }
                    })
                    .SetYAxis(new YAxis
                    {
                        AllowDecimals = false,
                        Min = 0,
                        Title = new YAxisTitle
                        {
                            Text = "Goal Total (by Reason)",
                            Rotation = 270,
                            Align = AxisTitleAligns.Middle
                        },
                        Labels = new YAxisLabels
                        {
                            X = -1
                        }
                    })
                    .SetLegend(new Legend
                    {
                        Enabled = true,
                        Layout = Layouts.Vertical,
                        Align = HorizontalAligns.Right,
                        VerticalAlign = VerticalAligns.Top,
                        Y = 25,
                        Title = new LegendTitle {Text = "Goal Reasons"}
                    })
                    .SetPlotOptions(new PlotOptions
                    {
                        Column = new PlotOptionsColumn
                        {
                            Stacking = Stackings.Normal,
                            GroupPadding = 0,
                            BorderWidth = 0
                        }
                    })
                    .SetSeries(series.ToArray());

                return _chart;

            }

        }

    }

}
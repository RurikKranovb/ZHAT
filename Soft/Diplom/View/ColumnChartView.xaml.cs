using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Diplom.Infrastructure;
using Diplom.Infrastructure.Fact;
using Diplom.Infrastructure.Norm;
using Diplom.View.Infrastructure;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;


namespace Diplom.View
{
    /// <summary>
    /// Логика взаимодействия для ColumnChartView.xaml
    /// </summary>
    public partial class ColumnChartView : Window
    {
        private PlotModel modelP1;
        private PlotModel plotModel1;
        private PlotModel _plotModel;
        private double _lastItem;


        public ColumnChartView()
        {
            InitializeComponent();
        }

        public void GetPieView(GetNormDataItems getNormDataItems)
        {
            ChartItems = new ChartItems();
            ChartItemses = new ObservableCollection<ChartItems>();

            ChartTables = new ObservableCollection<ChartTable>();

            modelP1 = new PlotModel();

            var seriesP1 = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                //OutsideLabelFormat = "{0}",
                //StartAngle = 100
            };



            foreach (var getFactDataItem in getNormDataItems.GetNormListItems)
            {
                try
                {
                    var e = getFactDataItem.C + "-" + getFactDataItem.D;
                    double d = double.Parse(getFactDataItem.E, CultureInfo.InvariantCulture);
                    double g = double.Parse(getFactDataItem.G, CultureInfo.InvariantCulture);

                    ChartItems.ChartItemses.Add(new ChartItem()
                    {
                        A = getFactDataItem.A,
                        C = getFactDataItem.C,
                        //B = getFactDataItem.B,
                        D = d,
                        E = e,
                        G = g
                    });
                }

                catch (Exception exception)
                {
                    var e = getFactDataItem.C + "-" + getFactDataItem.D;

                    double d = double.Parse(getFactDataItem.E, CultureInfo.GetCultureInfo("Ru-ru"));
                    double g = double.Parse(getFactDataItem.G, CultureInfo.GetCultureInfo("Ru-ru"));

                    ChartItems.ChartItemses.Add(new ChartItem()
                    {
                        A = getFactDataItem.A,
                        C = getFactDataItem.C,
                        //B = getFactDataItem.B,
                        D = d,
                        E = e,
                        G = g
                    });
                }

            }
            ChartItems.NameList = getNormDataItems.NameList;


            ChartItemses.Add(ChartItems);

            var max = ChartItemses.Select(items => items.ChartItemses.Max(item => item.G)).FirstOrDefault();
            var min = ChartItemses.Select(items => items.ChartItemses.Min(item => item.G)).FirstOrDefault();

            var step = (max - min) / 7;


            var list = new Dictionary<double, int>();

            var countStep = 0;

            foreach (var items in ChartItemses.Select(items => items.ChartItemses.OrderBy(item => item.G).ToList()))
            {

                var objects = items.Select(item => item.E).Distinct().ToList();
                double i = items.Count();

                foreach (var o in items)
                {
                    var newItem = o.G;
                    if (newItem >= _lastItem)
                    {
                        if (countStep != 0)
                        {
                            list.Add(_lastItem, countStep);

                            ChartTables.Add(new ChartTable()
                            {
                                Name = _lastItem.ToString(CultureInfo.InvariantCulture),
                                Count = countStep
                            });

                            seriesP1.Slices.Add(
                                new PieSlice(_lastItem.ToString(CultureInfo.InvariantCulture), countStep)
                                {
                                    IsExploded = true,
                                });

                        }

                        countStep = 0;

                        _lastItem = newItem + step;
                    }
                    countStep++;
                }

                if (list.ContainsKey(_lastItem)) continue;
                list.Add(_lastItem, countStep);

                ChartTables.Add(new ChartTable()
                {
                    Name = _lastItem.ToString(CultureInfo.InvariantCulture),
                    Count = countStep
                });

                seriesP1.Slices.Add(new PieSlice(_lastItem.ToString(CultureInfo.InvariantCulture), countStep)
                {
                    IsExploded = true,
                });

                //foreach (var o in objects.OrderBy(s => s))
                //{
                //    var tms = items.Where(item => item.E == o);

                //    var count = tms.Count();

                //    //var average = Math.Round(tms.Average(item => item.D), 1);

                //    //dictionary.Add(o,count);
                //    ChartTables.Add(new ChartTable()
                //    {
                //        Name = o,
                //        Count = count
                //    });


                //    seriesP1.Slices.Add(new PieSlice(o, count)
                //    {
                //        IsExploded = true,
                //    });

                //}
            }






            //foreach (var chartItemse in ChartItemses)
            //{
            //    foreach (var o in chartItemse.ChartItemses.OrderBy(item => item.G))
            //    {
            //        var newItem = o.G;
            //        if (newItem >= _lastItem)
            //        {
            //            if (countStep != 0)
            //            {
            //                list.Add(_lastItem, countStep);
            //            }

            //            countStep = 0;

            //            _lastItem = newItem + step;                        
            //        }
            //        countStep++;
            //    }
            //}

            //if (!list.ContainsKey(_lastItem))
            //{
            //    list.Add(_lastItem, countStep);
            //}

            //var sum = ChartTables.Sum(table => table.Count);

            //ChartTables.Add(new ChartTable()
            //{
            //    Name = "Итого:",
            //    Count = sum
            //});

            var list1 = ChartTables.OrderBy(table => table.Name).Select(table => table.Name).ToList();
            var list2 = ChartTables.OrderBy(table => table.Name).Select(table => table.Count).ToList();
            //Dictionary<string, int> dictionary = new Dictionary<string, int>();
            //ListBox.ToolTip = "Класс-Специализация железнодорожной линии";
            //ListBox1.ToolTip = "Расчетное значение потерь поездо-часов";

            //ListBox.ItemsSource = list1;
            //ListBox1.ItemsSource = list2;
            modelP1.Series.Add(seriesP1);

            //PlotView.Model = modelP1;

        }

        public ChartItems ChartItems { get; set; }

        public ObservableCollection<ChartItems> ChartItemses { get; set; }

        public ObservableCollection<ChartTable> ChartTables { get; set; }

        public ChartItems ChartItemss { get; set; }


        public ObservableCollection<ChartItems> ChartItemsess { get; set; }

        public PlotModel PlotModel
        {
            get => _plotModel;
            set { _plotModel = value; }
        }

        public PlotModel Model1
        {
            get { return modelP1; }
            set { modelP1 = value; }
        }


        public void GetChartView(GetNormDataItems getNormDataItems)
        {
            ChartItemss = new ChartItems();


            foreach (var item in getNormDataItems.GetNormListItems)
            {

                try
                {
                    var e = item.C + "-" + item.D;
                    double d = double.Parse(item.E, CultureInfo.InvariantCulture);

                    ChartItemss.ChartItemses.Add(new ChartItem()
                    {
                        A = item.A,
                        C = item.C,
                        //B = getFactDataItem.B,
                        D = d,
                        E = e
                    });
                }

                catch (Exception exception)
                {
                    var e = item.C + "-" + item.D;

                    double d = double.Parse(item.E, CultureInfo.GetCultureInfo("Ru-ru"));

                    ChartItemss.ChartItemses.Add(new ChartItem()
                    {
                        A = item.A,
                        C = item.C,
                        //B = getFactDataItem.B,
                        D = d,
                        E = e
                    });
                }
            }


            var model = new PlotModel();

            _plotModel = new PlotModel();



            var enumerable = ChartItemss.ChartItemses.Select(item => item.E).Distinct().OrderBy(s => s).ToList();

            var bar = new List<BarItem>();
            List<string> its = new List<string>();

            var bar1 = new List<BarItem>();
            List<string> its1 = new List<string>();

            foreach (var s in enumerable.OrderBy(s => s))
            {

                var count = (double) ChartItemss.ChartItemses.Count(chartItem => chartItem.E == s);

                var where = ChartItemss.ChartItemses.Where(item => item.E == s).ToList();

                var average = where.Select(item => item.D).Average();

                its1.Add(s);

                bar1.Add(new BarItem(average));


                bar.Add(new BarItem(count));

                its.Add(s);
            }


            var barSeries = new BarSeries
            {
                ItemsSource = bar,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}"
            };

            model.Series.Add(barSeries);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = its
            });

            var barSeries1 = new BarSeries
            {
                ItemsSource = bar1,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}"
            };

            _plotModel.Series.Add(barSeries1);

            _plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = its
            });

            PlotViews.Model = _plotModel;

            Kolvo.Model = model;

        }


        public void DiagramKg(GetNormDataItems getNormDataItems, GetFactDataItems selectFactListItem)
        {

            modelP1 = new PlotModel();
            var bar = new List<BarItem>();
            //var seriesP1 = new PieSeries
            //{
            //    StrokeThickness = 2.0,
            //    InsideLabelPosition = 0.8,
            //    AngleSpan = 360,
            //    //OutsideLabelFormat = "{0}",
            //    //StartAngle = 100
            //};


            var listItem = new List<ChartItem>();
            foreach (var item in getNormDataItems.GetNormListItems)
            {
                try
                {
                    double g = double.Parse(item.G, CultureInfo.InvariantCulture);

                    listItem.Add(new ChartItem()
                    {
                        G = g
                    });
                }
                catch (Exception e)
                {
                    double g = double.Parse(item.G, CultureInfo.GetCultureInfo("Ru-ru"));

                    listItem.Add(new ChartItem()
                    {
                        G = g
                    });
                }
            }

            foreach (var item in selectFactListItem.GetFactListItems)
            {
                try
                {
                    double g = double.Parse(item.H, CultureInfo.InvariantCulture);

                    listItem.Add(new ChartItem()
                    {
                        G = g
                    });
                }
                catch (Exception e)
                {
                    double g = double.Parse(item.H, CultureInfo.GetCultureInfo("Ru-ru"));

                    listItem.Add(new ChartItem()
                    {
                        G = g
                    });
                }
            }

            var max = listItem.Max(item => item.G);
            var min = listItem.Min(item => item.G);

            var step = (max - min) / 7;


            var list = new Dictionary<double, int>();
            _lastItem = 0;
            var countStep = 0;

            foreach (var o in listItem.OrderBy(item => item.G).ToList())
            {

                var newItem = o.G;
                if (newItem >= _lastItem)
                {
                    if (countStep != 0)
                    {
                        list.Add(_lastItem, countStep);

                        bar.Add(new BarItem(countStep));

                    }

                    countStep = 0;

                    _lastItem = newItem + step;
                }
                countStep++;


               
            }

            if (list.ContainsKey(_lastItem))
            {
                list.Add(_lastItem, countStep);

                bar.Add(new BarItem(countStep));
            }

         


            var barSeries = new BarSeries
            {
                ItemsSource = bar,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}"
            };

            modelP1.Series.Add(barSeries);

            modelP1.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = list.Keys
            });
            Step.Model = modelP1;
        }
    }

    //public class ChartTable
    //{
    //    public string Name { get; set; }
    //    public int Count { get; set; }
    //}
}


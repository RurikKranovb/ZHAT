using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Diplom.DataBase;
using Diplom.DataBase.Infrastructure;
using Diplom.Infrastructure.Common;
using Diplom.Infrastructure.Fact;
using Diplom.Infrastructure.Table.Facts;
using FluentNest;
using Nest;

namespace Diplom.Command.Import
{
    public class ImportFactCommand
    {
        private List<FactIntInc> _factIntIncs;
        private List<FactIntOtk> _factIntOtks;
        private List<FactKg> _factKgs;
        private List<FactSrVrDoRecovery> _factSrVrDoRecoveries;
        private List<FactTrainTime> _factTrainTimes;
        private List<FactVrYstr> _factVrYstrs;
        private List<ResultFact> _factResult;

        public ImportFactCommand()
        {
            Initialize();
        }

        private void Initialize()
        {
            _factIntIncs = new List<FactIntInc>();
            _factIntOtks = new List<FactIntOtk>();
            _factKgs = new List<FactKg>();
            _factSrVrDoRecoveries = new List<FactSrVrDoRecovery>();
            _factTrainTimes = new List<FactTrainTime>();
            _factVrYstrs = new List<FactVrYstr>();
            _factResult = new List<ResultFact>();
        }

        public void SetFactDataBase(ImportFactDataItems factOriginalDataItems)
        {
            Initialize();
            ClientElasticsearch(factOriginalDataItems);
        }

        private void ClientElasticsearch(ImportFactDataItems factOriginalDataItems)
        {
            try
            {
                var client = ElasticSearchDataModel.Instance.Settings;
                if (client == null) return;

                var search = client
                    .Search<FactResultItems>(s => s
                        .Skip(0).Take(0)
                        .FilterOn(host => host.NameList == factOriginalDataItems.NameList.ToLower())
                        .Aggregations(a => a
                            .SortedTopHits(1, x => x.NameList, SortType.Ascending)
                            .GroupBy(selectionList => selectionList.NameList)));

                var lists = search.Aggs.Aggregations.Count == 0
                    ? null
                    : search.Aggs
                        .GetGroupBy<FactResultItems>(selectionList => selectionList.NameList)
                        .SelectMany(bucket =>
                            bucket.GetSortedTopHits<FactResultItems>(selectionList => selectionList.NameList,
                                SortType.Ascending))
                        .FirstOrDefault();

                if (lists == null)
                {
                    var hits = client.Search<FactResultItems>(s => s)
                        .Hits;

                    foreach (var hit in hits)
                    {
                        if (String.Equals(factOriginalDataItems.NameList, hit.Source.NameList,
                            StringComparison.CurrentCultureIgnoreCase))
                        {
                            var delete = client.Delete(new DocumentPath<FactResultItems>(hit.Id));
                        }
                    }

                    AddFactItems(client, factOriginalDataItems);
                }
                else
                {
                    AddFactItems(client, factOriginalDataItems);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }


        private void AddFactItems(ElasticClient client, ImportFactDataItems factOriginalDataItems)
        {
            List<FactOriginalData> originalDatas = new List<FactOriginalData>();
            foreach (var item in factOriginalDataItems.ImportFactListItems)
            {
                var a = string.IsNullOrEmpty(item.A) ? "0" : item.A;
                //var b = string.IsNullOrEmpty(item.B) ? "0" : item.B;
                var C = string.IsNullOrEmpty(item.C) ? "0" : item.C;
                var d = string.IsNullOrEmpty(item.D) ? "0" : item.D;
                //var e = string.IsNullOrEmpty(item.E) ? "0" : item.E;
                var f = string.IsNullOrEmpty(item.F) ? "0" : item.F;
                var g = string.IsNullOrEmpty(item.G) ? "0" : item.G;
                //var h = string.IsNullOrEmpty(item.H) ? "0" : item.H;
                //var i = string.IsNullOrEmpty(item.I) ? "0" : item.I;
                var j = string.IsNullOrEmpty(item.J) ? "0" : item.J;
                //var k = string.IsNullOrEmpty(item.K) ? "0" : item.K;
                //var l = string.IsNullOrEmpty(item.L) ? "0" : item.L;
                //var m = string.IsNullOrEmpty(item.M) ? "0" : item.M;
                //var n = string.IsNullOrEmpty(item.N) ? "0" : item.N;
                //var o = string.IsNullOrEmpty(item.O) ? "0" : item.O;
                originalDatas.Add(new FactOriginalData()
                {
                    A = a,
                    //B = b,
                    C = C,
                    D = d,
                    //E = e,
                    F = f,
                    G = g,
                    //H = h,
                    //I = i,
                    J = j,
                    //K = k,
                    //L = l,
                    //M = m,
                    //N = n,
                    //O = o
                });
            }

            foreach (var factOriginalData in originalDatas)
            {
                var f = double.Parse(factOriginalData.F);
                var g = double.Parse(factOriginalData.G);
                //var n = double.Parse(factOriginalData.N);
                //var m = double.Parse(factOriginalData.M);
                //var l = double.Parse(factOriginalData.L);
                //var o = double.Parse(factOriginalData.O);


                var e = f / (365 * 24 - g);
                _factIntOtks.Add(new FactIntOtk()
                {
                    A = factOriginalData.A,
                    B = factOriginalData.B,
                    C = factOriginalData.C,
                    D = factOriginalData.D,
                    E = e
                });

                e = g / (60 * f);
                _factVrYstrs.Add(new FactVrYstr()
                {
                    A = factOriginalData.A,
                    B = factOriginalData.B,
                    C = factOriginalData.C,
                    D = factOriginalData.D,
                    E = e
                });

                //e = (f + n + l + m) / (365 * 24 - g - o);

                //_factIntIncs.Add(new FactIntInc()
                //{
                //    A = factOriginalData.A,
                //    B = factOriginalData.B,
                //    C = factOriginalData.C,
                //    D = factOriginalData.D,
                //    E = e
                //});


                //e = (g + o) / (60 * (f + n));

                //_factSrVrDoRecoveries.Add(new FactSrVrDoRecovery()
                //{

                //    A = factOriginalData.A,
                //    B = factOriginalData.B,
                //    C = factOriginalData.C,
                //    D = factOriginalData.D,
                //    E = e
                //});

                _factTrainTimes.Add(new FactTrainTime()
                {
                    A = factOriginalData.A,
                    B = factOriginalData.B,
                    C = factOriginalData.C,
                    D = factOriginalData.D,
                    E = factOriginalData.J
                });
            }

            foreach (var factOriginalData in originalDatas)
            {
                var firstOrDefault = _factVrYstrs.FirstOrDefault(ystr => ystr.A == factOriginalData.A);
                var factIntOtk = _factIntOtks.FirstOrDefault(ystr => ystr.A == factOriginalData.A);

                var e = 1 / (1 + factIntOtk.E * firstOrDefault.E);
                var f = e == 1 ? 0 : e;


                _factKgs.Add(new FactKg()
                {
                    A = factOriginalData.A,
                    B = factOriginalData.B,
                    C = factOriginalData.C,
                    D = factOriginalData.D,
                    E = e,
                    F = f
                });
            }


            foreach (var factOriginalData in originalDatas)
            {
                var firstOrDefault = _factIntOtks.FirstOrDefault(otk => otk.A == factOriginalData.A);
                var factVrYstr = _factVrYstrs.FirstOrDefault(otk => otk.A == factOriginalData.A);
                var orDefault = _factKgs.FirstOrDefault(otk => otk.A == factOriginalData.A);
                //var factIntInc = _factIntIncs.FirstOrDefault(otk => otk.A == factOriginalData.A);
                //var factSrVrDoRecovery = _factSrVrDoRecoveries.FirstOrDefault(otk => otk.A == factOriginalData.A);

                var f = 0;
                if (!double.IsNaN(orDefault.F))
                {
                    f = orDefault.F == 0 ? 0 : 1;
                }
                _factResult.Add(new ResultFact()
                {
                    A = factOriginalData.A.ToString(),
                    //B = factOriginalData.B.ToString(),
                    C = factOriginalData.C.ToString(),
                    D = factOriginalData.D.ToString(),
                    E = Math.Round(double.Parse(factOriginalData.J), 2).ToString(CultureInfo.InvariantCulture) , //Фактическое значение потерь поездо-часов за отчетный период
                    F = Math.Round(firstOrDefault.E, 6).ToString(CultureInfo.InvariantCulture), //Фактическое значение интенсивности отказов 1 и 2 категории
                    G = Math.Round(factVrYstr.E, 2).ToString(CultureInfo.InvariantCulture), // Фактическое значение среднего времени устранения отказов 1 и 2 категории
                    H = double.IsNaN(orDefault.F) ? "0.999" : Math.Round(orDefault.F, 6).ToString(CultureInfo.InvariantCulture),// Фактическое значение коэффициента готовности по отказам 1 и 2 категории
                    I = f.ToString(),
                    //J = factIntInc.E.ToString(CultureInfo.InvariantCulture),
                    //K = factSrVrDoRecovery.E.ToString(CultureInfo.InvariantCulture)
                });
            }

            var c = client.Index(new FactResultItems()
            {
                NameList = factOriginalDataItems.NameList.ToLower(),

                ResultData = _factResult
            });
        }
    }
}

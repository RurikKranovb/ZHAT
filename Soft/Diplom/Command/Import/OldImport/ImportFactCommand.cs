using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Diplom.DataBase;
using Diplom.DataBase.Infrastructure;
using Diplom.Infrastructure.Common;
using Diplom.Infrastructure.Fact;
using FluentNest;
using Nest;

namespace Diplom.Command.Import.OldImport
{
    public class ImportFactCommand
    {
        public void SetFactDataBase(ImportFactDataItems factOriginalDataItems)
        {
            
            var client = ElasticSearchDataModel.Instance.Settings;
            if (client == null) return;

            var search = client
                .Search<FactSelectionList>(s => s
                    .Skip(0).Take(0)
                    .FilterOn(host => host.NameList == importItem.NameList.ToLower())
                    .Aggregations(a => a
                        .SortedTopHits(1, x => x.NameList, SortType.Ascending)
                        .GroupBy(selectionList => selectionList.NameList)));

            var lists = search.Aggs.Aggregations.Count == 0
                ? null
                : search.Aggs
                    .GetGroupBy<FactSelectionList>(selectionList => selectionList.NameList)
                    .SelectMany(bucket =>
                        bucket.GetSortedTopHits<FactSelectionList>(selectionList => selectionList.NameList,
                            SortType.Ascending))
                    .FirstOrDefault();

            if (lists == null)
            {

               
                var hits = client.Search<FactSelectionList>(s => s)
                    .Hits;

                foreach (var hit in hits)
                {
                    if (String.Equals(importItem.NameList, hit.Source.NameList,
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        var delete = client.Delete(new DocumentPath<FactSelectionList>(hit.Id));
                    }
                }


                AddFactItems(client, importItem, currentList.ToLower());
            }
            else
            {

                AddFactItems(client, importItem, currentList.ToLower());

            }

        }


        private void AddFactItems(ElasticClient client, FactImportItems importItem, string currentList)
        {
            //var list = new List<OriginalData>();
            var result = new List<FactResult>();
            foreach (var item in importItem.ImportItemItems)
            {
                var resultItem = new FactResult();

                resultItem.A = item.A;
                resultItem.C = item.C;
                resultItem.B = item.B;
                resultItem.D = item.D;
                //if (item.A.ToLower() == "бабаево")
                //{
                    
                //}

                var f = string.IsNullOrEmpty(item.F) ? "-" : item.F;
                var g = string.IsNullOrEmpty(item.G) ? "-" : item.G;
                var n = string.IsNullOrEmpty(item.N) ? "-" : item.N;
                var l = string.IsNullOrEmpty(item.L) ? "-" : item.L;
                var m = string.IsNullOrEmpty(item.M)
                    ? "-"
                    : item.M;
                var o = string.IsNullOrEmpty(item.O)
                    ? "-"
                    : item.O;
                var j = string.IsNullOrEmpty(item.J) ? "-" : item.J;

                if (f != "-" & g != "-")
                {
                    var f1 = double.Parse(f);
                    var g1 = double.Parse(g);
                    double faktIntOtk = f1 / (365 * 24 - g1);
                    resultItem.F = faktIntOtk.ToString(CultureInfo.InvariantCulture);
                    double faktVrustr = g1 / (60 * f1);
                    resultItem.G = faktVrustr.ToString(CultureInfo.InvariantCulture);
                    double faktKg = 1 / (1 + faktIntOtk * faktVrustr);
                    resultItem.H = faktKg.ToString(CultureInfo.InvariantCulture);

                    if (n != "-" & o != "-")
                    {
                        var o1 = double.Parse(o);
                        var n1 = double.Parse(n);
                        double faktSrVrDoVosst = (g1 + o1) / (60 * (f1 + n1));
                        resultItem.K = faktSrVrDoVosst.ToString(CultureInfo.InvariantCulture);
                        if (m != "-")
                        {
                            var m1 = double.Parse(m);

                            double faktIntInc = (f1 + n1 + m1) / (365 * 24 - g1 - o1);
                            resultItem.J = faktIntInc.ToString(CultureInfo.InvariantCulture);
                        }
                    }

                }
                else if (n != "-" & o != "-")
                {
                    var g1 = 0;
                    var f1 = 0;

                    var o1 = double.Parse(o);
                    var n1 = double.Parse(n);
                    double faktSrVrDoVosst = (g1 + o1) / (60 * (f1 + n1));
                    resultItem.K = faktSrVrDoVosst.ToString(CultureInfo.InvariantCulture);
                    if (m != "-")
                    {
                        var m1 = double.Parse(m);

                        double faktIntInc = (f1 + n1 + m1) / (365 * 24 - g1 - o1);
                        resultItem.J = faktIntInc.ToString(CultureInfo.InvariantCulture);
                    }
                }
                else
                {

                }

                resultItem.I = resultItem.H == null ? 0 : 1; //:todo


                resultItem.E = j;

                //Фактическое значение интенсивности отказов 1 и 2 категории (ноль - значение не установлено)
                //double faktIntOtk = f == 0 ? 0 : f / (365 * g);
                //resultItem.F = faktIntOtk;
                ////Фактическое значение среднего времени устранения отказов 1 и 2 категории, час
                //double faktVrustr = g == 0 ? 0 : g / (60 * f); // возможен 0
                //resultItem.G = faktVrustr;
                ////Фактическое значение Кг по отказам 1 и 2 категории
                //double faktKg = 1 / (faktIntOtk * faktVrustr);
                //resultItem.H = faktKg;

                ////Фактическое значение интенсивности инцидентов, 1/ч
                //double faktIntInc = (f + n + m) / (365 * 24 - g - o);
                //resultItem.J = faktIntInc;

                ////Фактическое значение среднего времени до восстановления
                //var check = (60 * (f + n));
                //double faktSrVrDoVosst = check == 0 ? 0 : (g + o) / (60 * (f + n));
                //resultItem.K = faktSrVrDoVosst;
                //var faktPoezdoTime = j;
                //resultItem.E = faktPoezdoTime;

                //resultItem.I = faktKg != 0 ? 0 : 1;


                result.Add(resultItem);

                //list.Add(new OriginalData()
                //{
                //    C = item.C,
                //    J = item.J,
                //    O = item.O,
                //    G = item.G,
                //    I = item.I,
                //    M = item.M,
                //    F = item.F,
                //    H = item.H,
                //    N = item.N,
                //    L = item.L,
                //    E = item.E,
                //    A = item.A,
                //    B = item.B,
                //    K = item.K,
                //    D = item.D
                //});



                //AddNeeItem(client, resultItem);
            }
            var c = client.Index(new FactSelectionList()
            {
                NameList = currentList.ToLower(),

                ResultData = result
            });
            DelayIsExist(client, c.Id);
            //isValid =  c.IsValid;
        }

        private bool DelayIsExist(ElasticClient client, string id)
        {
            for (var i = 0; i < 20; i++)
            {
                if (IsExist(client, id))
                {
                    return true;
                }
                Thread.Sleep(100);
            }
            return false;
        }

        private bool IsExist(ElasticClient index, string id)
        {
            var search = index.Search<FactSelectionList>(s => s
                .AllTypes()
                .Query(p => p.Ids(d => d.Values(new List<string> { id })))
            );

            var item = search
                .Hits
                .FirstOrDefault();

            return search.IsValid && item != null;
        }

        private void AddNeeItem(ElasticClient client, FactResult resultItem)
        {

            var t = new FactSelectionList();
            t.NameList = resultItem.ListName;
            t.ResultData.Add(new FactResult()
            {
                H = resultItem.H,
                C = resultItem.C,
                J = resultItem.J,
                F = resultItem.J,
                A = resultItem.A,
                E = resultItem.E,
                I = resultItem.I,
                B = resultItem.B,
                D = resultItem.D,
                G = resultItem.G,
                K = resultItem.K
            });

            var add = client.Index(t);

        }
    }
}

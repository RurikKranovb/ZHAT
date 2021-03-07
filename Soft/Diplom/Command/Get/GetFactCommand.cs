using System;
using System.Collections.ObjectModel;
using Diplom.DataBase;
using Diplom.DataBase.Infrastructure;
using Diplom.Infrastructure.Fact;

namespace Diplom.Command.Get
{
    public class GetFactCommand
    {
        private GetFactDataItems _resultItem;
        private ObservableCollection<GetFactDataItems> _resultItems;

        public ObservableCollection<GetFactDataItems> GetFile()
        {
            try
            {
                _resultItems = new ObservableCollection<GetFactDataItems>();
              
               var client = ElasticSearchDataModel.Instance.Settings;


               var search = client
                   .Search<FactResultItems>(s => s
                       .Skip(0).Take(Int32.MaxValue));
                //        .Aggregations(a => a
                //            .SortedTopHits(1, x => x.NameList, SortType.Ascending)
                //            .GroupBy(config => config.NameList)));

                //var docItems = search.Aggs.Aggregations.Count == 0
                //    ? new List<FactResultItems>()
                //    : search.Aggs
                //        .GetGroupBy<FactResultItems>(arg => arg.NameList)
                //        .SelectMany(
                //            bucket => bucket.GetSortedTopHits<FactResultItems>(x => x.NameList, SortType.Ascending))
                //        .ToList();


                foreach (var doc in search.Documents)
                {
                    _resultItem = new GetFactDataItems();

                    _resultItem.NameList = doc.NameList;


                    foreach (var item in doc.ResultData)
                    {
                        _resultItem.GetFactListItems.Add(new ResultFact()
                        {
                            C = item.C,
                            A = item.A,
                            B = item.B,
                            D = item.D,
                            H = item.H,
                            I = item.I,
                            F = item.F,
                            J = item.J,
                            E = item.E,
                            G = item.G, 
                            K = item.K
                        });
                    }

                    _resultItems.Add(_resultItem);
                }
                return _resultItems;

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

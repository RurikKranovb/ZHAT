using System;
using System.Linq;
using Diplom.DataBase;
using Diplom.DataBase.Infrastructure;
using Diplom.Infrastructure.Fact;
using FluentNest;
using Nest;

namespace Diplom.Command.Set
{
    public class SaveFactCommand 
    {
        public void SetFile(GetFactDataItems getFactDataItemse)
        {
            var client = ElasticSearchDataModel.Instance.Settings;
            if (client == null) return;

            var search = client
                .Search<FactResultItems>(s => s
                    .Skip(0).Take(0)
                    .FilterOn(host => host.NameList == getFactDataItemse.NameList.ToLower())
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
                    if (String.Equals(getFactDataItemse.NameList, hit.Source.NameList,
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        var delete = client.Delete(new DocumentPath<FactResultItems>(hit.Id));
                    }
                }

                AddFactItems(client, getFactDataItemse);
            }          
        }

        private void AddFactItems(ElasticClient client, GetFactDataItems getFactDataItemse)
        {
            var c = client.Index(new FactResultItems()
            {
                NameList = getFactDataItemse.NameList.ToLower(),

                ResultData = getFactDataItemse.GetFactListItems
            });
        }
    }
}

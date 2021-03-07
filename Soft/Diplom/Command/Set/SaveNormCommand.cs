using System;
using System.Linq;
using Diplom.DataBase;
using Diplom.DataBase.Infrastructure;
using Diplom.Infrastructure.Norm;
using FluentNest;
using Nest;

namespace Diplom.Command.Set
{
    public class SaveNormCommand
    {
        public void SetFile(GetNormDataItems getNormDataItemse)
        {
            var client = ElasticSearchDataModel.Instance.Settings;
            if (client == null) return;

            var search = client
                .Search<NormResultItems>(s => s
                    .Skip(0).Take(0)
                    .FilterOn(host => host.NameList == getNormDataItemse.NameList.ToLower())
                    .Aggregations(a => a
                        .SortedTopHits(1, x => x.NameList, SortType.Ascending)
                        .GroupBy(selectionList => selectionList.NameList)));

            var lists = search.Aggs.Aggregations.Count == 0
                ? null
                : search.Aggs
                    .GetGroupBy<NormResultItems>(selectionList => selectionList.NameList)
                    .SelectMany(bucket =>
                        bucket.GetSortedTopHits<NormResultItems>(selectionList => selectionList.NameList,
                            SortType.Ascending))
                    .FirstOrDefault();

            if (lists == null)
            {
                var hits = client.Search<NormResultItems>(s => s)
                    .Hits;

                foreach (var hit in hits)
                {
                    if (String.Equals(getNormDataItemse.NameList, hit.Source.NameList,
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        var delete = client.Delete(new DocumentPath<NormResultItems>(hit.Id));
                    }
                }

                AddFactItems(client, getNormDataItemse);
            }
        }

        private void AddFactItems(ElasticClient client, GetNormDataItems getNormDataItemse)
        {
            var c = client.Index(new NormResultItems()
            {
                NameList = getNormDataItemse.NameList.ToLower(),

                ResultData = getNormDataItemse.GetNormListItems
            });
        }
    }
}


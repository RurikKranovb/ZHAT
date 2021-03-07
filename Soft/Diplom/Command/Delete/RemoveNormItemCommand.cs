using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Diplom.DataBase;
using Diplom.DataBase.Infrastructure;
using Nest;

namespace Diplom.Command.Delete
{
    public class RemoveNormItemCommand
    {
        public bool Remove(string nameList)
        {
            try
            {
                var client = ElasticSearchDataModel.Instance.Settings;

                var hit = client.Search<NormResultItems>(s => s
                        .Query(q => q
                            .Bool(b => b
                                .Must(
                                    m =>
                                        m.QueryString(qs => qs
                                            .Query(nameList)))
                            )))
                    .Hits
                    .FirstOrDefault();

                var delete = client.Delete(new DocumentPath<NormResultItems>(hit?.Id));

                DelayIsExist(client, delete.Id);

                return delete.IsValid;
            }
            catch (Exception e)
            {
                return false;
            }
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
            var search = index.Search<NormResultItems>(s => s
                .AllTypes()
                .Query(p => p.Ids(d => d.Values(new List<string> { id })))
            );

            var item = search
                .Hits
                .FirstOrDefault();

            return search.IsValid && item == null;
        }
    }
}

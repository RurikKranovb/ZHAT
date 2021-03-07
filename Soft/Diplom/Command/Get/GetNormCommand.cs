using System;
using System.Collections.ObjectModel;
using Diplom.DataBase;
using Diplom.DataBase.Infrastructure;
using Diplom.Infrastructure.Norm;

namespace Diplom.Command.Get
{
   public class GetNormCommand
    {
        private ObservableCollection<GetNormDataItems> _resultItems;
        private GetNormDataItems _resultItem;

        public ObservableCollection<GetNormDataItems> GetFile()
        {
            try
            {
                _resultItems = new ObservableCollection<GetNormDataItems>();


                var client = ElasticSearchDataModel.Instance.Settings;

                var search = client
                    .Search<NormResultItems>(s => s
                        .Skip(0).Take(Int32.MaxValue));
                     


                foreach (var doc in search.Documents)
                {
                    _resultItem = new GetNormDataItems();
                    _resultItem.NameList = doc.NameList;


                    foreach (var item in doc.ResultData)
                    {
                        _resultItem.GetNormListItems.Add(new ResultNorm()
                        {
                            A = item.A,
                            B = item.B,
                            C = item.C,
                            D = item.D,
                            E = item.E,
                            F = item.F,
                            G = item.G,
                            H = item.H,
                            I = item.I,
                            J = item.J,
                            K = item.K,
                            L = item.L,
                            M = item.M
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

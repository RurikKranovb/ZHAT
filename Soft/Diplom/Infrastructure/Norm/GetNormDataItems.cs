using System.Collections.ObjectModel;
using Diplom.DataBase.Infrastructure;
using Diplom.Helper;

namespace Diplom.Infrastructure.Norm
{
   public class GetNormDataItems : ValidatableBindableBase

    { 
        public GetNormDataItems()
        {
            GetNormListItems = new ObservableCollection<ResultNorm>();
        }
        private string _nameList;

        public string NameList
        {
            get => _nameList;
            set
            {
                if (_nameList == value) return;
                _nameList = value;
                RaisePropertyChanged(() => NameList);
            }
        }

        public ObservableCollection<ResultNorm> GetNormListItems { get; }
    }
        
}

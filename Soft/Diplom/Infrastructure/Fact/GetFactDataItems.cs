using System.Collections.ObjectModel;
using Diplom.DataBase.Infrastructure;
using Diplom.Helper;

namespace Diplom.Infrastructure.Fact
{
  public  class GetFactDataItems : ValidatableBindableBase
  {
      public GetFactDataItems()
      {
          GetFactListItems = new ObservableCollection<ResultFact>();
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

      public ObservableCollection<ResultFact> GetFactListItems { get; }
  }
}

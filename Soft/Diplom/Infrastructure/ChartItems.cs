using System.Collections.ObjectModel;
using Diplom.Helper;

namespace Diplom.Infrastructure
{
    public class ChartItems:ValidatableBindableBase
    {
        public ChartItems()
        {
            ChartItemses = new ObservableCollection<ChartItem>();
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

        public ObservableCollection<ChartItem> ChartItemses { get; set; }
    }
}

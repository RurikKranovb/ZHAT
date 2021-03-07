using System.Collections.Generic;
using Diplom.Helper;
using Diplom.Infrastructure.Common;

namespace Diplom.Infrastructure.Fact
{
    public class ImportFactDataItems : ValidatableBindableBase
    {
        public ImportFactDataItems()
        {
            ImportFactListItems = new List<FactOriginalData>();
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

        public IList<FactOriginalData> ImportFactListItems { get; }
    }
}

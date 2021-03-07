using System.Collections.Generic;
using Diplom.Helper;
using Diplom.Infrastructure.Common;

namespace Diplom.Infrastructure.Norm
{
    public class ImportNormDataItems : ValidatableBindableBase
    {
        public ImportNormDataItems()
        {
            NormOriginImportItems = new List<NormOriginData>();
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

        public IList<NormOriginData> NormOriginImportItems { get; }
    }
}

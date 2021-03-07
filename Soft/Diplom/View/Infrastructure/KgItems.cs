using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Helper;

namespace Diplom.View.Infrastructure
{
    public class KgItems : ValidatableBindableBase
    {
        private string _name;

        public KgItems()
        {
            KgCollection = new ObservableCollection<double>();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)return;
                _name = value;
                RaisePropertyChanged(Name);
            }
        }

        public ObservableCollection<double> KgCollection { get; }
    }
}

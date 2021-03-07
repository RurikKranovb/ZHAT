using System;
using System.Linq.Expressions;
using Prism.Mvvm;

namespace Diplom.Helper
{
    [Serializable]
    public abstract class ValidatableBindableBase : BindableBase
    {
        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            OnPropertyChanged(propertyExpression);
        }
    }
}

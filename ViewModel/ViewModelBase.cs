using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Constructor        
        protected ViewModelBase() { }

        // INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

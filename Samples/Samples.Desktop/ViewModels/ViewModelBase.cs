using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Samples.Desktop.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected virtual bool SetProperty<T>(ref T property, T value, [CallerMemberName] string name = "")
        {
            if (EqualityComparer<T>.Default.Equals(value, property)) return false;

            property = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            OnPropertyChanged(name);
            return true;
        }

        protected virtual void OnPropertyChanged(string name)
        {

        }
    }
}

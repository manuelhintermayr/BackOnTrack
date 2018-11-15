using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BackOnTrack.Annotations;

namespace BackOnTrack.Services.ProgramConfiguration
{
    public class CurrentProgramConfiguration : INotifyPropertyChanged
    {
        private bool _proxyEnabled;
        public bool ProxyEnabled
        {
            get { return _proxyEnabled;}
            set
            {
                _proxyEnabled = value;
                OnPropertyChanged("ProxyEnabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}

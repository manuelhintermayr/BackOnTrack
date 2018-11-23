using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BackOnTrack.Annotations;

namespace BackOnTrack.Services.ProgramConfiguration
{
    public class CurrentProgramConfiguration : INotifyPropertyChanged
    {
        private bool _proxyEnabled;
        private bool _autoRunEnabled;

        public bool ProxyEnabled
        {
            get { return _proxyEnabled;}
            set
            {
                _proxyEnabled = value;
                OnPropertyChanged("ProxyEnabled");
            }
        }

        public bool AutoRunEnabled
        {
            get { return _autoRunEnabled; }
            set
            {
                _autoRunEnabled = value;
                OnPropertyChanged("AutoRunEnabled");
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

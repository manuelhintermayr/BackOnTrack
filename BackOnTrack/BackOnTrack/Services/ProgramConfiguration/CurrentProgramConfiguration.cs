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
        private int _proxyPortNumber;

        public bool ProxyEnabled
        {
            get { return _proxyEnabled;}
            set
            {
                _proxyEnabled = value;
                OnPropertyChanged("ProxyEnabled");
            }
        }

        public int ProxyPortNumber
        {
            get { return _proxyPortNumber; }
            set
            {
                _proxyPortNumber = value;
                OnPropertyChanged("ProxyPortNumber");
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

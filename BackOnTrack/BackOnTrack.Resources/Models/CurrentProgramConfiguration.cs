using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BackOnTrack.Resources.Models
{
    public class CurrentProgramConfiguration : INotifyPropertyChanged
    {
        private bool _proxyEnabled;
        private bool _autoRunEnabled;
        private int _proxyPortNumber;

        public bool ProxyEnabled
        {
            get { return _proxyEnabled; }
            set
            {
                _proxyEnabled = value;
                OnPropertyChanged("ProxyEnabled");
            }
        }
        public string ProxyPortNumber
        {
            get { return _proxyPortNumber.ToString(); }
            set
            {
                try
                {
                    _proxyPortNumber = Int32.Parse(value);
                }
                catch (Exception)
                {
                    _proxyPortNumber = 0;
                }

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

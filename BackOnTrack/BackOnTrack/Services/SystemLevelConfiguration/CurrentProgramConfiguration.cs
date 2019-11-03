using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.Services.SystemLevelConfiguration
{
    public class CurrentProgramConfiguration : INotifyPropertyChanged
    {
        #region Variables

        private bool _proxyEnabled;
        private bool _autoRunEnabled;
        private int _proxyPortNumber;
        private bool _imageReminderEnabled;
        private int _imageReminderImageWidth;
        private int _imageReminderImageHeight;

        #endregion

        #region General Settings

        public bool AutoRunEnabled
        {
            get { return _autoRunEnabled; }
            set
            {
                _autoRunEnabled = value;
                OnPropertyChanged("AutoRunEnabled");
            }
        }

        #endregion
        #region Image Reminder Settings

        public bool ImageReminderEnabled
        {
            get { return _imageReminderEnabled; }
            set
            {
                _imageReminderEnabled = value;
                OnPropertyChanged("ImageReminderEnabled");
            }
        }
        public string ImageReminderImageWidth
        {
            get { return _imageReminderImageWidth.ToString(); }
            set
            {
                try
                {
                    _imageReminderImageWidth = Int32.Parse(value);
                }
                catch (Exception)
                {
                    _imageReminderImageWidth = 0;
                }

                OnPropertyChanged("ImageReminderImageWidth");
            }
        }
        public string ImageReminderImageHeight
        {
            get { return _imageReminderImageHeight.ToString(); }
            set
            {
                try
                {
                    _imageReminderImageHeight = Int32.Parse(value);
                }
                catch (Exception)
                {
                    _imageReminderImageHeight = 0;
                }

                OnPropertyChanged("ImageReminderImageHeight");
            }
        }

        #endregion
        #region Proxy Settings

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

        #endregion

        #region Property Configuration

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}

using System.ComponentModel;

namespace BackOnTrack.Services.UserConfiguration
{
    public class HostEntry : INotifyPropertyChanged
    {
        private string _content;
        private int _lineNumber;
        private bool _isEnabled; 

        public string Content {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        public int LineNumber
        {
            get { return _lineNumber; }
            set
            {
                _lineNumber = value;
                OnPropertyChanged("LineNumber");
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
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

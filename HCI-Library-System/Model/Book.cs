using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HCI_Library_System.Model
{
    public class Book : INotifyPropertyChanged
    {
        private string _title, _author;
        private BitmapImage _imageUrl;

        public long Id {
            get; set;
        }
        public string Title {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }
        public string Author {
            get { return _author; }
            set { _author = value; NotifyPropertyChanged("Author"); }
        }
        public BitmapImage ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; NotifyPropertyChanged("ImageUrl"); }
        }

        private string _digitalCopyUrl;

        public string DigitalCopyUrl
        {
            get { return _digitalCopyUrl; }
            set
            {
                _digitalCopyUrl = value; NotifyPropertyChanged("DigitalCopyUrl");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public override string ToString()
        {
            return  Title;
        }
    }
}

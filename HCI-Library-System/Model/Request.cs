using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Library_System.Model
{
    class Request : INotifyPropertyChanged
    {
        public enum RequestState
        {
            Pending, Approved, Rejected, Overdue, Cancelled
        }
        public Book RequestedBook { get; set; }
        public User From { get; set; }
        private RequestState _state;
        public RequestState State { get { return _state; } set
            {
                _state = value; NotifyPropertyChanged("State");
            }
        }
        public DateTime RequestDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public long Id { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}

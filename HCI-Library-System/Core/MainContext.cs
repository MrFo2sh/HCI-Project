using HCI_Library_System.Database;
using HCI_Library_System.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HCI_Library_System.Core
{
    class MainContext
    {
        private static MainContext _instance;
        public static MainContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainContext();
                return _instance;
            }
        }


        public ObservableCollection<Book> BooksList { get; private set; }

        public ObservableCollection<Request> requests;

        public ObservableCollection<Book> favs;

        private MainContext()
        {
            BooksList = new ObservableCollection<Book>();
            requests = new  ObservableCollection<Request>();
            favs = new ObservableCollection<Book>();

            DatabaseManager.Instance.GetBooks().ForEach(BooksList.Add);
            //CheckExpiredRequests();
            //Insertdummy();



        }

        internal void CheckExpiredRequests()
        {
            foreach(Request req in GetAllRequests())
            {
                if(req.ExpiryDate.Date.Equals(DateTime.Now.Date) ||
                    
                    (req.ExpiryDate.Date.CompareTo(DateTime.Now.Date) > 0))
                {
                    req.State = Request.RequestState.Overdue;
                    UpdateRequest(req);
                }
            }
        }

        public static void Reset()
        {
            _instance = null;
        }

        public void AddFavorite(Book book)
        {
            favs.Add(book);
            DatabaseManager.Instance.AddFavorite(CurrentUser, book);
        }

        private void Insertdummy()
        {

          


            var image = new BitmapImage(new Uri("/HCI-Library-System;component/Resources/Book2.jpg", UriKind.Relative));

            for (int i = 0; i < 10; i++)
            {
                var book = new Book()
                {
                    Title = "Software Engineering 8 " + i,
                    Author = "Sommerville",
                    ImageUrl = image,
                    DigitalCopyUrl = i % 2 == 0 ? "test" : null
                };
                BooksList.Add(book);
                book.Id = DatabaseManager.Instance.AddBook(book);

            }
        }

        public User CurrentUser { get; set; }

        public string StartLogin(string username, string password)
        {

            User user = DatabaseManager.Instance.GetUser(username);
            if (user == null)
            {
                Console.WriteLine("null");
                return "INVALID";
            }
            else if (!password.Equals(user.Password))
            {
                Console.WriteLine("password: " + user.Password + ", entered: " + password);
                return "INVALID";
            }
            else
            {
                CurrentUser = user;
                Application.Current.Dispatcher.Invoke(() => {
                    DatabaseManager.Instance.GetRequests(CurrentUser).ForEach(requests.Add);
                    DatabaseManager.Instance.GetFavorites(CurrentUser).ForEach(favs.Add);
                });
                
                return "VALID";
            }

            
            
        }

        public void BookUpdated(Book book)
        {
            if(book.Id == -1)
            {
                book.Id = DatabaseManager.Instance.AddBook(book);
                BooksList.Add(book);
            }
            else
            {
                DatabaseManager.Instance.EditBook(book);
            }
        }

        public List<Request> GetAllRequests()
        {
            return DatabaseManager.Instance.GetAllRequests();
        }

        public void UpdateRequest(Request req)
        {
            DatabaseManager.Instance.UpdateRequestState(req);
        }



        public void Logout()
        {
            CurrentUser = null;
        }

        internal void SendRequest(Book book)
        {
            Request req = new Request()
            {
                RequestDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(7),
                From = CurrentUser,
                RequestedBook = book,
                State = Request.RequestState.Pending
            };
            requests.Add(req);
            DatabaseManager.Instance.AddRequest(req);
        }

        internal void RemoveFavorite(Book book)
        {
            favs.Remove(book);
            int index = -1;
            foreach(var b in favs)
            {
                if (book.Title.Equals(b.Title))
                    index = favs.IndexOf(b);
            }
            if(index != -1)
            {
                favs.RemoveAt(index);
            }
            DatabaseManager.Instance.RemoveFavorite(CurrentUser, book);
        }
    }
}

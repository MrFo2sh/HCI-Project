using HCI_Library_System.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HCI_Library_System.Database
{
    class DatabaseManager
    {
        private static DatabaseManager _instance;
        public static DatabaseManager Instance { get
            {
                if (_instance == null)
                    _instance = new DatabaseManager();
                return _instance;
            } }


        SQLiteConnection conn;

        private DatabaseManager()
        {
            conn = new SQLiteConnection("Data Source=Users.db");
            conn.Open();
            CreateTables();
           // RemoveAllFavorite();
        }

        private void CreateTables()
        {
            var command = conn.CreateCommand();
            //table Create
            command.CommandText = "CREATE TABLE IF NOT EXISTS users_tbl(id varchar(64) primary key, password string, FirstName Varchar(30), LastName Varchar(30), Birthdate DateTime,"
                +"email string, type int, ImageUrl string)" ;
            command.ExecuteNonQuery();

            command.CommandText = "CREATE TABLE IF NOT EXISTS books_tbl(id integer primary key autoincrement, title Varchar(64), author Varchar(64), cover string,"
             + "digitalcopy string)";
            command.ExecuteNonQuery();

            command.CommandText = "CREATE TABLE IF NOT EXISTS requests_tbl(id integer primary key autoincrement, requested_book integer, from_user varchar(64), state int,"
            + "order_date DateTime, expiry_date DateTime, FOREIGN KEY(requested_book) REFERENCES books_tbl(id),FOREIGN KEY(from_user) REFERENCES users_tbl(id)  )";
            command.ExecuteNonQuery();

            command.CommandText = "CREATE TABLE IF NOT EXISTS favs_tbl(user_id varchar(64), fav_book integer, FOREIGN KEY(user_id) REFERENCES users_tbl(id),FOREIGN KEY(fav_book) REFERENCES books_tbl(id)  )";
            command.ExecuteNonQuery();

           // InsertDummyData(command);

        }

        private void InsertDummyData(SQLiteCommand command)
        {
            try
            {
                command.CommandText = "INSERT INTO users_tbl VALUES('test1', 'pass1', 'Test','Name',DATETIME('now'), 'test@ssd.com', 0, NULL)";
                command.ExecuteNonQuery();

                var image = new BitmapImage(new Uri("/HCI-Library-System;component/Resources/Book2.jpg", UriKind.Relative));

                var book = new Book()
                {
                    Title = "Software Engineering 8",
                    Author = "Sommerville",
                    ImageUrl = image,
                    DigitalCopyUrl = null
                };

                AddBook(book);
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            try
            {
                var image = new BitmapImage(new Uri("/HCI-Library-System;component/Resources/placeholder-user.png", UriKind.Relative));

                command = conn.CreateCommand();
                command.CommandText = "INSERT INTO users_tbl VALUES('admin', 'admin', 'Mohamed','Fouad',DATETIME('now'), 'test@ssd.com', 2, '"+image.UriSource.ToString()+"')";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO users_tbl VALUES('staff', 'staff', 'Essam','Hamed',DATETIME('now'), 'tesasdast@ssd.com', 1, '" + image.UriSource.ToString() + "')";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO users_tbl VALUES('student', 'student', 'Muhammad','Khaled',DATETIME('now'), 'tesasdast@ssd.com', 0, '" + image.UriSource.ToString() + "')";
                command.ExecuteNonQuery();
            } catch(Exception ex)
            {

            }
        }

        public long AddBook(Book book)
        {
            var command = conn.CreateCommand();
            command.CommandText = "INSERT INTO books_tbl VALUES(NULL, @title, @author, @cover, @digitalcopy)";
            command.Parameters.Add(new SQLiteParameter("@title", book.Title));
            command.Parameters.Add(new SQLiteParameter("@author", book.Author));
            command.Parameters.Add(new SQLiteParameter("@cover", book.ImageUrl.UriSource.ToString()));
            command.Parameters.Add(new SQLiteParameter("@digitalcopy", (string)book.DigitalCopyUrl));
            command.ExecuteNonQuery();
            return conn.LastInsertRowId;
        }

        public long EditBook(Book book)
        {
            var command = conn.CreateCommand();
            command.CommandText = "UPDATE books_tbl set title=@title, author=@author, cover=@cover, digitalcopy=@digitalcopy WHERE id=@id;";
            command.Parameters.Add(new SQLiteParameter("@id", book.Id));
            command.Parameters.Add(new SQLiteParameter("@title", book.Title));
            command.Parameters.Add(new SQLiteParameter("@author", book.Author));
            command.Parameters.Add(new SQLiteParameter("@cover", book.ImageUrl.UriSource.ToString()));
            command.Parameters.Add(new SQLiteParameter("@digitalcopy", book.DigitalCopyUrl));
            command.ExecuteNonQuery();
            return conn.LastInsertRowId;
        }

        public long RemoveAllFavorite()
        {
            var command = conn.CreateCommand();
            command.CommandText = "DELETE FROM favs_tbl;";
            command.ExecuteNonQuery();
            return conn.LastInsertRowId;
        }

        public long RemoveFavorite(User user, Book book)
        {
            var command = conn.CreateCommand();
            command.CommandText = "DELETE FROM favs_tbl WHERE user_id=@user_id AND fav_book=@book_id;";
            command.Parameters.Add(new SQLiteParameter("@user_id", user.Username));
            command.Parameters.Add(new SQLiteParameter("@book_id", book.Id));
            command.ExecuteNonQuery();
            return conn.LastInsertRowId;
        }


        public long AddRequest(Request req)
        {
            var command = conn.CreateCommand();
            command.CommandText = "INSERT INTO requests_tbl VALUES(NULL, @requested_book, @from_user, @state, @order_date, @expiry_date)";
            command.Parameters.Add(new SQLiteParameter("@requested_book", req.RequestedBook.Id));
            command.Parameters.Add(new SQLiteParameter("@from_user", req.From.Username));
            command.Parameters.Add(new SQLiteParameter("@state", (int)req.State));
            command.Parameters.Add(new SQLiteParameter("@order_date", req.RequestDate));
            command.Parameters.Add(new SQLiteParameter("@expiry_date", req.ExpiryDate));
            command.ExecuteNonQuery();
            return conn.LastInsertRowId;
        }

        public long AddFavorite(User user, Book book)
        {
            var command = conn.CreateCommand();
            command.CommandText = "INSERT INTO favs_tbl VALUES('"+user.Username+"', "+book.Id+");";
            command.ExecuteNonQuery();
            return conn.LastInsertRowId;
        }

        public List<Book> GetFavorites(User user)
        {
            List<Book> books = new List<Book>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM favs_tbl WHERE user_id = '"+user.Username+"';";


            SQLiteDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                books.Add(GetBook(sdr.GetInt64(1)));
            }
            sdr.Close();
            return books;
        }

        public long UpdateRequestState(Request req)
        {
            var command = conn.CreateCommand();
            command.CommandText = "UPDATE requests_tbl set state=@state WHERE id=@id";
            command.Parameters.Add(new SQLiteParameter("@id", req.Id));
            command.Parameters.Add(new SQLiteParameter("@state", (int)req.State));
            command.ExecuteNonQuery();
            return conn.LastInsertRowId;
        }

        public List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM books_tbl;";
            SQLiteDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                var book = new Book();
                book.Id = sdr.GetInt64(0);
                book.Title = sdr.GetString(1);
                book.Author = sdr.GetString(2);
                book.ImageUrl = new BitmapImage(new Uri(sdr.GetString(3), UriKind.RelativeOrAbsolute));
                book.DigitalCopyUrl = ConvertFromDBVal<string>(sdr.GetValue(4));
                books.Add(book);
            }
            sdr.Close();
            return books;
        }

        public Book GetBook(long id)
        {
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM books_tbl WHERE id="+id+";";
            SQLiteDataReader sdr = command.ExecuteReader();
            var book = new Book();
            while (sdr.Read())
            {
                book.Id = sdr.GetInt64(0);
                book.Title = sdr.GetString(1);
                book.Author = sdr.GetString(2);
                book.ImageUrl = new BitmapImage(new Uri(sdr.GetString(3), UriKind.RelativeOrAbsolute));
                book.DigitalCopyUrl = ConvertFromDBVal<string>(sdr.GetValue(4));
            }
            sdr.Close();
            return book;
        }

        public User GetUser(string username)
        {
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM users_tbl WHERE id='" + username + "';";
            SQLiteDataReader sdr = command.ExecuteReader();
            var user = new User();
            while (sdr.Read())
            {
                user.Username = sdr.GetString(0);
                if (user.Username == null)
                    return null;
                user.Password = sdr.GetString(1);
                user.FirstName = sdr.GetString(2);
                user.LastName = sdr.GetString(3);
                user.Birthdate = sdr.GetDateTime(4);
                user.Email = sdr.GetString(5);
                user.Type = (User.UserType) sdr.GetInt32(6);
                user.ImageUrl = ConvertFromDBVal<string>(sdr.GetString(7));
            }
            sdr.Close();
            return user;
        }

        public List<Request> GetAllRequests()
        {
            List<Request> requests = new List<Request>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM requests_tbl;";
            SQLiteDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                var req = new Request();
                req.Id = sdr.GetInt64(0);
                req.RequestedBook = GetBook(sdr.GetInt64(1));
                req.From = GetUser(sdr.GetString(2));
                req.State = (Request.RequestState)sdr.GetInt32(3);
                req.RequestDate = sdr.GetDateTime(4);
                req.ExpiryDate = sdr.GetDateTime(5);
                requests.Add(req);
            }
            sdr.Close();
            return requests;
        }

        public List<Request> GetRequests(User user)
        {
            List<Request> requests = new List<Request>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM requests_tbl WHERE from_user='"+user.Username+"';";
            SQLiteDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                var req = new Request();
                req.Id = sdr.GetInt64(0);
                req.RequestedBook = GetBook(sdr.GetInt64(1));
                req.From = user;
                req.State = (Request.RequestState)sdr.GetInt32(3);
                req.RequestDate = sdr.GetDateTime(4);
                req.ExpiryDate = sdr.GetDateTime(5);
                requests.Add(req);
            }
            sdr.Close();
            return requests;
        }

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
    }
}

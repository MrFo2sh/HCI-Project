using HCI_Library_System.Core;
using HCI_Library_System.Database;
using HCI_Library_System.Model;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_Library_System.Pages
{
    /// <summary>
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class Books : UserControl
    {

        private readonly CollectionViewSource viewSource = new CollectionViewSource();

        private ContextMenu ctxMenuStudent;
        private ContextMenu ctxMenuTeacher;
        private ContextMenu ctxMenuAdmin;
        private MenuItem showDigitalCopy1;
        private MenuItem showDigitalCopy2;
        private MenuItem favBook1;
        private MenuItem favBook2;

        public Books()
        {
            InitializeComponent();
            DataContext = this;

            SetupContextMenus();

            switch(MainContext.Instance.CurrentUser.Type)
            {
                case User.UserType.Admin:
                    listBooks.ContextMenu = ctxMenuAdmin;
                    break;
                case User.UserType.Teacher:
                    listBooks.ContextMenu = ctxMenuTeacher;
                    break;
                case User.UserType.Student:
                    listBooks.ContextMenu = ctxMenuStudent;
                    btnAdd.Visibility = Visibility.Hidden;
                    break;
            }



            listBooks.ItemsSource = MainContext.Instance.BooksList;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listBooks.ItemsSource);
            view.Filter = BooksFilter;
        }


        private void SetupContextMenus()
        {
            
            MenuItem item1 = new MenuItem() { Header = "Request book" };
            item1.Click += requestBook_Click;

            favBook1 = new MenuItem() { Header = "Favorite book" };
            favBook1.Click += favoriteBook_Click;

            MenuItem item3 = new MenuItem() { Header = "Edit book" };
            item3.Click += editBook_Click;

            showDigitalCopy1 = new MenuItem() { Header = "Show Digital Copy" };
            showDigitalCopy1.Click += ShowDigitalCopy_Click;
            showDigitalCopy2 = new MenuItem() { Header = "Show Digital Copy" };
            showDigitalCopy2.Click += ShowDigitalCopy_Click;



            ctxMenuStudent = new ContextMenu();
            ctxMenuStudent.Items.Add(item1);
            ctxMenuStudent.Items.Add(favBook1);
            ctxMenuStudent.Items.Add(showDigitalCopy1);

            item1 = new MenuItem() { Header = "Request book" };
            item1.Click += requestBook_Click;

            favBook2 = new MenuItem() { Header = "Favorite book" };
            favBook2.Click += favoriteBook_Click;

            ctxMenuTeacher = new ContextMenu();
            ctxMenuTeacher.Items.Add(item1);
            ctxMenuTeacher.Items.Add(favBook2);
            ctxMenuTeacher.Items.Add(showDigitalCopy2);

            ctxMenuAdmin = new ContextMenu();
            ctxMenuAdmin.Items.Add(item3);
        }

        private void ShowDigitalCopy_Click(object sender, RoutedEventArgs e)
        {
            new DigitalCopyWindow(listBooks.SelectedItem as Book).Show();
        }

        private void requestBook_Click(object sender, RoutedEventArgs e)
        {
            RequestBook(listBooks.SelectedItem as Book);
        }

        private void favoriteBook_Click(object sender, RoutedEventArgs e)
        {
            if(favBook1.Header.Equals("Unfavorite"))
            {
                MainContext.Instance.RemoveFavorite(listBooks.SelectedItem as Book);
                MainWindow.Instance.ShowAppBar("Book unfavorited");
            }
            else
            {
                MainContext.Instance.AddFavorite(listBooks.SelectedItem as Book);
                MainWindow.Instance.ShowAppBar("Book favorited");
            }

        }

        private void editBook_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowEditBookFlyout(listBooks.SelectedItem as Book);
        }

        private bool BooksFilter(object item)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
                return true;
            return ((item as Book).Title.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                || ((item as Book).Author.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private ICommand textBoxButtonCmd;

        public ICommand TextBoxButtonCmd
        {
            get
            {
                return this.textBoxButtonCmd ?? (this.textBoxButtonCmd = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x =>
                    {
                        if (x is string)
                        {
                            StartSearch(x as string);
                        }
                        else
                        {
                            StartSearch("");
                        }
  
                    }
                });
            }
        }

        private void StartSearch(string searchText)
        {
            CollectionViewSource.GetDefaultView(listBooks.ItemsSource).Refresh();
        }

        private void listBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainContext.Instance.CurrentUser.Type == User.UserType.Admin) return;
            RequestBook(listBooks.SelectedItem as Book);
        }

        private void RequestBook(Book book)
        {
            MainContext.Instance.SendRequest(book);
            MainWindow.Instance.ShowAppBar("Book requested");
        }
    

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            CollectionViewSource.GetDefaultView(listBooks.ItemsSource).Refresh();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowEditBookFlyout(null);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBooks.ItemsSource).Refresh();
        }

        private void listBooks_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if(listBooks.SelectedItem == null)
            {
                e.Handled = true;
            }
            else
            {
                Book book = listBooks.SelectedItem as Book;
                bool isFav = false;
                foreach(Book b in MainContext.Instance.favs)
                {
                    if(book.Id == b.Id)
                    {
                        favBook1.Header = "Unfavorite";
                        favBook2.Header = "Unfavorite";
                        isFav = true;
                        break;
                    }
                }

                if(!isFav)
                {
                    favBook1.Header = "Favorite book";
                    favBook2.Header = "Favorite book";
                }

                if(book.DigitalCopyUrl == null)
                {
                    showDigitalCopy1.IsEnabled = false;
                    showDigitalCopy2.IsEnabled = false;
                }
                else
                {
                    showDigitalCopy1.IsEnabled = true;
                    showDigitalCopy2.IsEnabled = true;
                }
            }
        }
    }

    public class SimpleCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);
            return true; // if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parameter);
        }
    }
}

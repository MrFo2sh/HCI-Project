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
using MahApps.Metro.Controls;
using MahApps.Metro;
using System.Runtime.InteropServices;
using System.Globalization;
using HCI_Library_System.Pages;
using HCI_Library_System.Core;
using HCI_Library_System.Model;

namespace HCI_Library_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();
        public static MainWindow Instance { get; private set; }


        private Book currentBook;
        public MainWindow()
        {
            Instance = this;
            AllocConsole();
            InitializeComponent();
            SwitchToLoginPage();
        }

        public void LoggedIn()
        {
            SwitchToMainPage();

            ShowAppBar("Welcome, " + MainContext.Instance.CurrentUser.FirstName);

        }

        public void ShowAppBar(string text)
        {
            welcomeFlyout.Header = text;
            welcomeFlyout.IsOpen = true;
        }

        internal void ShowWarningFlyout(User user)
        {
            warning_To.Text = user.Email +" ("+user.Username +")";
            warning_Title.Text = "Warning for overdue book";
            warningFlyout.IsOpen = true;
        }

        public void ShowEditBookFlyout(Book book)
        {
            if(book == null)
            {
                currentBook = new Book();
                currentBook.Id = -1;
                editBook_bookTitle.Text = "";
                editBook_bookAuthor.Text = "";
                editBook_bookCover.Text ="";
                editBook_bookDigitalCopy.Text = "";
            }
            else
            {
                currentBook = book;
                editBook_bookTitle.Text = book.Title;
                editBook_bookAuthor.Text = book.Author;
                editBook_bookCover.Text = book.ImageUrl.UriSource.ToString();
                editBook_bookDigitalCopy.Text = book.DigitalCopyUrl;
            }
            editBookFlyout.IsOpen = true;
        }

        private void SwitchToMainPage()
        {
            btnLogout.Visibility = Visibility.Visible;
            transitioning.Content = new MainPage();
        }

        private void SwitchToLoginPage()
        {
            MainContext.Reset();
            var m= MainContext.Instance;
            btnLogout.Visibility = Visibility.Hidden;
            transitioning.Content = new LoginPage();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainContext.Instance.Logout();
            SwitchToLoginPage();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editBook_Save_Click(object sender, RoutedEventArgs e)
        {
            if(editBook_bookTitle.Text.Length == 0)
            {
                MessageBox.Show("Please enter title");
                return;
            }
            if (editBook_bookAuthor.Text.Length == 0)
            {
                MessageBox.Show("Please enter author");
                return;
            }
            if (editBook_bookCover.Text.Length == 0)
            {
                MessageBox.Show("Please enter cover url");
                return;
            }
            currentBook.Title = editBook_bookTitle.Text;
            currentBook.Author = editBook_bookAuthor.Text;
            currentBook.ImageUrl = new BitmapImage(new Uri(editBook_bookCover.Text, UriKind.RelativeOrAbsolute));
            currentBook.DigitalCopyUrl = editBook_bookDigitalCopy.Text;
            MainContext.Instance.BookUpdated(currentBook);
            editBookFlyout.IsOpen = false;
        }

        private void editBook_cancel_Click(object sender, RoutedEventArgs e)
        {
            editBookFlyout.IsOpen = false;
        }

        private void warning_Cancel_Click(object sender, RoutedEventArgs e)
        {
            warningFlyout.IsOpen = false;
        }

        private void warning_Send_Click(object sender, RoutedEventArgs e)
        {
            warningFlyout.IsOpen = false;
            ShowAppBar("Email sent!");
        }
    }


}

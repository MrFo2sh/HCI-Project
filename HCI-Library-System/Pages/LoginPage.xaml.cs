using HCI_Library_System.Core;
using HCI_Library_System.Database;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        private static readonly string LOGIN_TEXT = "Login";
        private static readonly string CANCEL_TEXT = "Cancel";

        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            var dd = DatabaseManager.Instance;
        }

        private async void Login()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            var b = MainContext.Instance.BooksList; //DO NOT REMOVE
            string result = await Task.Run(() =>
            {
                return MainContext.Instance.StartLogin(username, password);
            });
            if (result.Equals("VALID"))
            {
                MainWindow.Instance.LoggedIn();

            }
            else
            {
                SetNormal();
                ShowError("Incorrect username or password");
            }

        }

        private void ShowError(string error)
        {
            lblError.Content = error;
            panelError.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            panelError.Visibility = Visibility.Hidden;
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (btnLogin.Content.Equals(LOGIN_TEXT))
            {
                ForceValidations();
                if (!IsFormValid())
                {
                    if (txtPassword.Password.Length == 0)
                    {
                        MessageBox.Show("Please enter a password");
                    }
                    return;
                }
                SetLogginIn();
            }
            else
            {
                SetNormal();
            }




        }

        private void SetNormal()
        {
            HideError();
            btnLogin.Content = LOGIN_TEXT;

            lblForgotPassword.Visibility = Visibility.Visible;
            progressLogin.Visibility = Visibility.Hidden;
            txtPassword.IsEnabled = true;
            txtUsername.IsEnabled = true;
        }

        private void SetLogginIn()
        {
            HideError();
            btnLogin.Content = CANCEL_TEXT;

            lblForgotPassword.Visibility = Visibility.Hidden;
            progressLogin.Visibility = Visibility.Visible;
            txtPassword.IsEnabled = false;
            txtUsername.IsEnabled = false;
            Login();
        }


        private void ForgotPassword_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnLogin_Click(this, null);
        }


        private bool IsFormValid()
        {
            return (!txtUsername.GetBindingExpression(TextBox.TextProperty).HasError) &&
                txtPassword.Password.Length != 0;
        }


        private void ForceValidations()
        {
            txtUsername.GetBindingExpression(TextBox.TextProperty).UpdateSource();

        }
    }
}

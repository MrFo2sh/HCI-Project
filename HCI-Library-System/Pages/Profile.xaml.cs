using HCI_Library_System.Core;
using HCI_Library_System.Database;
using HCI_Library_System.Model;
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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();
            User user = MainContext.Instance.CurrentUser;
            lblId.Text = user.Username;
            lblName.Text = user.FirstName + " " + user.LastName;
            lblEmail.Content = user.Email;
            lblDob.Content = user.Birthdate.ToShortDateString();
            var image = new BitmapImage(new Uri(user.ImageUrl, UriKind.RelativeOrAbsolute));
            imgUser.Source = image;
            if(user.Type == User.UserType.Admin)
            {
                panelFavs.Visibility = Visibility.Hidden;
            }
            else
            {
                listFavorites.ItemsSource = MainContext.Instance.favs;
            }

            ContextMenu ctx = new ContextMenu();
            MenuItem item1 = new MenuItem() { Header = "Remove" };
            item1.Click += Item1_Click;
            ctx.Items.Add(item1);
          //  listFavorites.ContextMenu = ctx;

        }

        private void Item1_Click(object sender, RoutedEventArgs e)
        {
            string title = listFavorites.SelectedItem as string;
            MainContext.Instance.RemoveFavorite(MainContext.Instance.favs.Where(x => x.Title.Equals(title)).FirstOrDefault());
        }
    }
}

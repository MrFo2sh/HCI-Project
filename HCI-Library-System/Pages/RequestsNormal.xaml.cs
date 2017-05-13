using HCI_Library_System.Core;
using HCI_Library_System.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for RequestsNormal.xaml
    /// </summary>
    public partial class RequestsNormal : UserControl
    {
        ContextMenu ctx;
        
        public RequestsNormal()
        {
            InitializeComponent();
            
            listRequests.ItemsSource = MainContext.Instance.requests;
            ctx = new ContextMenu();
            MenuItem item1 = new MenuItem() { Header = "Cancel" };
            item1.Click += Item1_Click;
            ctx.Items.Add(item1);
        }

        private void Item1_Click(object sender, RoutedEventArgs e)
        {
            var req = listRequests.SelectedItem as Request;
            req.State = Request.RequestState.Cancelled;
            MainContext.Instance.UpdateRequest(req);
        }

        private void listRequests_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var req = listRequests.SelectedItem as Request;
            listRequests.ContextMenu = null;
            if (req == null || req.State != Request.RequestState.Pending)
            {
                e.Handled = true;
            }
            else
            {
                listRequests.ContextMenu = ctx;
            }
        }
    }
}

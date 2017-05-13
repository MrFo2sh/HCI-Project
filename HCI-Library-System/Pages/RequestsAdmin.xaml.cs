using HCI_Library_System.Core;
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
    /// Interaction logic for RequestsAdmin.xaml
    /// </summary>
    public partial class RequestsAdmin : UserControl
    {
        private ContextMenu ctxApproveReject;
        private ContextMenu ctxWarning;
        public RequestsAdmin()
        {
            InitializeComponent();
            gridRequests.ItemsSource = MainContext.Instance.GetAllRequests();
            
            ctxApproveReject = new ContextMenu();
            var item1 = new MenuItem() { Header = "Approve" };
            item1.Click += Approve_Click;
            var item2 = new MenuItem() { Header = "Reject" };
            item2.Click += Reject_Click;

            ctxApproveReject.Items.Add(item1);
            ctxApproveReject.Items.Add(item2);

            ctxWarning = new ContextMenu();
            var item3 = new MenuItem() { Header = "Send warning e-mail" };
            item3.Click += Warning_Click;
            ctxWarning.Items.Add(item3);
        }

        private void Warning_Click(object sender, RoutedEventArgs e)
        {
            var req = gridRequests.SelectedItem as Request;
            Console.WriteLine("clicked warning");
            MainWindow.Instance.ShowWarningFlyout(req.From);
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            ApproveRequest(gridRequests.SelectedItem as Request);
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            RejectRequest(gridRequests.SelectedItem as Request);
        }

        private void gridRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(gridRequests.SelectedItems.Count == 0)
            {
                btnApprove.IsEnabled = false;
                btnReject.IsEnabled = false;
                return;
            }
            foreach(var selected in gridRequests.SelectedItems)
            {
                Request req = selected as Request;
                if(req.State != Request.RequestState.Pending)
                {
                    btnApprove.IsEnabled = false;
                    btnReject.IsEnabled = false;
                    return;
                }
            }
            btnApprove.IsEnabled = true;
            btnReject.IsEnabled = true;
        }

        private void ApproveRequest(Request req)
        {
            req.State = Request.RequestState.Approved;
            MainContext.Instance.UpdateRequest(req);
        }

        private void RejectRequest(Request req)
        {
            req.State = Request.RequestState.Rejected;
            MainContext.Instance.UpdateRequest(req);
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            foreach(var r in gridRequests.SelectedItems)
            {
                ApproveRequest(r as Request);
            }
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            foreach (var r in gridRequests.SelectedItems)
            {
                RejectRequest(r as Request);
            }

        }

        private void gridRequests_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            gridRequests.ContextMenu = null;
            if(gridRequests.SelectedItem == null)
            {
                e.Handled = true;
                return;
            }

            Request req = gridRequests.SelectedItem as Request;
            if(req.State == Request.RequestState.Pending)
            {
                gridRequests.ContextMenu = ctxApproveReject;
                gridRequests.ContextMenu.IsOpen = true;
            }
            else if(req.State == Request.RequestState.Overdue)
            {
                gridRequests.ContextMenu = ctxWarning;
                gridRequests.ContextMenu.IsOpen = true;
            }

        }
    }
}

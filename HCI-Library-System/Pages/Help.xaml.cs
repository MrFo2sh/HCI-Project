using HCI_Library_System.Core;
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
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : UserControl
    {
        public Help()
        {
            InitializeComponent();
            if (MainContext.Instance.CurrentUser.Type == Model.User.UserType.Admin)
            {
                Content = new HelpAdmin();
            }
            else if (MainContext.Instance.CurrentUser.Type == Model.User.UserType.Teacher)
            {
                Content = new HelpTeacher();
            }
            else
            {
                Content = new HelpStudent();
            }
        }
    }
}

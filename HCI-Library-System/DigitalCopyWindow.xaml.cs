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
using System.Windows.Shapes;
using HCI_Library_System.Model;

namespace HCI_Library_System
{
    /// <summary>
    /// Interaction logic for DigitalCopyWindow.xaml
    /// </summary>
    public partial class DigitalCopyWindow : MetroWindow
    {
        public DigitalCopyWindow(Book book)
        {
            InitializeComponent();
            webBrowser.Url = new Uri(book.DigitalCopyUrl);
        }
    }
}

using HCI_Library_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HCI_Library_System.Pages
{
    class BooksListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WithoutDigitalCopyTemplate { get; set; }
        public DataTemplate WithDigitalCopyTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Book book = item as Book;
            if(book.DigitalCopyUrl == null)
            {
                return WithoutDigitalCopyTemplate;
            }
            return WithDigitalCopyTemplate;
        }
    }
}

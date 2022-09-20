using RestaurantManager.BusinessModels.OrderTicket;
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

namespace RestaurantManager.UserInterface.PosReports.Sales
{
    /// <summary>
    /// Interaction logic for SalesReport.xaml
    /// </summary>
    public partial class SalesReport : Page
    {
        public SalesReport()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create a PrintDialog  
            PrintDialog printDlg = new PrintDialog();
            // Create a FlowDocument dynamically.  
            FlowDocument doc = CreateFlowDocument();
            doc.Name = "FlowDoc";
            // Create IDocumentPaginatorSource from FlowDocument  
            IDocumentPaginatorSource idpSource = doc;
            // Call PrintDocument method to send document to printer  
             printDlg.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing."); 
        }

        private FlowDocument CreateFlowDocument()
        {
            // Create a FlowDocument  
            FlowDocument doc = new FlowDocument();
            // Create a Section  
            Section sec = new Section();
            // Create first Paragraph  
            Paragraph p1 = new Paragraph();
            // Create and add a new Bold, Italic and Underline  
            Bold bld = new Bold();
            bld.Inlines.Add(new Run("First Paragraph"));
            Italic italicBld = new Italic();
            italicBld.Inlines.Add(bld);
            Underline underlineItalicBld = new Underline();
            underlineItalicBld.Inlines.Add(italicBld);
            // Add Bold, Italic, Underline to Paragraph  
            p1.Inlines.Add(underlineItalicBld);
            // Add Paragraph to Section  
            sec.Blocks.Add(p1);
            // Add Section to FlowDocument  
            doc.Blocks.Add(sec);
            return doc;
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new PosDbContext();
                db.OrderItem.AsNoTracking();
                var mainlist = db.OrderItem.ToList();
                var distlist = mainlist.Distinct(new MyComparer());
                List<Un_OrderItem> displist = new List<Un_OrderItem>();
                foreach (var x in distlist)
                {
                    Un_OrderItem item = new Un_OrderItem();
                    int qty = mainlist.Where(k => k.ParentProductItemGuid == x.ParentProductItemGuid).Count();
                    decimal total = mainlist.Where(k => k.ParentProductItemGuid == x.ParentProductItemGuid).Sum(l => l.Quantity * l.Price);
                    item.ParentProductItemGuid = x.ParentProductItemGuid;
                    item.ItemName = x.ItemName;
                    item.Quantity = qty;
                    item.Total = total;
                    displist.Add(item);
                }
                Datagrid_OrderItems.ItemsSource = displist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public class MyComparer : IEqualityComparer<OrderItem>
        {
            public bool Equals(OrderItem x, OrderItem y)
            {
                // compare multiple fields
                return  x.ParentProductItemGuid == y.ParentProductItemGuid;
            }

            public int GetHashCode(OrderItem obj)
            {
                return
                    obj.ParentProductItemGuid.GetHashCode();
            }
             
        }
    }
}

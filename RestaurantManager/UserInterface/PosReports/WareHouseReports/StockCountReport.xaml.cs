using DatabaseModels.CRM;
using DatabaseModels.OrderTicket;
using DatabaseModels.Payments;
using DatabaseModels.Security;
using DatabaseModels.Inventory;
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;

namespace RestaurantManager.UserInterface.PosReports.WareHouseReports
{
    /// <summary>
    /// Interaction logic for StockCountReport.xaml
    /// </summary>
    public partial class StockCountReport : Page
    {
        readonly ObservableCollection<dynamic> MainList = new ObservableCollection<dynamic>(); 
        List<MenuProductItem> MainList_ProductItems = new List<MenuProductItem>(); 
        public StockCountReport()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Datagrid_ProductItems.ItemsSource = MainList;
            Combobox_Department.ItemsSource = Enum.GetNames(typeof(PosEnums.Departments));
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new PosDbContext();
                //load Items 
                MainList.Clear();
                Label_TotalItems_Count.Content = "0";
                MainList_ProductItems = db.MenuProductItem.AsNoTracking().ToList();
                 
                var pitems = (from oi in db.OrderItem.AsNoTracking()
                              join d in db.OrderMaster.AsNoTracking() on oi.OrderID equals d.OrderNo 
                              where d.OrderNo==oi.OrderID
                              select new
                              {
                                  oi.ProductItemGuid,
                                  oi.ItemName, 
                                  oi.OrderID,
                                  ItemQuantity = oi.Quantity,
                                  TicketStatus=d.OrderStatus
                              }).ToList();
                // MessageBox.Show(pitems.Count().ToString(), pitems.Where(l=>l.TicketStatus!=PosEnums.OrderTicketStatuses.Completed.ToString()).Count().ToString());

                if (Combobox_Isprecountablle.SelectedItem!=null)
                {
                    var selectedboolvalue = ((ComboBoxItem)Combobox_Isprecountablle.SelectedItem).Content.ToString();

                    if (selectedboolvalue == "True")
                    {
                        MainList_ProductItems.RemoveAll(r => r.IsPrecount == false);
                    }
                    else if (selectedboolvalue == "False")
                    {
                        MainList_ProductItems.RemoveAll(r => r.IsPrecount == true);
                    }
                }
                if (Combobox_Department.SelectedItem!=null)
                {
                    MainList_ProductItems.RemoveAll(r => r.Department != Combobox_Department.SelectedItem.ToString());
                } 
                foreach (var pitem in MainList_ProductItems)
                {
                    DateTime Lastdate = DateTime.Now;
                    decimal soldqty = 0;
                    decimal Receivedqty = 0; 
                    if(db.StockFlowTransaction.AsNoTracking().Where(k => k.ProductGuid == pitem.ProductGuid && k.FlowDirection == "IN").Count() > 0)
                    {
                        Receivedqty = db.StockFlowTransaction.AsNoTracking().Where(k => k.ProductGuid == pitem.ProductGuid && k.FlowDirection == "IN").Sum(s => s.Quantity);
                    }
                    if(pitems.Where(k => k.ProductItemGuid == pitem.ProductGuid && k.TicketStatus == PosEnums.OrderTicketStatuses.Completed.ToString()).Count() > 0)
                    {
                        soldqty = pitems.Where(k => k.ProductItemGuid == pitem.ProductGuid && k.TicketStatus == PosEnums.OrderTicketStatuses.Completed.ToString()).Sum(s => s.ItemQuantity);
                    }

                    //find the items sold count

                    if (pitem.IsPrecount)
                    {
                        MainList.Add(new
                        {
                            pitem.ProductName,
                            pitem.ProductGuid,
                            Sold = soldqty,
                            Received = Receivedqty,
                            Instock = Receivedqty - soldqty,
                            LastStockDate = Lastdate,
                        });
                    }
                    else
                    {
                        MainList.Add(new
                        {
                            pitem.ProductName,
                            pitem.ProductGuid,
                            Sold = soldqty,
                            Received = 0,
                            Instock = 0,
                            LastStockDate = Lastdate,
                        });
                    }
                }
                Label_TotalItems_Count.Content = MainList.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_ProductItems.ItemsSource == null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_ProductItems.ItemsSource);
                if (filter == "")
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = new Predicate<object>(Contains);
                }
                //if (filter == "")
                //    cv.Filter = null;
                //else
                //{
                //    cv.Filter = o =>
                //    {
                //        MenuProductItem p = o as MenuProductItem;
                //        return p.ProductName.ToLower().Contains(filter.ToLower()) || p.AvailabilityStatus.ToString().ToLower().Contains(filter.ToLower());
                //    };
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool Contains(object de)
        {  
            System.Type type = de.GetType();
            string itemname = (string)type.GetProperty("ProductName").GetValue(de, null);
            return itemname.ToString().ToLower().Contains(Textbox_SearchBox.Text.ToLower());

        }

        private void Button_ViewAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new PosDbContext();
                //load Items 
                MainList.Clear();
                Label_TotalItems_Count.Content = "0";
                Combobox_Department.SelectedItem = null;
                Combobox_Isprecountablle.SelectedItem = null;
                Textbox_SearchBox.Text = "";
                MainList_ProductItems = db.MenuProductItem.AsNoTracking().ToList();

                var pitems = (from oi in db.OrderItem.AsNoTracking()
                              join d in db.OrderMaster.AsNoTracking() on oi.OrderID equals d.OrderNo
                              where d.OrderNo == oi.OrderID
                              select new
                              {
                                  oi.ProductItemGuid,
                                  oi.ItemName, 
                                  oi.OrderID,
                                  ItemQuantity = oi.Quantity,
                                  TicketStatus = d.OrderStatus
                              }).ToList();
                // MessageBox.Show(pitems.Count().ToString(), pitems.Where(l=>l.TicketStatus!=PosEnums.OrderTicketStatuses.Completed.ToString()).Count().ToString());
                foreach (var pitem in MainList_ProductItems)
                {
                    DateTime Lastdate = DateTime.MinValue;
                    int soldqty = 0;
                    int Receivedqty = 0;
                    if (db.StockFlowTransaction.AsNoTracking().Where(k => k.ProductGuid == pitem.ProductGuid && k.FlowDirection == "IN").Count() > 0)
                    {
                        Receivedqty = (int)db.StockFlowTransaction.AsNoTracking().Where(k => k.ProductGuid == pitem.ProductGuid && k.FlowDirection == "IN").Sum(s => s.Quantity);
                        Lastdate = db.StockFlowTransaction.AsNoTracking().First(k => k.ProductGuid == pitem.ProductGuid && k.FlowDirection == "IN").TransactionDate;
                    }
                    if (pitems.Where(k => k.ProductItemGuid == pitem.ProductGuid && k.TicketStatus == PosEnums.OrderTicketStatuses.Completed.ToString()).Count() > 0)
                    {
                        soldqty = pitems.Where(k => k.ProductItemGuid == pitem.ProductGuid && k.TicketStatus == PosEnums.OrderTicketStatuses.Completed.ToString()).Sum(s => s.ItemQuantity);
                    }

                    //find the items sold count
                    if (pitem.IsPrecount)
                    {
                        MainList.Add(new
                        {
                            pitem.ProductName,
                            pitem.ProductGuid,
                            Sold = soldqty,
                            Received = Receivedqty,
                            Instock = Receivedqty - soldqty,
                            LastStockDate = Lastdate,
                        });
                    }
                    else
                    {
                        MainList.Add(new
                        {
                            pitem.ProductName,
                            pitem.ProductGuid,
                            Sold = soldqty,
                            Received = 0,
                            Instock = 0,
                            LastStockDate = Lastdate,
                        });
                    }
                    
                }
                Label_TotalItems_Count.Content = MainList.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private void Button_Print_Click(object sender, RoutedEventArgs e)
        {

            try
            { 
                // Create a new PDF document
                PdfDocument document = new PdfDocument();

                // Add a page to the document
                PdfPage page = document.AddPage();

                // Create a drawing object for the page
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Create a font
                XFont font = new XFont("Arial", 12);

                // Draw text on the page
                gfx.DrawString("Hello, PDFsharp!", font, XBrushes.Black,
                    new XRect(10, 10, page.Width, page.Height), XStringFormats.TopLeft);

                // Save the document to a file
                //document.Save("D:\\example.pdf");
            }
            catch
            {
                MessageBox.Show("error");
            }
        }  
    }
}

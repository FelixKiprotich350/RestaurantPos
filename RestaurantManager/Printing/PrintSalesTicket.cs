using DatabaseModels.Inventory;
using DatabaseModels.OrderTicket;
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using winforms = System.Windows;

namespace RestaurantManager.Printing
{
    public class PrintSalesTicket
    {

        public string Printtype = "";
        public string TicketNo = "";
        public decimal GrossTotal = 0;
        public DateTime? TicketDate = DateTime.MinValue;
        List<OrderItem> Receiptitems = new List<OrderItem>();
        private bool IsTicketPrepared = false;
        private List<string> errors = new List<string>();
         
        public void PrepareTicketInfo(string printtype, string ticketno, decimal grosstotal, DateTime dateTime, List<OrderItem> ticketitems)
        {
            try
            {
                if (printtype != "" && ticketno != "" && grosstotal != 0 && dateTime != DateTime.MinValue && ticketitems.Count > 0)
                {
                    Printtype = printtype;
                    TicketNo = ticketno;
                    GrossTotal = grosstotal;
                    TicketDate = dateTime;
                    Receiptitems = ticketitems;
                    IsTicketPrepared = true;
                    Print();
                }

                else
                {
                    errors.Add("All info not provided!");
                }

            }
            catch (Exception exception1)
            {
                winforms.MessageBox.Show(exception1.Message, "Failed to print Receipt", winforms.MessageBoxButton.OK, winforms.MessageBoxImage.Error);
            }
        }

        private void Print()
        {
            try
            {
                if (!IsTicketPrepared)
                {
                    return;
                }
                PrintDocument document = new PrintDocument();
                if (Printtype == "Kitchen")
                {
                    document.PrintPage += new PrintPageEventHandler(ProvideContentForKitchenorder);
                }
                else if (Printtype == "Ticket")
                {
                    document.PrintPage += new PrintPageEventHandler(ProvideContentForFullTicket);
                }
                else
                {
                    return;
                }
                document.PrintController = new StandardPrintController();
                //document.PrinterSettings.PrintFileName = "Ticket.pdf";
                //document.PrinterSettings.PrintToFile = true; 

                document.Print();

            }
            catch (Exception exception1)
            {
                winforms.MessageBox.Show(exception1.Message, "Failed to print Receipt", winforms.MessageBoxButton.OK, winforms.MessageBoxImage.Error);
            }
        }

        private void ProvideContentForFullTicket(object sender, PrintPageEventArgs e)
        {
            int Center_X = 150;
            Graphics graphics = e.Graphics;
            //begin receipt
            int topoffset = 10;
            StringFormat format1 = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            StringFormat format = format1;
            graphics.DrawString(SharedVariables.ClientInfo().ClientTitle.ToUpper(), new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().PhysicalAddress, new Font("Arial", 10f), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString("Ticket Invoice", new Font("Palatino Linotype", 15f, FontStyle.Bold), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            graphics.DrawString("____________", new Font("Palatino Linotype", 15f), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString("TicketNo:" + this.TicketNo, new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Date:" + SharedVariables.CurrentDate().ToShortDateString(), new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)topoffset);
            graphics.DrawString("Counter : " + SharedVariables.LogInCounter, new Font("Arial", 10f), new SolidBrush(Color.Black), 120f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Time : " + SharedVariables.CurrentDate().ToShortTimeString(), new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)topoffset);
            graphics.DrawString("Served By: " + SharedVariables.CurrentUser.UserFullName, new Font("Arial", 10f), new SolidBrush(Color.Black), 120f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("Item                              Qty Price    Total", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 10f, (float)topoffset);
            graphics.DrawString("______________________________________", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            foreach (OrderItem ord_i in Receiptitems)
            {
                if (ord_i.ItemName.ToString().Length <= 0x1f)
                {
                    graphics.DrawString(ord_i.ItemName.ToString(), new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
                }
                else
                {
                    Array array = ord_i.ItemName.ToString().ToCharArray(0, 30);
                    string s = "";
                    int index = 0;
                    while (true)
                    {
                        string text1;
                        if (index >= array.Length)
                        {
                            graphics.DrawString(s, new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
                            break;
                        }
                        object obj1 = array.GetValue(index);
                        if (obj1 != null)
                        {
                            text1 = obj1.ToString();
                        }
                        else
                        {
                            object local1 = obj1;
                            text1 = null;
                        }
                        s += text1;
                        index++;
                    }
                }
                topoffset += 15;
                string[] textArray1 = new string[] { "                                                     ", ord_i.Quantity.ToString().Trim(), " *  ", ((int)ord_i.Price).ToString(), "   ", ((int)ord_i.Total).ToString() };
                graphics.DrawString(string.Concat(textArray1), new Font("Arial", 8f), new SolidBrush(Color.Black), 0f, (float)topoffset);
                topoffset += 15;
            }
            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 15;
            graphics.DrawString("TOTAL :", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 50f, (float)topoffset);
            graphics.DrawString(GrossTotal.ToString("N2"), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Black), 150f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 40;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote1, new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 30;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote2, new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
        }

        private void ProvideContentForKitchenorder(object sender, PrintPageEventArgs e)
        {
            int Center_X = 150;
            List<OrderItem> kitchenitems = new List<OrderItem>();
            var db = new PosDbContext();
            List<MenuProductItem> menu = db.MenuProductItem.AsNoTracking().ToList();
            foreach (var m in Receiptitems)
            {
                var menuitem = menu.FirstOrDefault(k => k.ProductGuid == m.ProductItemGuid);
                if (menuitem != null)
                {
                    if (menuitem.Department == PosEnums.Departments.Restaurant.ToString() && menuitem.IsPrecount == false)
                    {
                        kitchenitems.Add(m);
                        //Debug.WriteLine(m.ItemName);
                    }
                }
            }

            if (kitchenitems.Count <= 0)
            {
                e.Cancel = true;
                return;
            }

            Graphics graphics = e.Graphics;
            //begin receipt
            int topoffset = 10;
            StringFormat format1 = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            StringFormat format = format1;
            graphics.DrawString("Kitchen Order", new Font("Palatino Linotype", 15f, FontStyle.Bold), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            graphics.DrawString("____________", new Font("Palatino Linotype", 15f), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString("Ticket No:" + TicketNo, new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Ticket Time:" + TicketDate.ToString(), new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Served By: " + SharedVariables.CurrentUser.UserFullName, new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("Item                              Qty   Price ", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 10f, (float)topoffset);
            graphics.DrawString("______________________________________", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            foreach (OrderItem ord_i in kitchenitems)
            {
                if (ord_i.ItemName.ToString().Length <= 0x1f)
                {
                    graphics.DrawString(ord_i.ItemName.ToString(), new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
                }
                else
                {
                    Array array = ord_i.ItemName.ToString().ToCharArray(0, 30);
                    string s = "";
                    int index = 0;
                    while (true)
                    {
                        string text1;
                        if (index >= array.Length)
                        {
                            graphics.DrawString(s, new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
                            break;
                        }
                        object obj1 = array.GetValue(index);
                        if (obj1 != null)
                        {
                            text1 = obj1.ToString();
                        }
                        else
                        {
                            object local1 = obj1;
                            text1 = null;
                        }
                        s += text1;
                        index++;
                    }
                }
                topoffset += 15;
                string[] textArray1 = new string[] { "                                                     ", ord_i.Quantity.ToString().Trim(), " *  ", ((int)ord_i.Price).ToString(), "   " };
                graphics.DrawString(string.Concat(textArray1), new Font("Arial", 8f), new SolidBrush(Color.Black), 0f, (float)topoffset);
                topoffset += 15;
            }
            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Printing Time:" + SharedVariables.CurrentDate().ToString(), new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)topoffset);

        }

    }
}

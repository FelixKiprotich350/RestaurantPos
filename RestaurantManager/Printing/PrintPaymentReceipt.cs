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
    public class PrintPaymentReceipt
    {
        public string R_receiptno = "";
        public decimal R_totalpaid = 0;
        public decimal R_TotalCharged = 0;
        public decimal R_balance = 0;
        public DateTime? TicketDate = DateTime.MinValue;
        private bool IsTicketPrepared = false;
        public List<OrderItem> Receiptitems = new List<OrderItem>();
        private List<string> errors = new List<string>();

        public PrintPaymentReceipt()
        {
            ///initializer
        }

        public void PrepareReceiptInfoAsync()
        {
            try
            {
                if (R_receiptno!="")
                {
                  
                    IsTicketPrepared = true; 
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

        public void ProvideContentforsalesreceipt(object sender, PrintPageEventArgs e)
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
            graphics.DrawString(SharedVariables.ClientInfo().Phone, new Font("Arial", 10f), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().Email, new Font("Arial", 10f), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().ClientKRAPIN.ToUpper(), new Font("Arial", 12f), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString("Payment Receipt", new Font("Palatino Linotype", 12f, FontStyle.Bold), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            graphics.DrawString("____________", new Font("Palatino Linotype", 15f), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 15;
            graphics.DrawString("Receipt No:" + R_receiptno, new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)topoffset);
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
                string[] textArray1 = new string[] { "                                                     ", ord_i.Quantity.ToString().Trim(), " *  ", ord_i.Price.ToString(), "   ", decimal.Parse(ord_i.Total.ToString()).ToString("N2") };
                graphics.DrawString(string.Concat(textArray1), new Font("Arial", 8f), new SolidBrush(Color.Black), 0f, (float)topoffset);
                topoffset += 15;
            }
            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 15;
            graphics.DrawString("TOTAL :", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 50f, (float)topoffset);
            graphics.DrawString(R_TotalCharged.ToString("N2"), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Black), 150f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Amount Paid :", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 50f, (float)topoffset);
            graphics.DrawString(R_totalpaid.ToString("N2"), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Black), 150f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Balance", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 50f, (float)topoffset);
            graphics.DrawString(R_balance.ToString("N2"), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Black), 150f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 15;
            graphics.DrawString("Tax%        TaxAmt", new Font("Arial", 10f, FontStyle.Underline), new SolidBrush(Color.Black), 70f, (float)topoffset);
            topoffset += 15;
            graphics.DrawString(SharedVariables.ClientInfo().TaxPercentage.ToString(), new Font("Arial", 10f), new SolidBrush(Color.Black), 80f, (float)topoffset);
            graphics.DrawString((SharedVariables.ClientInfo().TaxPercentage / 100 * R_TotalCharged).ToString("N2"), new Font("Arial", 10f), new SolidBrush(Color.Black), 135f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote1, new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote1, new Font("Arial", 10f, FontStyle.Italic), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 15;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote1, new Font("Arial", 8f), new SolidBrush(Color.Black), (float)Center_X, (float)topoffset, format);
        }

        public async Task PrintReceipt()
        {
            try
            {
                if (!IsTicketPrepared)
                {
                    return;
                }
                await Task.Run(() =>
                {
                    PrintDocument document = new PrintDocument();
                    document.PrintPage += new PrintPageEventHandler(this.ProvideContentforsalesreceipt);
                    document.PrintController = new StandardPrintController();
                    //document.PrinterSettings.PrintFileName = "Receipt.pdf";
                    //document.PrinterSettings.PrintToFile = true;
                    document.Print();
                });
            }
            catch (Exception exception1)
            {
                winforms.MessageBox.Show(exception1.Message, "Failed to print Receipt", winforms.MessageBoxButton.OK, winforms.MessageBoxImage.Error);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Printing
{
    class PrintTicket
    {
        public void PrintReceipt()
        {
            try
            {
                PrintDocument document = new PrintDocument();
              //  document.PrintPage += new PrintPageEventHandler(this.ProvideContent);
                document.PrintController = new StandardPrintController();
                document.Print();
            }
            catch (Exception exception1)
            {
                System.Windows.MessageBox.Show(exception1.Message, "Failed to print Receipt");
            }
        }

        //public void ProvideContent(object sender, PrintPageEventArgs e)
        //{
        //    ReceiptData data = new ReceiptData();
        //    int num2 = 0;
        //    while (true)
        //    {
        //        if (num2 >= this.Cart_Gridview.Rows.Count)
        //        {
        //            Graphics graphics = e.Graphics;
        //            int num = 10;
        //            StringFormat format1 = new StringFormat();
        //            format1.LineAlignment = StringAlignment.Center;
        //            format1.Alignment = StringAlignment.Center;
        //            StringFormat format = format1;
        //            graphics.DrawString(Title.ToUpper(), new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //            num += 20;
        //            graphics.DrawString(Box, new Font("Arial", 10f), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //            num += 20;
        //            graphics.DrawString(Tel, new Font("Arial", 10f), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //            num += 20;
        //            graphics.DrawString(Email, new Font("Arial", 10f), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //            num += 20;
        //            graphics.DrawString(Pin.ToUpper(), new Font("Arial", 12f), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //            num += 20;
        //            graphics.DrawString("Sales Receipt", new Font("Palatino Linotype", 15f, FontStyle.Bold), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //            graphics.DrawString("____________", new Font("Palatino Linotype", 15f), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //            num += 15;
        //            graphics.DrawString("BillNo:" + this.TransactionCode, new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)num);
        //            num += 20;
        //            graphics.DrawString("Date:" + Program.CurrentDateTime().ToShortDateString(), new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)num);
        //            graphics.DrawString("Counter : " + Program.LogInCounter, new Font("Arial", 10f), new SolidBrush(Color.Black), 120f, (float)num);
        //            num += 20;
        //            graphics.DrawString("Time : " + Program.CurrentDateTime().ToShortTimeString(), new Font("Arial", 10f, FontStyle.Regular), new SolidBrush(Color.Black), 10f, (float)num);
        //            graphics.DrawString("Served By: " + Program.CurrLoggedInUser.UserFirstname, new Font("Arial", 10f), new SolidBrush(Color.Black), 120f, (float)num);
        //            num += 10;
        //            graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)num);
        //            num += 10;
        //            graphics.DrawString("Item                              Qty Price    Total", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 10f, (float)num);
        //            graphics.DrawString("______________________________________", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)num);
        //            num += 20;
        //            int num3 = 0;
        //            while (true)
        //            {
        //                if (num3 >= data.DataTable1.Rows.Count)
        //                {
        //                    graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 10f, (float)num);
        //                    num += 15;
        //                    graphics.DrawString("TOTAL :", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 50f, (float)num);
        //                    graphics.DrawString(this.GrossTotal.ToString("N2"), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Black), 150f, (float)num);
        //                    num += 20;
        //                    graphics.DrawString("Amount Paid :", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 50f, (float)num);
        //                    graphics.DrawString(this.AmountPaid.ToString("N2"), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Black), 150f, (float)num);
        //                    num += 20;
        //                    graphics.DrawString("Balance", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), 50f, (float)num);
        //                    graphics.DrawString(this.Balance.ToString("N2"), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Black), 150f, (float)num);
        //                    num += 10;
        //                    graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)num);
        //                    num += 15;
        //                    graphics.DrawString("Tax%        TaxAmt", new Font("Arial", 10f, FontStyle.Underline), new SolidBrush(Color.Black), 70f, (float)num);
        //                    num += 15;
        //                    graphics.DrawString(TaxPercentage.ToString(), new Font("Arial", 10f), new SolidBrush(Color.Black), 80f, (float)num);
        //                    graphics.DrawString(this.TaxAmt.ToString(), new Font("Arial", 10f), new SolidBrush(Color.Black), 135f, (float)num);
        //                    num += 10;
        //                    graphics.DrawString("----------------------------------------------------------------", new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)num);
        //                    num += 20;
        //                    graphics.DrawString(Text1, new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //                    num += 20;
        //                    graphics.DrawString(Text2, new Font("Arial", 10f, FontStyle.Italic), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //                    num += 15;
        //                    graphics.DrawString(Text3, new Font("Arial", 8f), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //                    num += 15;
        //                    graphics.DrawString(Text4, new Font("Arial", 8f), new SolidBrush(Color.Black), (float)this.Center_X, (float)num, format);
        //                    return;
        //                }
        //                if (data.DataTable1.Rows[num3][0].ToString().Length <= 0x1f)
        //                {
        //                    graphics.DrawString(data.DataTable1.Rows[num3][0].ToString(), new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)num);
        //                }
        //                else
        //                {
        //                    Array array = data.DataTable1.Rows[num3][0].ToString().ToCharArray(0, 30);
        //                    string s = "";
        //                    int index = 0;
        //                    while (true)
        //                    {
        //                        string text1;
        //                        if (index >= array.Length)
        //                        {
        //                            graphics.DrawString(s, new Font("Arial", 10f), new SolidBrush(Color.Black), 10f, (float)num);
        //                            break;
        //                        }
        //                        object obj1 = array.GetValue(index);
        //                        if (obj1 != null)
        //                        {
        //                            text1 = obj1.ToString();
        //                        }
        //                        else
        //                        {
        //                            object local1 = obj1;
        //                            text1 = null;
        //                        }
        //                        s = s + text1;
        //                        index++;
        //                    }
        //                }
        //                num += 15;
        //                string[] textArray1 = new string[] { "                                                     ", data.DataTable1.Rows[num3][1].ToString().Trim(), " *  ", data.DataTable1.Rows[num3][2].ToString(), "   ", decimal.Parse(data.DataTable1.Rows[num3][3].ToString()).ToString("N2") };
        //                graphics.DrawString(string.Concat(textArray1), new Font("Arial", 8f), new SolidBrush(Color.Black), 0f, (float)num);
        //                num += 15;
        //                num3++;
        //            }
        //        }
        //        object[] values = new object[] { this.Cart_Gridview.Rows[num2].Cells[1].Value.ToString(), this.Cart_Gridview.Rows[num2].Cells[3].Value.ToString(), this.Cart_Gridview.Rows[num2].Cells[4].Value.ToString(), this.Cart_Gridview.Rows[num2].Cells[5].Value.ToString() };
        //        data.DataTable1.Rows.Add(values);
        //        num2++;
        //    }
        //}
    }
}

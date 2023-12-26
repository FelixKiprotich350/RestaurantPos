using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DatabaseModels.Accounts;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;  
using System.IO; 
using System.Reflection;   
using System.Drawing.Printing;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Isharptext = iTextSharp.text.Font;
using RestaurantManager.ActivityLogs;
using RestaurantManager.GlobalVariables;

namespace RestaurantManager.UserInterface.Accounts.InvoiceAccount
{
    /// <summary>
    /// Interaction logic for FullAccountStatement.xaml
    /// </summary>
    public partial class FullAccountStatement1 : Window
    {
        decimal totalinvoice = 0;
        decimal totalpaid = 0;
        decimal balance = 0;
        InvoicableAccount InvAccount = null;
        public FullAccountStatement1(InvoicableAccount account)
        {
            InitializeComponent();
            InvAccount = account;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                List<AccountStatementRecord> statementrecord = new List<AccountStatementRecord>();
                var db = new PosDbContext();
                var invmaster = db.InvoicesMaster.AsNoTracking().Where(k => k.CustomerAccNo == InvAccount.PersonAccNo).ToList();
                Textbox_AccountNumber.Text = InvAccount.PersonAccNo;
                Textbox_FullName.Text = InvAccount.FullName;
                Textbox_Status.Text = InvAccount.AccountStatus;

                foreach (var x in invmaster)
                {
                    statementrecord.Add(new AccountStatementRecord()
                    {
                        InvNo = x.InvoiceNo,
                        PayRefNo = "N/A",
                        ServedBy = x.SystemUser,
                        TransactionDate = x.InvoiceDate,
                        Debit = x.InvoiceAmount,
                        Credit = 0,
                        Balance = 0
                    }); ;
                    var invpayments = db.TicketPaymentItem.AsNoTracking().Where(k => k.ParentSourceRef == x.InvoiceNo && k.PayForSource == GlobalVariables.PosEnums.PaymentForSources.InvoicePay.ToString()).ToList();
                    foreach (var y in invpayments)
                    {
                        statementrecord.Add(new AccountStatementRecord()
                        {
                            InvNo = y.ParentSourceRef,
                            PayRefNo = y.PaymentGuid.ToString(),
                            TransactionDate = y.PaymentDate,
                            Debit = 0,
                            ServedBy="N/A",
                            Credit = y.AmountUsed,
                            Balance = 0
                        });

                    }
                }
                statementrecord.OrderByDescending(k => k.TransactionDate);
                foreach (var x in statementrecord)
                {
                    totalinvoice += x.Debit;
                    totalpaid += x.Credit;
                    balance = totalpaid - totalinvoice;
                    x.Balance = balance;
                }
                Textbox_TotalInvoiceAmount.Text = totalinvoice.ToString("N2");
                Textbox_TotalPaidAmount.Text = totalpaid.ToString("N2");
                Textbox_TotalPendingBalance.Text = balance.ToString("N2");
                Datagrid_AccountStatement.ItemsSource = statementrecord;
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Viewed invoice statement", "account no=" + InvAccount.PersonAccNo);
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

               

            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void GeneratePDF()
        {
            try
            {
                string filePath = "";
                System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
                if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (fb.SelectedPath != "")
                    {
                        filePath = fb.SelectedPath + "\\InvoiceStatement-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                    }
                    else
                    {
                        MessageBox.Show("Your selected Path is Empty!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You didn't select any Path!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

                PdfPTable pdfTable = new PdfPTable(Datagrid_AccountStatement.Columns.Count);
                pdfTable.DefaultCell.Padding = 2;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                Isharptext HeaderFont = new Isharptext(Isharptext.FontFamily.TIMES_ROMAN, 12, 1);
                Isharptext cellcontentfont = new Isharptext(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 0);
                Isharptext cellcontentboldfont = new Isharptext(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1);
                foreach (DataGridColumn column in Datagrid_AccountStatement.Columns)
                {

                    PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString(), HeaderFont));
                    pdfTable.AddCell(cell);
                }


                foreach (var item in Datagrid_AccountStatement.Items)
                {
                    foreach (PropertyInfo prop in item.GetType().GetProperties())
                    {
                        object propValue = prop.GetValue(item); // Get the value of each property
                        Console.WriteLine($"Property: {prop.Name}, Value: {propValue}");
                        pdfTable.AddCell(new Phrase(propValue.ToString(), cellcontentfont));
                    }
                }
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document pdfDoc1 = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                    iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc1, stream);
                    pdfDoc1.Open();
                    pdfDoc1.AddTitle("Invoicable Account Statement");
                    pdfDoc1.Add(new iTextSharp.text.Paragraph("Full Account Statement.") { Alignment = Element.ALIGN_CENTER });
                    pdfDoc1.Add(new iTextSharp.text.Paragraph("Account Number : " + Textbox_AccountNumber.Text) { Alignment = Element.ALIGN_LEFT });
                    pdfDoc1.Add(new iTextSharp.text.Paragraph("Full Name : " + Textbox_FullName.Text) { Alignment = Element.ALIGN_LEFT });
                    pdfDoc1.Add(new iTextSharp.text.Paragraph(" "));
                    pdfDoc1.Add(pdfTable);
                    pdfDoc1.Add(new iTextSharp.text.Paragraph("Total Pending Balance : " + balance.ToString("N2"), cellcontentboldfont) { Alignment = Element.ALIGN_RIGHT });
                    pdfDoc1.Add(new iTextSharp.text.Paragraph($"Printing Date: {DateTime.Now.ToString()}") { Alignment = Element.ALIGN_CENTER });
                    pdfDoc1.Add(new iTextSharp.text.Paragraph("This is a System generated Document. Do not edit any part of this document!", new Isharptext(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 2)) { Alignment = Element.ALIGN_CENTER });
                    pdfDoc1.Close();
                    stream.Close();
                }

                MessageBox.Show("Statement Exported Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Exported Invoice Statement", "account no=" + InvAccount.PersonAccNo);
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_ExportPdf_Click(object sender, RoutedEventArgs e)
        {
            GeneratePDF();
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

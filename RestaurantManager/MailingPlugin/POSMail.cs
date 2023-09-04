using DatabaseModels.GeneralSettings;
using RestaurantManager.ApplicationFiles; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantManager.MailingPlugin
{

    public class POSMail
    {
        public POSMail()
        {

        }
        MailMessage message;
        SmtpClient smtp;
        public   async Task<bool> SendWPClosureMail(string Message, string Subject, bool IsMessageHtml)
        {
            bool final = false;
            try
            {
                MailingProfile mp = null;
                using (var db=new PosDbContext())
                {
                    mp = db.MailingProfile.FirstOrDefault();
                }
                if(mp is null)
                {
                    return false;
                }
                message = new MailMessage();
                message.To.Add(mp.DestinationAddress);
                // message.CC.Add(txtCC.Text);
                message.Subject = Subject;
                message.From = new MailAddress(mp.SenderAddress, mp.DisplayName,Encoding.ASCII);
                message.Body = Message;
                message.IsBodyHtml = IsMessageHtml;
                //message.Attachments.Add(new Attachment(lblAttachment.Text));
                // set smtp details
                smtp = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = true,
                    Port = 587,
                    Credentials = new NetworkCredential(mp.MailingAddress, mp.AppPassword),
                    EnableSsl = true
                };
               await smtp.SendMailAsync(message);
                
                smtp.SendCompleted += new SendCompletedEventHandler(Smtp_SendCompleted);
               // await Task.CompletedTask;
                final = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Message Box",MessageBoxButton.OK,MessageBoxImage.Error);
                final = false;
            }
            return final;
        }

        void Smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {

            if (e.Cancelled == true)
            {
                MessageBox.Show("Email sending cancelled!");
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            smtp.SendAsyncCancel();
            MessageBox.Show("Email sending cancelled!");
        }
       
    }
}

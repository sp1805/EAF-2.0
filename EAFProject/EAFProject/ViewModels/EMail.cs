using System;
using System.Net.Mail;

namespace EAFProject.ViewModels
{
    public class EMail
    {
        
        public static void sendmail(string From, string To,string Subject,string Body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.prod.lclad.com", 25);
            smtpClient.UseDefaultCredentials = false;
           
            try
            {

                MailMessage mailMessage = new MailMessage(From, To, Subject, Body);
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                Console.Write("E-mail sent!");
            }
            catch (Exception ex)
            {
                Console.Write("Could not send the e-mail - error: " + ex.Message);
                Console.Write(ex.StackTrace);
            }
        }
    }
}
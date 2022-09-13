using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Rocky;
using Rocky_Utility;


public class EmailSender
{
    public static void SendEmail(string messagebody)
    {
        try
        {
            // Credentials
            var credentials = new NetworkCredential(WC.AdminEmail, WC.AdminPassword);
            // Mail message
            var mail = new MailMessage()
            {
                From = new MailAddress(WC.AdminEmail),
                Subject = "New Inquiry",
                
                Body = messagebody
            };
 
            mail.To.Add(new MailAddress(WC.CustomerEmail));
 
            // Smtp client
            var client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };
            // Send it...         
            client.Send(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in sending email: " + ex.Message);
        }
        Console.WriteLine($"Email for {WC.AdminEmail} successfully sent to {WC.CustomerEmail}");
        // Console.ReadKey();
    }
}
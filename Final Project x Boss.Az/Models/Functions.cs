using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.NotificationNamespace;
namespace Final_Project_x_Boss.Az.Models
{
    internal static class Functions
    {
        public static void SendMail(string toMail, Notification notification)
        {

            MailMessage message = new MailMessage();
            message.From = new MailAddress("consolebossaz@gmail.com");
            message.To.Add(new MailAddress(toMail));
            message.IsBodyHtml = true;
            message.Body = $"<html><body> {notification.LongData()} </body></html>";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("consolebossaz@gmail.com", "tpwveajrwdjqrqsf"),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }
        static public int Print<T>(List<T> arr, ref int x, ref int y)
        {
            int index = 0;
            while (true)
            {
                //Console.Clear();
                for (int i = 0; i < arr.Count; i++)
                {
                    if (i == index) Console.ForegroundColor = ConsoleColor.DarkGreen;
                    else Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(x, y + i);
                    Console.WriteLine(arr[i]);
                }
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (index == 0) index = arr.Count - 1;
                    else index--;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (index == arr.Count - 1) index = 0;
                    else index++;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    return index;
                }
            }
        }

    }

}

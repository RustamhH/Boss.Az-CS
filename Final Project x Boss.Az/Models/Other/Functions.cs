using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.NotificationNamespace;
using Final_Project_x_Boss.Az.Models.UserNamespace;
using Final_Project_x_Boss.Az.Models.WorkerNamespace;
using Final_Project_x_Boss.Az.Models.Services;
using Final_Project_x_Boss.Az.Models.EmployerNamespace;
using Final_Project_x_Boss.Az.Models.AdminNamespace;

namespace Final_Project_x_Boss.Az.Models.Other
{
    internal static class Functions
    {
        public static void SendMail(string toMail, Notification notification)
        {

            MailMessage message = new MailMessage();
            message.From = new MailAddress("consolebossaz@gmail.com");
            message.To.Add(new MailAddress(toMail));
            message.Subject = notification.Title;
            message.IsBodyHtml = true;
            message.Body = $"<html><body> {notification.Content} </body></html>";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("consolebossaz@gmail.com", "tpwveajrwdjqrqsf"),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }
        public static int Print<T>(List<T> arr, ref int x, ref int y)
        {
            int index = 0;
            while (true)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    if (i == index)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
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
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return index;
                }
            }
        }


        public static KeyValuePair<string, string> FixEmailPassword()
        {
            string email, password;
            do
            {
                Console.Write("Enter E-Mail: ");
                email = Console.ReadLine();
            } while (email == null || email == "");
            do
            {
                Console.Write("Enter Password: ");
                password = Console.ReadLine();
            } while (password == null || password == "");

            return new(email, password);
        }


        public static string FixId()
        {
            string? id;
            do
            {
                Console.Write("Enter id to delete worker: ");
                id = Console.ReadLine();
            } while (id == null || id == "");
            return id;
        }




        




        public static void UserRegistration(ref Database database, User user)
        {
            string name, surname, username, email, password, city, phone;
            double realbudget;
            int realage, registercode;
            do
            {
                Console.Write("Enter Name: ");
                name = Console.ReadLine();
            } while (name == null || name == "");
            do
            {
                Console.Write("Enter Surname: ");
                surname = Console.ReadLine();
            } while (surname == null || surname == "");
            do
            {
                Console.Write("Enter Age: ");
            } while (!int.TryParse(Console.ReadLine(), out realage));
            do
            {
                Console.Write("Enter Username: ");
                username = Console.ReadLine();
            } while (username == null || username == "");
            do
            {
                Console.Write("Enter City: ");
                city = Console.ReadLine();
            } while (city == null || city == "");
            do
            {
                Console.Write("Enter Phone Number: ");
                phone = Console.ReadLine();
            } while (phone == null || phone == "");
            do
            {
                Console.Write("Enter Budget: ");
            } while (!double.TryParse(Console.ReadLine(), out realbudget));

            KeyValuePair<string, string> pair = FixEmailPassword();
            email = pair.Key;
            password = pair.Value;
            Random random = new();
            int randint = random.Next(100000, 1000000);
            SendMail(email, new("Your Verification Code", randint.ToString(), database.DefaultAdmin));
            do
            {
                Console.Write("Enter registration code:");
            } while (!int.TryParse(Console.ReadLine(), out registercode));
            if (randint == registercode)
            {
                try
                {
                    user.Name = name;
                    user.Surname = surname;
                    user.Age = realage;
                    user.Username = username;
                    user.City = city;
                    user.Phone = phone;
                    user.Budget = realbudget;
                    user.Email = email;
                    user.Password = password;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey(true);
                    return;
                }
                // processlere,adminin notif,usere mail
                if (database.RegisterUser(user))
                {
                    SendMail(email, new("Welcome", "Your have registered to your Boss.Az account successfully", database.DefaultAdmin));
                    database.DefaultAdmin!.Notifications!.Add(new("New Register", $"{username} has registered to Boss.Az", user));
                    database.DefaultAdmin.AddProcess(new($"{username} has registered to Boss.Az"));
                }
                else
                {
                    Console.WriteLine("You already registered to Boss.Az");
                    Console.ReadKey(true);
                    return;
                }

            }
            else
            {
                Console.WriteLine("Verification code isn't correct");
                Console.ReadKey(true);
                return;
            }
        }

        public static void CVCreation(ref Database database,Worker worker)
        {
            Categories category;
            string profession, school, gitlink, linkedin;
            int skillcount, companycount, languagecount;
            bool diplom;
        }

        

    }

}

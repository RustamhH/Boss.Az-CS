﻿using System;
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
    internal static partial class Functions
    {
        public static partial void SendMail(string toMail, Notification notification);
        public static partial int Print<T>(List<T> arr, int x, int y);
        public static partial KeyValuePair<string, string> FixEmailPassword();
        public static partial string FixId();
        public static partial void UserRegistration(ref Database database, User user);
        public static partial void VacancySearchAlgorithm(ref Database database, bool isLong = true);
        public static partial void CVSearchAlgorithm(ref Database database, bool isLong = true);
    }



    internal static partial class Functions
    {
        public static partial void SendMail(string toMail, Notification notification)
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
            Console.WriteLine("Mail Sending ...");
            smtpClient.Send(message);
        }
        public static partial int Print<T>(List<T> arr, int x, int y)
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
        public static partial KeyValuePair<string, string> FixEmailPassword()
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
        public static partial string FixId()
        {
            string? id;
            do
            {
                Console.Write("Enter id: ");
                id = Console.ReadLine();
            } while (id == null || id == "");
            return id;
        }
        public static partial void UserRegistration(ref Database database, User user)
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
            SendMail(email, new("Your Verification Code", randint.ToString(), database.DefaultAdmin.Username));
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
                    Notification notification = new("Welcome", "Your have registered to your Boss.Az account successfully", database.DefaultAdmin.Username);
                    SendMail(email, notification);
                    user.Notifications!.Add(notification);
                    database.DefaultAdmin!.Notifications!.Add(new("New Register", $"{username} has registered to Boss.Az", user.Username));
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
        public static partial void VacancySearchAlgorithm(ref Database database, bool isLong = true)
        {
            int searchx = 100, searchy = 2;
            int filterornot = Print(new List<string> { "Use filter", "Don't use filter" }, searchx, searchy);
            Console.ForegroundColor = ConsoleColor.White;
            if (filterornot == 0)
            {
                while (true)
                {

                    int filteringchoice = Print(new List<string> { "By Category", "By Salary", "By Experience Time", "By Premium", "Exit" }, searchx, searchy);
                    if (filteringchoice == 0)
                    {
                        Categories category;
                        int CategoryChoice = Print(Enum.GetNames(typeof(Categories)).ToList(), searchx, searchy);
                        Enum.TryParse(CategoryChoice.ToString(), out category);
                        database.ShowVacancies(category, isLong);
                    }
                    else if (filteringchoice == 1)
                    {
                        double salary;
                        do
                        {
                            Console.Write("Enter Minimum Salary: ");
                        } while (!double.TryParse(Console.ReadLine(), out salary));
                        database.ShowVacancies(salary, isLong);
                    }
                    else if (filteringchoice == 2)
                    {
                        int extime;
                        do
                        {
                            Console.Write("Enter Minimum Experience Time: ");
                        } while (!int.TryParse(Console.ReadLine(), out extime));
                        database.ShowVacancies(extime, isLong);
                    }
                    else if (filteringchoice == 3)
                    {
                        database.ShowPremiumVacancies(isLong);
                    }
                    else VacancySearchAlgorithm(ref database);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                database.ShowVacancies(isLong);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public static partial void CVSearchAlgorithm(ref Database database, bool isLong = true)
        {
            int searchx = 100, searchy = 2;
            int filterornot = Print(new List<string> { "Use filter", "Don't use filter" }, searchx, searchy);
            Console.ForegroundColor = ConsoleColor.White;
            if (filterornot == 0)
            {
                while (true)
                {

                    int filteringchoice = Print(new List<string> { "By Category", "By Salary", "By Premium", "Exit" }, searchx, searchy);
                    if (filteringchoice == 0)
                    {
                        Categories category;
                        int CategoryChoice = Print(Enum.GetNames(typeof(Categories)).ToList(), searchx, searchy);
                        Enum.TryParse(CategoryChoice.ToString(), out category);
                        database.ShowCVs(category, isLong);
                        Console.ReadKey(true);
                    }
                    else if (filteringchoice == 1)
                    {
                        double salary;
                        do
                        {
                            Console.Write("Enter Minimum Salary: ");
                        } while (!double.TryParse(Console.ReadLine(), out salary));
                        database.ShowCVs(salary, isLong);
                        Console.ReadKey(true);
                    }
                    else if (filteringchoice == 2)
                    {
                        database.ShowPremiumCVs(isLong);
                        Console.ReadKey(true);
                    }
                    else CVSearchAlgorithm(ref database);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                database.ShowCVs(isLong);
                Console.ForegroundColor = ConsoleColor.White;

            }
        }
    }

}

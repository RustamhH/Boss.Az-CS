using System.Net.Mail;
using System.Net;
using Final_Project_x_Boss.Az.Models.WorkerNamespace;
using Final_Project_x_Boss.Az.Models.UserNamespace;
using static Final_Project_x_Boss.Az.Models.Other.Functions;
using Final_Project_x_Boss.Az.Models.AdminNamespace;
using Final_Project_x_Boss.Az.Models.EmployerNamespace;
using System.Reflection;
using Final_Project_x_Boss.Az.Models.Other;
using System.Xml;

namespace Final_Project_x_Boss.Az
{



    internal class Program
    {
         
        

        static void Main(string[] args)
        {

            
            Database database = new();
            int mainx = 49, mainy = 13;



            //Worker worker = new("Rustem","Hesenli",22,"RustamH","rustamh2006@gmail.com","rustam2006","Baku","0513940859",1111);
            //Employer employer = new("Hesen","Abdullazade",21,"HasanHttps","hasanabdullazad@gmail.com","hasan2222","Baku","0554876502",8123);


            //database.AddEmployer(employer);
            //database.AddWorker(worker);


            // Employer
                // EditVacancy,ApplyToCV
            // Worker
                // EditCV,ApplyToVacancy




            while (true)
            {
                int MainChoice = Print(new List<string> { "Admin", "User", "Job Postings", "Exit" }, ref mainx, ref mainy);

                if (MainChoice == 0)
                {
                    while (true)
                    {
                        
                        int SignChoice = Print(new List<string> { "Login", "Exit" }, ref mainx, ref mainy);
                        if (SignChoice == 0)
                        {
                            Admin admin = new();
                            try
                            {
                                KeyValuePair<string, string> pair = FixEmailPassword();
                                admin.Email = pair.Key;
                                admin.Password = pair.Value;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.ReadKey(true);
                                Console.Clear();
                                break;
                            }
                            if (database.LoginAdmin(admin))
                            {

                                while (true)
                                {
                                    int AdminChoice = Print(new List<string> { "Notifications", "Show User", "Delete User", "Check Deadlines", "Check CVs", "Check Vacancies", "Show Previous Processes", "My Account", "Exit" }, ref mainx, ref mainy);
                                    if (AdminChoice == 0)
                                    {
                                        database.CurrentAdmin!.ShowMyNotifications();
                                        Console.ReadKey(true);
                                    }
                                    else if (AdminChoice == 1)
                                    {

                                        while (true)
                                        {
                                            int ShowUserChoice = Print(new List<string> { "Show Worker", "Show Employer", "Exit" }, ref mainx, ref mainy);
                                            if (ShowUserChoice == 0)
                                            {
                                                database.ReadWorkers();
                                                Console.ReadKey(true);
                                            }
                                            else if (ShowUserChoice == 1)
                                            {
                                                database.ReadEmployers();
                                                Console.ReadKey(true);
                                            }
                                            else break;
                                        }
                                    }
                                    else if (AdminChoice == 2)
                                    {
                                        while (true)
                                        {
                                            int DeleteUserChoice = Print(new List<string> { "Delete Worker", "Delete Employer", "Exit" }, ref mainx, ref mainy);
                                            if (DeleteUserChoice == 0)
                                            {
                                                database.ReadWorkers();
                                                string Deleteid=FixId();
                                                try
                                                {
                                                    database.DeleteWorker(Deleteid);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                                Console.ReadKey(true);
                                            }
                                            else if (DeleteUserChoice == 1)
                                            {
                                                database.ReadEmployers();
                                                string Deleteid=FixId();
                                                try
                                                {
                                                    database.DeleteEmployer(Deleteid);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                                Console.ReadKey(true);
                                            }
                                            else break;
                                        }
                                    }
                                    else if (AdminChoice == 3)
                                    {
                                        // deadline -lari yoxlayir.
                                    }
                                    else if (AdminChoice == 4)
                                    {
                                        // request olunmus CV-ler

                                    }
                                    else if (AdminChoice == 5)
                                    {
                                        // request olunmus Vacancy-ler
                                    }
                                    else if (AdminChoice == 6)
                                    {
                                        database.CurrentAdmin!.ShowProcesses();
                                        Console.ReadKey(true);
                                    }
                                    else if (AdminChoice == 7)
                                    {
                                        Console.WriteLine(database.CurrentAdmin!);
                                    }
                                    else break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Admin not found , try again");
                                Console.ReadKey(true);
                                Console.Clear();
                                break;
                            }
                        }
                        else break;
                    }
                }
                else if (MainChoice == 1)
                {
                    while (true)
                    {

                        User user = null;
                        int UserChoice = Print(new List<string> { "Worker", "Employer", "Exit" }, ref mainx, ref mainy);
                        if (UserChoice == 0)
                        {
                            while (true)
                            {
                                user = new Worker();
                                int SignChoice = Print(new List<string> { "Login", "Register", "Exit" }, ref mainx, ref mainy);
                                if (SignChoice == 0)
                                {
                                    try
                                    {
                                        KeyValuePair<string, string> pair = FixEmailPassword();
                                        user.Email = pair.Key;
                                        user.Password = pair.Value;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        Console.ReadKey(true);
                                        Console.Clear();
                                        break;
                                    }
                                    if (database.LoginUser(user))
                                    {
                                        while (true)
                                        {
                                            int WorkerChoice = Print(new List<string> { "Notifications", "Add CV", "Delete CV", "Vacancies", "Show My CVs","Make CV Premium","Extend CV Deadline","My Account", "Exit" }, ref mainx, ref mainy);
                                            if (WorkerChoice == 0)
                                            {
                                                database.CurrentWorker!.ShowMyNotifications();
                                                Console.ReadKey(true);
                                            }
                                            else if (WorkerChoice == 1)
                                            {

                                            }
                                            else if (WorkerChoice == 2)
                                            {
                                                string deleteId = FixId();
                                                database.WorkerCvDeletion(deleteId);
                                            }
                                            else if (WorkerChoice == 3)
                                            {

                                            }
                                            else if (WorkerChoice == 4)
                                            {
                                                database.CurrentWorker!.ShowMyCVs();
                                                Console.ReadKey(true);
                                            }
                                            else if (WorkerChoice == 5)
                                            {

                                            }
                                            else if (WorkerChoice == 6)
                                            {

                                            }
                                            else if (WorkerChoice == 7)
                                            {
                                                Console.WriteLine(database.CurrentWorker);
                                            }
                                            else break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Worker not found , try again");
                                        Console.ReadKey(true);
                                        Console.Clear();
                                        break;
                                    }
                                }
                                else if (SignChoice == 1)
                                {
                                    UserRegistration(ref database, user);
                                }
                                else break;
                            }
                        }
                        else if (UserChoice == 1)
                        {
                            while (true)
                            {
                                user = new Employer();
                                int SignChoice = Print(new List<string> { "Login", "Register", "Exit" }, ref mainx, ref mainy);
                                if (SignChoice == 0)
                                {
                                    try
                                    {
                                        KeyValuePair<string, string> pair = FixEmailPassword();
                                        user.Email = pair.Key;
                                        user.Password = pair.Value;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        Console.ReadKey(true);
                                        Console.Clear();
                                        break;
                                    }
                                    if (database.LoginUser(user))
                                    {
                                        while (true)
                                        {
                                            int EmployerChoice = Print(new List<string> { "Notifications", "Add Vacancy", "Delete Vacancy", "CVs","Show My Vacancies", "My Account", "Exit" }, ref mainx, ref mainy);
                                            if (EmployerChoice == 0)
                                            {
                                                database.CurrentEmployer!.ShowMyNotifications();
                                                Console.ReadKey(true);
                                            }
                                            else if (EmployerChoice == 1)
                                            {

                                            }
                                            else if (EmployerChoice == 2)
                                            {
                                                string deleteId = FixId();
                                                database.EmployerVacancyDeletion(deleteId);
                                            }
                                            else if (EmployerChoice == 3)
                                            {

                                            }
                                            else if (EmployerChoice == 4)
                                            {
                                                database.CurrentEmployer!.ShowMyVacancies();
                                                Console.ReadKey(true);
                                            }
                                            else if (EmployerChoice == 5)
                                            {

                                            }
                                            else if (EmployerChoice == 6)
                                            {

                                            }
                                            else if (EmployerChoice == 7)
                                            {
                                                Console.WriteLine(database.CurrentEmployer);
                                            }
                                            else break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Employer not found , try again");
                                        Console.ReadKey(true);
                                        Console.Clear();
                                        break;
                                    }
                                }
                                else if (SignChoice == 1)
                                {
                                    UserRegistration(ref database, user);
                                }
                                else break;
                            }
                        }
                        else break;
                    }
                }
                else if (MainChoice == 2)
                {
                    while(true)
                    {
                        int JobSearchChoices = Print(new List<string> { "CVs", "Vacancies","Exit" },ref mainx,ref mainy);
                        if (JobSearchChoices == 0)
                        {
                            int CategoryChoice = Print(Enum.GetNames(typeof(Categories)).ToList(), ref mainx, ref mainy);
                            //database.ShowVacancies();
                            //Console.ReadKey(true);
                        }
                        else if (JobSearchChoices == 1)
                        {
                            int CategoryChoice = Print(Enum.GetNames(typeof(Categories)).ToList(), ref mainx, ref mainy);
                            //database.ShowCVs();
                            //Console.ReadKey(true);
                        }
                        else break;
                    }
                }
                else break;
            }
        }
    }
}
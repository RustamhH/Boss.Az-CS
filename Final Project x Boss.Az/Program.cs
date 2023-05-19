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
using System.Transactions;
using System.Data;
using Final_Project_x_Boss.Az.Models.CVNamespace;

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


            string MYLOGO =
                @"
                                     ______                                  
                                    (____  \                    /\           
                                     ____)  ) ___   ___  ___   /  \  _____   
                                    |  __  ( / _ \ /___)/___) / /\ \(___  )  
                                    | |__)  ) |_| |___ |___ || |__| |/ __/   
                                    |______/ \___/(___/(___(_)______(_____)  
                ";










            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(MYLOGO);
                int MainChoice = Print(new List<string> { "Admin", "User", "Job Postings", "Exit" },  mainx,  mainy);

                if (MainChoice == 0)
                {
                    while (true)
                    {
                        
                        int SignChoice = Print(new List<string> { "Login", "Exit" }, mainx,  mainy);
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
                                    int AdminChoice = Print(new List<string> { "Notifications", "Show User", "Delete User", "Check Deadlines", "Check CVs", "Check Vacancies", "Show Previous Processes", "My Account", "Exit" },  mainx,  mainy);
                                    if (AdminChoice == 0)
                                    {
                                        database.CurrentAdmin!.ShowMyNotifications();
                                        database.CurrentAdmin!.AddProcess(new($"{database.CurrentAdmin.Username} checked his/her notifications"));
                                        Console.ReadKey(true);
                                    }
                                    else if (AdminChoice == 1)
                                    {

                                        while (true)
                                        {
                                            int ShowUserChoice = Print(new List<string> { "Show Worker", "Show Employer", "Exit" },  mainx,  mainy);
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
                                            int DeleteUserChoice = Print(new List<string> { "Delete Worker", "Delete Employer", "Exit" },  mainx,  mainy);
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
                                        int deadlinechoice = Print(new List<string> { "Check Expired CVs", "Check Expired Vacancies" },mainx,mainy);
                                        if(deadlinechoice==0)
                                        {
                                            database.CurrentAdmin!.CheckCVDeadlines(ref database);
                                            Console.WriteLine("All CVs have been checked");
                                            Console.ReadKey(true);
                                        }
                                        else
                                        {
                                            database.CurrentAdmin!.CheckVacancyDeadlines(ref database);
                                            Console.WriteLine("All vacancies have been checked");
                                            Console.ReadKey(true);
                                        }
                                    }
                                    else if (AdminChoice == 4)
                                    {
                                        // request olunmus CV-ler
                                        database.DefaultAdmin.ShowRequestedCVs();
                                        string id=FixId();
                                        database.DefaultAdmin.CheckCv(ref  database, id);
                                    }
                                    else if (AdminChoice == 5)
                                    {
                                        // request olunmus Vacancy-ler
                                        database.DefaultAdmin.ShowRequestedVacancies();
                                        string id = FixId();
                                        database.DefaultAdmin.CheckVacancy(ref database, id);
                                    }
                                    else if (AdminChoice == 6)
                                    {
                                        database.CurrentAdmin!.ShowProcesses();
                                        Console.ReadKey(true);
                                    }
                                    else if (AdminChoice == 7)
                                    {
                                        Console.WriteLine(database.CurrentAdmin!);
                                        database.CurrentAdmin!.AddProcess(new($"{database.CurrentAdmin.Username} checked his account"));
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
                        int UserChoice = Print(new List<string> { "Worker", "Employer", "Exit" }, mainx,  mainy);
                        if (UserChoice == 0)
                        {
                            while (true)
                            {
                                user = new Worker();
                                int SignChoice = Print(new List<string> { "Login", "Register", "Exit" },  mainx,  mainy);
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
                                            int WorkerChoice = Print(new List<string> { "Notifications", "Add CV", "Delete CV", "Vacancies", "Show My CVs","My Account", "Exit" },  mainx,  mainy);
                                            if (WorkerChoice == 0)
                                            {
                                                database.CurrentWorker!.ShowMyNotifications();
                                                database.DefaultAdmin!.AddProcess(new($"{database.CurrentWorker.Username} checked his/her notifications"));
                                                Console.ReadKey(true);
                                            }
                                            else if (WorkerChoice == 1)
                                            {
                                                database.CurrentWorker!.CVCreation(ref database);
                                            }
                                            else if (WorkerChoice == 2)
                                            {
                                                database.CurrentWorker!.ShowMyCVs(false);
                                                database.DefaultAdmin!.AddProcess(new($"{database.CurrentWorker.Username} checked his/her cv's"));
                                                string deleteId = FixId();
                                                database.WorkerCvDeletion(deleteId);
                                            }
                                            else if (WorkerChoice == 3)
                                            {
                                                database.CurrentWorker!.Apply(ref database);
                                            }
                                            else if (WorkerChoice == 4)
                                            {
                                                database.CurrentWorker!.ShowMyCVs();
                                                database.DefaultAdmin.AddProcess(new($"{database.CurrentWorker.Username} checked his/her cv's"));
                                                Console.ReadKey(true);
                                            }
                                            else if (WorkerChoice == 5)
                                            {
                                                Console.WriteLine(database.CurrentWorker);
                                                database.DefaultAdmin.AddProcess(new($"{database.CurrentWorker!.Username} checked his/her account"));
                                                Console.ReadKey(true);
                                                Console.Clear();
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
                                int SignChoice = Print(new List<string> { "Login", "Register", "Exit" }, mainx, mainy);
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
                                            int EmployerChoice = Print(new List<string> { "Notifications", "Add Vacancy", "Delete Vacancy", "CVs","Show My Vacancies", "My Account", "Exit" },  mainx,  mainy);
                                            if (EmployerChoice == 0)
                                            {
                                                database.CurrentEmployer!.ShowMyNotifications();
                                                database.DefaultAdmin!.AddProcess(new($"{database.CurrentEmployer.Username} checked his/her notifications"));
                                                Console.ReadKey(true);
                                            }
                                            else if (EmployerChoice == 1)
                                            {
                                                database.CurrentEmployer!.VacancyCreation(ref database);
                                            }
                                            else if (EmployerChoice == 2)
                                            {
                                                database.CurrentEmployer!.ShowMyVacancies(false);
                                                database.DefaultAdmin.AddProcess(new($"{database.CurrentEmployer!.Username} checked his/her vacancies"));
                                                string deleteId = FixId();
                                                database.EmployerVacancyDeletion(deleteId);
                                            }
                                            else if (EmployerChoice == 3)
                                            {
                                                database.CurrentEmployer!.Apply(ref database);
                                            }
                                            else if (EmployerChoice == 4)
                                            {
                                                database.CurrentEmployer!.ShowMyVacancies();
                                                database.DefaultAdmin.AddProcess(new($"{database.CurrentEmployer!.Username} checked his/her vacancies"));
                                                Console.ReadKey(true);
                                            }
                                            else if (EmployerChoice == 5)
                                            {
                                                Console.WriteLine(database.CurrentEmployer);
                                                Console.ReadKey(true);
                                                Console.Clear();
                                                database.DefaultAdmin.AddProcess(new($"{database.CurrentEmployer!.Username} checked his/her account"));

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
                        int JobSearchChoices = Print(new List<string> { "CVs", "Vacancies","Exit" },mainx,mainy);
                        if (JobSearchChoices == 0)
                        {
                            CVSearchAlgorithm(ref database);                            
                        }
                        else if (JobSearchChoices == 1)
                        {
                            VacancySearchAlgorithm(ref database);
                        }
                        else break;
                    }
                }
                else break;
            }
        }
    }
}
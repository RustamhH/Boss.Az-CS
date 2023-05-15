using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az;
using Final_Project_x_Boss.Az.Models.CVNamespace;
using Final_Project_x_Boss.Az.Models.NotificationNamespace;
using Final_Project_x_Boss.Az.Models.PersonNamespace;
using Final_Project_x_Boss.Az.Models.EmployerNamespace;
using Final_Project_x_Boss.Az.Models.VacancyNamespace;
using Final_Project_x_Boss.Az.Models.WorkerNamespace;
using Final_Project_x_Boss.Az.Models.UserNamespace;
using Final_Project_x_Boss.Az.Models.Services;
using Final_Project_x_Boss.Az.Models.Other;

namespace Final_Project_x_Boss.Az.Models
{
    namespace AdminNamespace
    {
        internal sealed class Admin : Person
        {
            public List<Process> Processes { get; set; }
            public Dictionary<Guid,Vacancy> RequestedVacancies { get; set; }
            public Dictionary<Guid,CV> RequestedCV { get; set; }
            public Admin()
            {
               SetProcessesFromFile();
            }
            public Admin(string name, string surname, int age, string username,string email, string password) : base(name, surname, age, username,email, password)
            {
                SetProcessesFromFile();
            }


            
            
            public void SaveProcess()
            {
                JsonFileHandler.Write("Processes.json", Processes);
            }

            public void SetProcessesFromFile()
            {
                Processes = JsonFileHandler.Read<List<Process>>("Processes.json");
            }

            public void ShowProcesses()
            {
                foreach (var item in Processes)
                {
                    Console.WriteLine(item);
                }
            }



            public void AddProcess(Process p)
            {
                Processes.Add(p);
                SaveProcess();
            }

            public void CheckVacancyDeadlines(ref List<Employer>employers)
            {
                foreach (var employer in employers)
                {
                    foreach (var vacancy in employer.MyVacancies)
                    {
                        if(vacancy.EndTime<=DateTime.Now)
                        {
                            employer.DeleteVacancybyID(vacancy.Id.ToString());
                            Notification not=new("Expired Vacancy", $"Dear {employer.Username},your vacancy about {vacancy.Category} has been deleted due to expiring by Boss.Az admins", this);
                            employer.Notifications.Add(not);
                            AddProcess(new Process($"Admin {Username} deleted employer {employer.Username}'s  {vacancy.Id} vacancy"));
                            Functions.SendMail(employer.Email, not);
                        }
                    }
                }
            }
            public void CheckCVDeadlines(ref List<Worker>workers)
            {
                foreach (var worker in workers)
                {
                    foreach (var CV in worker.MyCVs)
                    {
                        if(CV.EndTime<=DateTime.Now)
                        {
                            worker.DeleteCVbyID(CV.Id.ToString());
                            Notification not = new("Expired CV", $"Dear {worker.Username},your CV about {CV.Category} has been deleted due to expiring by Boss.Az admins", this);
                            worker.Notifications!.Add(not);
                            AddProcess(new Process($"Admin {Username} deleted worker {worker.Username}'s  {CV.Id} vacancy"));
                            Functions.SendMail(worker.Email, not);
                        }
                    }
                }
            }

            





            public void CheckVacancy(ref Database database,string vacancyid)
            {
                foreach (var item in RequestedVacancies)
                {
                    if(item.Value.Id.ToString()==vacancyid)
                    {
                        Employer employer;
                        try
                        {
                            employer = database.FindEmployerById(item.Key.ToString());
                        }
                        catch (Exception ex) { 
                            Console.WriteLine(ex.Message);
                            Console.ReadKey(true);
                            return;
                        }
                        Notification not = default;
                        int x = 66, y = 11;
                        int choice = Functions.Print(new List<string> { "Vacancy Appropriate", "Vacancy Inappropriate" }, ref x, ref y);
                        if (choice == 0)
                        {
                            AddProcess(new Process($"Admin {Username} accepted employer {employer.Username}'s vacancy request"));
                            not = new Notification("Good Vacancy", $"Your Vacancy with [{item.Value.Id}] id accepted by Boss.Az admins", this);
                            employer.Notifications!.Add(not);
                            Functions.SendMail(employer.Email, not);
                            employer.AddVacancy(item.Value);
                            return;
                        }

                        not = new Notification("Bad Vacancy", $"Your Vacancy about {item.Value.Category} rejected by Boss.Az admins", this);
                        Functions.SendMail(employer.Email, not);
                        employer.Notifications!.Add(not);
                        AddProcess(new Process($"Admin {Username} rejected employer {employer.Username}'s vacancy request"));
                    }
                    else
                    {
                        Console.WriteLine("Vacancy Not Found");
                        Console.ReadKey(true);
                        return;
                    }
                }
            }


            public void CheckCv(ref Database database,string cvid)
            {
                foreach (var item in RequestedCV)
                {
                    if (item.Value.Id.ToString() == cvid)
                    {
                        Worker worker;
                        try
                        {
                            worker = database.FindWorkerById(item.Key.ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadKey(true);
                            return;
                        }
                        Notification not = default;
                        int x = 66, y = 11;
                        int choice = Functions.Print(new List<string> { "CV Appropriate", "CV Inappropriate" }, ref x, ref y);
                        if (choice == 0)
                        {
                            AddProcess(new Process($"Admin {Username} accepted worker {worker.Username}'s CV request"));
                            not = new Notification("Good Vacancy", $"Your Vacancy with [{item.Value.Id}] id accepted by Boss.Az admins", this);
                            worker.Notifications!.Add(not);
                            Functions.SendMail(worker.Email, not);
                            worker.AddCV(item.Value);
                            return;
                        }

                        not = new Notification("Bad CV", $"Your CV about {item.Value.Category} rejected by Boss.Az admins", this);
                        Functions.SendMail(worker.Email, not);
                        worker.Notifications!.Add(not);
                        AddProcess(new Process($"Admin {Username} rejected worker {worker.Username}'s CV request"));
                    }
                    else
                    {
                        Console.WriteLine("CV Not Found");
                        Console.ReadKey(true);
                        return;
                    }
                }
            }



            public void ShowRequestedVacancies()
            {
                foreach (var item in RequestedVacancies)
                {
                    Console.WriteLine(item.Value);
                }
            }
            public void ShowRequestedCVs()
            {
                foreach (var item in RequestedCV)
                {
                    Console.WriteLine(item.Value);
                }
            }






        }
    }

   
}

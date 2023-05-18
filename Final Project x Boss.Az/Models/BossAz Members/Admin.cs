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
using System.Runtime.CompilerServices;

namespace Final_Project_x_Boss.Az.Models
{
    namespace AdminNamespace
    {
        internal sealed class Admin : Person
        {
            public List<Process> Processes { get; set; }
            public List<Vacancy> RequestedVacancies { get; set; }
            public List<CV> RequestedCV { get; set; }
            public Admin()
            {
               RequestedCV = new();
               RequestedVacancies = new();
               SetRequestedCVsFromFile();
               SetRequestedVacanciesFromFile();
               SetProcessesFromFile();
            }
            public Admin(string name, string surname, int age, string username,string email, string password) : base(name, surname, age, username,email, password)
            {
                RequestedCV = new();
                RequestedVacancies = new();
                SetRequestedCVsFromFile();
                SetRequestedVacanciesFromFile();
                SetProcessesFromFile();
            }


            
            public void SaveRequestedVacancies()
            {
                JsonFileHandler.Write("Requested Vacancies.json", RequestedVacancies);
            }
            public void SaveRequestedCVs()
            {
                JsonFileHandler.Write("Requested CVs.json", RequestedCV);
            }

            


            public void SaveProcess()
            {
                JsonFileHandler.Write("Processes.json", Processes);
            }

            public void SetProcessesFromFile()
            {
                Processes = JsonFileHandler.Read<List<Process>>("Processes.json");
            }
            
            public void SetRequestedVacanciesFromFile()
            {
                RequestedVacancies = JsonFileHandler.Read<List<Vacancy>>("Requested Vacancies.json");
            }
            public void SetRequestedCVsFromFile()
            {
                RequestedCV = JsonFileHandler.Read<List<CV>>("Requested CVs.json");
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

            public void CheckVacancyDeadlines(ref Database database)
            {
                foreach (var employer in database.Employers)
                {
                    foreach (var vacancy in employer.MyVacancies)
                    {
                        if(vacancy.EndTime>DateTime.Now)
                        {
                            employer.DeleteVacancybyID(vacancy.Id.ToString());
                            Notification not=new("Expired Vacancy", $"Dear {employer.Username},your vacancy about {vacancy.Category} has been deleted due to expiring by Boss.Az admins", this);
                            employer.Notifications.Add(not);
                            AddProcess(new Process($"Admin {Username} deleted employer {employer.Username}'s  {vacancy.Id} vacancy "));
                            Functions.SendMail(employer.Email, not);
                        }
                    }
                }
            }
            public void CheckCVDeadlines(ref Database database)
            {
                foreach (var worker in database.Workers)
                {
                    foreach (var CV in worker.MyCVs)
                    {
                        if(CV.EndTime>DateTime.Now)
                        {
                            worker.DeleteCVbyID(CV.Id.ToString());
                            Notification not = new("Expired CV", $"Dear {worker.Username},your CV about {CV.Category} has been deleted due to expiring by Boss.Az admins", this);
                            worker.Notifications!.Add(not);
                            AddProcess(new Process($"Admin {Username} deleted worker {worker.Username}'s  {CV.Id} CV due to expiring"));
                            Functions.SendMail(worker.Email, not);
                        }
                    }
                }
            }

            





            public void CheckVacancy(ref Database database,string vacancyid)
            {
                
                foreach (var item in RequestedVacancies)
                { 
                    if (item.Id.ToString() == vacancyid)
                    {
                        Employer employer = database.FindEmployer(item.Offerer.Id.ToString());
                        if (employer != null)
                        {
                            Notification not = default;
                            int x = 66, y = 11;
                            int choice = Functions.Print(new List<string> { "Vacancy Appropriate", "Vacancy Inappropriate" }, x, y);
                            if (choice == 0)
                            {
                                AddProcess(new Process($"Admin {Username} accepted employer {employer.Username}'s vacancy request"));
                                not = new Notification("Good Vacancy", $"Your Vacancy with [{item.Id}] id accepted by Boss.Az admins", this);
                                employer.Notifications!.Add(not);
                                Functions.SendMail(employer.Email, not);
                                RequestedVacancies.Remove(item);
                                employer.AddVacancy(item);
                                database.SaveEmployers();
                                SaveRequestedVacancies();
                                return;
                            }

                            not = new Notification("Bad Vacancy", $"Your Vacancy with [{item.Id}] id  rejected by Boss.Az admins", this);
                            Functions.SendMail(employer.Email, not);
                            employer.Notifications!.Add(not);
                            RequestedVacancies.Remove(item);
                            AddProcess(new Process($"Admin {Username} rejected employer {employer.Username}'s vacancy request"));
                            SaveRequestedVacancies();
                        }
                        else
                        {
                            Console.WriteLine("Employer Not Found");
                            Console.ReadKey(true);
                            return;
                        }
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
                    if (item.Id.ToString() == cvid)
                    {
                        Worker worker = database.FindWorker(item.Owner.Id.ToString());
                        if (worker != null)
                        {
                            Notification not = default;
                            int x = 66, y = 11;
                            int choice = Functions.Print(new List<string> { "CV Appropriate", "CV Inappropriate" }, x, y);
                            if (choice == 0)
                            {
                                AddProcess(new Process($"Admin {Username} accepted worker {worker.Username}'s CV request"));
                                not = new Notification("Good Vacancy", $"Your Vacancy with [{item.Id}] id accepted by Boss.Az admins", this);
                                worker.Notifications!.Add(not);
                                Functions.SendMail(worker.Email, not);
                                worker.AddCV(item);
                                RequestedCV.Remove(item);
                                SaveRequestedCVs();
                                database.SaveWorkers();
                                return;
                            }

                            not = new Notification("Bad CV", $"Your CV with [{item.Id}] id rejected by Boss.Az admins", this);
                            Functions.SendMail(worker.Email, not);
                            worker.Notifications!.Add(not);
                            AddProcess(new Process($"Admin {Username} rejected worker {worker.Username}'s CV request"));
                            RequestedCV.Remove(item);
                            SaveRequestedCVs();
                        }
                        else
                        {
                            Console.WriteLine("Worker Not Found");
                            Console.ReadKey(true);
                            return;
                        }

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
                    Console.WriteLine(item);
                }
            }
            public void ShowRequestedCVs()
            {
                foreach (var item in RequestedCV)
                {
                    Console.WriteLine(item);                    
                }
            }



            public void AddRequestedCV(CV cv)
            {
                RequestedCV.Add(cv);
                SaveRequestedCVs();
            }
            public void AddRequestedVacancies(Vacancy vac)
            {
                RequestedVacancies.Add(vac);
                SaveRequestedVacancies();
            }


        }
    }

   
}

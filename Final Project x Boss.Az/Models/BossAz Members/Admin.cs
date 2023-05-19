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
            public Dictionary<Guid,List<Vacancy>> RequestedVacancies { get; set; }
            public Dictionary<Guid, List<CV>> RequestedCV { get; set; }
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
            
            public void SetRequestedVacanciesFromFile() {
                RequestedVacancies = JsonFileHandler.Read<Dictionary<Guid, List<Vacancy>>>("Requested Vacancies.json");
            }
            public void SetRequestedCVsFromFile()
            {
                RequestedCV = JsonFileHandler.Read<Dictionary<Guid, List<CV>>>("Requested CVs.json");
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
                            Notification not=new("Expired Vacancy", $"Dear {employer.Username},your vacancy about {vacancy.Category} has been deleted due to expiring by Boss.Az admins", Username);
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
                            Notification not = new("Expired CV", $"Dear {worker.Username},your CV about {CV.Category} has been deleted due to expiring by Boss.Az admins", Username);
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
                    foreach (var item1 in item.Value)
                    {
                        if (item1.Id.ToString() == vacancyid)
                        {
                            Employer employer = database.FindEmployer(item.Key.ToString());
                            if (employer != null)
                            {
                                Notification not = default;
                                int x = 66, y = 11;
                                int choice = Functions.Print(new List<string> { "Vacancy Appropriate", "Vacancy Inappropriate" }, x, y);
                                if (choice == 0)
                                {
                                    AddProcess(new Process($"Admin {Username} accepted employer {employer.Username}'s vacancy request"));
                                    not = new Notification("Good Vacancy", $"Your Vacancy with [{item1.Id}] id accepted by Boss.Az admins", Username);
                                    employer.Notifications!.Add(not);
                                    Functions.SendMail(employer.Email, not);
                                    RequestedVacancies.Remove(item.Key);
                                    employer.AddVacancy(item1);
                                    database.SaveEmployers();
                                    SaveRequestedVacancies();
                                    return;
                                }

                                not = new Notification("Bad Vacancy", $"Your Vacancy with [{item1.Id}] id  rejected by Boss.Az admins", Username);
                                Functions.SendMail(employer.Email, not);
                                employer.Notifications!.Add(not);
                                RequestedVacancies.Remove(item.Key);
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
                    }
                    
                }
            }
            public void CheckCv(ref Database database,string cvid)
            {
                foreach (var item in RequestedCV)
                {
                    foreach (var item1 in item.Value)
                    {
                        if (item1.Id.ToString() == cvid)
                        {
                            Worker worker = database.FindWorker(item.Key.ToString());
                            if (worker != null)
                            {
                                Notification not = default;
                                int x = 66, y = 11;
                                int choice = Functions.Print(new List<string> { "CV Appropriate", "CV Inappropriate" }, x, y);
                                if (choice == 0)
                                {
                                    AddProcess(new Process($"Admin {Username} accepted worker {worker.Username}'s CV request"));
                                    not = new Notification("Good Vacancy", $"Your Vacancy with [{item1.Id}] id accepted by Boss.Az admins", Username);
                                    worker.Notifications!.Add(not);
                                    Functions.SendMail(worker.Email, not);
                                    worker.AddCV(item1);
                                    RequestedCV.Remove(item.Key);
                                    SaveRequestedCVs();
                                    database.SaveWorkers();
                                    return;
                                }



                                not = new Notification("Bad CV", $"Your CV with [{item1.Id}] id rejected by Boss.Az admins", Username);
                                Functions.SendMail(worker.Email, not);
                                worker.Notifications!.Add(not);
                                AddProcess(new Process($"Admin {Username} rejected worker {worker.Username}'s CV request"));
                                RequestedCV.Remove(item.Key);
                                SaveRequestedCVs();
                            }
                            else
                            {
                                Console.WriteLine("Worker Not Found");
                                Console.ReadKey(true);
                                return;
                            }



                        }
                    }

                }
            }



            public void ShowRequestedVacancies()
            {
                foreach (var item in RequestedVacancies)
                {
                    foreach (var item1 in item.Value)
                    {
                        Console.WriteLine("Owner: "+item.Key+"\n"+item1);
                    }
                }
            }
            public void ShowRequestedCVs()
            {
                foreach (var item in RequestedCV)
                {
                    foreach (var item1 in item.Value)
                    {
                        Console.WriteLine("Owner: " + item.Key + "\n" + item1);
                    }
                }
            }



            public void AddRequestedCV(Guid guid,CV cv)
            {
                if (RequestedCV.ContainsKey(guid))
                {
                    RequestedCV![guid].Add(cv);
                }
                else
                {
                    RequestedCV.Add(guid, new() { cv });
                }
                SaveRequestedCVs();
                
            }
            public void AddRequestedVacancies(Guid guid,Vacancy vac)
            {
                if (RequestedVacancies.ContainsKey(guid))
                {
                    RequestedVacancies![guid].Add(vac);
                }
                else
                {
                    RequestedVacancies.Add(guid, new() { vac });
                }
                SaveRequestedVacancies();
            }


        }
    }

   
}

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
               SetRequestedCVsFromFile();
               SetRequestedVacanciesFromFile();
               SetProcessesFromFile();
            }
            public Admin(string name, string surname, int age, string username,string email, string password) : base(name, surname, age, username,email, password)
            {
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
                database.Employers.ForEach(employer => employer.MyVacancies.RemoveAll(vacancy => vacancy.EndTime <= DateTime.Now));
                database.SaveEmployers();
            }
            public void CheckCVDeadlines(ref Database database)
            {
                database.Workers.ForEach(worker => worker.MyCVs.RemoveAll(cv => cv.EndTime <= DateTime.Now));
                database.SaveWorkers();
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
                                Console.Clear();
                                int choice = Functions.Print(new List<string> { "Vacancy Appropriate", "Vacancy Inappropriate" }, x, y);
                                if (choice == 0)
                                {
                                    AddProcess(new Process($"Admin {Username} accepted employer {employer.Username}'s vacancy request"));
                                    not = new Notification("Good Vacancy", $"Your Vacancy with [{item1.Id}] id accepted by Boss.Az admins", Username);
                                    employer.AddVacancy(item1);
                                    database.SaveEmployers();
                                }
                                else
                                {
                                    not = new Notification("Bad Vacancy", $"Your Vacancy with [{item1.Id}] id  rejected by Boss.Az admins", Username);
                                    AddProcess(new Process($"Admin {Username} rejected employer {employer.Username}'s vacancy request"));
                                }
                                employer.Notifications!.Add(not);
                                Functions.SendMail(employer.Email, not);
                                RequestedVacancies[item.Key].Remove(item1);
                                if (RequestedVacancies[item.Key].Count == 0) RequestedVacancies.Remove(item.Key);
                                SaveRequestedVacancies();
                                Console.Clear();
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Employer Not Found");
                                Console.ReadKey(true);
                                Console.Clear();
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
                                Console.Clear();
                                int choice = Functions.Print(new List<string> { "CV Appropriate", "CV Inappropriate" }, x, y);
                                if (choice == 0)
                                {
                                    AddProcess(new Process($"Admin {Username} accepted worker {worker.Username}'s CV request"));
                                    not = new Notification("Good Vacancy", $"Your Vacancy with [{item1.Id}] id accepted by Boss.Az admins", Username);
                                    worker.AddCV(item1);
                                    database.SaveWorkers();
                                }
                                else
                                {
                                    not = new Notification("Bad CV", $"Your CV with [{item1.Id}] id rejected by Boss.Az admins", Username);
                                    AddProcess(new Process($"Admin {Username} rejected worker {worker.Username}'s CV request"));
                                }

                                worker.Notifications!.Add(not);
                                Functions.SendMail(worker.Email, not);
                                RequestedCV[item.Key].Remove(item1);
                                if (RequestedCV[item.Key].Count == 0) RequestedCV.Remove(item.Key);
                                SaveRequestedCVs();
                                Console.Clear();
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Worker Not Found");
                                Console.ReadKey(true);
                                Console.Clear();
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

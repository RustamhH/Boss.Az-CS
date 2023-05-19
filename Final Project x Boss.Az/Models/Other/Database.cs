using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.AdminNamespace;
using Final_Project_x_Boss.Az.Models.EmployerNamespace;
using Final_Project_x_Boss.Az.Models.WorkerNamespace;
using Final_Project_x_Boss.Az.Models.UserNamespace;
using Final_Project_x_Boss.Az.Models.NotificationNamespace;
using Final_Project_x_Boss.Az.Models.Services;
using static Final_Project_x_Boss.Az.Models.Other.Functions;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Numerics;
using Final_Project_x_Boss.Az.Models.CVNamespace;
using Final_Project_x_Boss.Az.Models.VacancyNamespace;
using System.Runtime.InteropServices;

namespace Final_Project_x_Boss.Az.Models.Other
{
    internal sealed class Database
    {
        public Admin? CurrentAdmin { get; set; }
        public Employer? CurrentEmployer { get; set; }
        public Worker? CurrentWorker { get; set; }
        public List<Employer> Employers { get; set; }
        public List<Worker> Workers { get; set; }




        public Admin DefaultAdmin { get; private set; }




        public Database()
        {
            DefaultAdmin = new("Admin", "Adminov", 25, "admin", "mya8min@gmail.com", "adminadmin123");
            SetWorkers();
            SetEmployers();
        }


        public void SetWorkers()
        {
            Workers = JsonFileHandler.Read<List<Worker>>("Workers.json");
        }
        public void SetEmployers()
        {
            Employers = JsonFileHandler.Read<List<Employer>>("Employers.json");
        }



        public void SaveWorkers()
        {
            JsonFileHandler.Write("Workers.json", Workers);
        }
        public void SaveEmployers()
        {
            JsonFileHandler.Write("Employers.json", Employers);
        }

        public void ReadWorkers()
        {
            foreach (var item in Workers)
            {
                Console.WriteLine(item);
            }
            CurrentAdmin!.AddProcess(new($"{CurrentAdmin.Username} viewed all workers"));
        }


        public void ReadEmployers()
        {
            foreach (var item in Employers)
            {
                Console.WriteLine(item);
            }
            CurrentAdmin!.AddProcess(new($"{CurrentAdmin.Username} viewed all employers"));
        }

        public void DeleteWorker(string id)
        {
            foreach (var item in Workers)
            {
                if (item.Id.ToString() == id)
                {
                    Workers.Remove(item);
                    CurrentAdmin!.AddProcess(new Process($"{CurrentAdmin.Username} deleted worker {item.Username}'s account"));
                    SendMail(item.Email, new Notification("Your Account is deleted", "Your account has been deleted by Boss.Az admins", CurrentAdmin.Username));
                    SaveWorkers();
                    Console.WriteLine("Worker Deleted");
                    return;
                }
            }
            throw new Exception("Worker Not Found");
        }
        public void DeleteEmployer(string id)
        {
            foreach (var item in Employers)
            {
                if (item.Id.ToString() == id)
                {
                    Employers.Remove(item);
                    CurrentAdmin!.AddProcess(new Process($"{CurrentAdmin.Username} deleted employer {item.Username}'s account"));
                    SendMail(item.Email, new Notification("Your Account is deleted", "Your account has been deleted by Boss.Az admins", CurrentAdmin.Username));
                    SaveEmployers();
                    Console.WriteLine("Employer Deleted");
                    return;
                }
            }
            throw new Exception("Employer Not Found");
        }




        public bool LoginAdmin(Admin admin)
        {
            if (admin.Equals(DefaultAdmin))
            {
                CurrentAdmin = DefaultAdmin;
                CurrentAdmin!.AddProcess(new Process($"{CurrentAdmin.Username} logged into his/her account"));
                SendMail(CurrentAdmin.Email, new("Login Successful", "You have logged in sucessfully", CurrentAdmin.Username));
                return true;
            }
            return false;
        }

        public bool LoginUser(User user)
        {
            foreach (var item in Workers)
            {
                if (item.Equals(user))
                {
                    CurrentWorker = item;
                    DefaultAdmin!.AddProcess(new Process($"{CurrentWorker.Username} logged into his/her account"));
                    DefaultAdmin!.Notifications!.Add(new("New Login", $"{CurrentWorker.Username} logged into his/her account", CurrentWorker.Username));
                    SendMail(CurrentWorker.Email, new("Logged in", "You have logged in to your Boss.Az account successfully", DefaultAdmin.Username));
                    return true;
                }
            }
            foreach (var item in Employers)
            {
                if (item.Equals(user))
                {
                    CurrentEmployer = item;
                    DefaultAdmin!.Processes.Add(new Process($"{CurrentEmployer.Username} logged into his/her account"));
                    DefaultAdmin!.Notifications!.Add(new("New Login", $"{CurrentEmployer.Username} logged into his/her account", CurrentEmployer.Username));
                    SendMail(CurrentEmployer.Email, new("Logged in", "You have logged in to your Boss.Az account successfully", DefaultAdmin.Username));
                    return true;
                }
            }
            return false;
        }

        public bool RegisterUser(User user)
        {
            foreach (var item in Workers)
            {
                if (item.Equals(user)) return false;
            }
            foreach (var item in Employers)
            {
                if (item.Equals(user)) return false;
            }
            if (user is Employer)
            {
                AddEmployer(user as Employer);
            }
            else if (user is Worker)
            {
                AddWorker(user as Worker);
            }
            return true;
        }




        // no filter
        public void ShowCVs(bool isLong = true)
        {
            foreach (var worker in Workers)
            {
                worker.MyCVs.Sort((c1, c2) => c1.ViewCount.CompareTo(c2.ViewCount));
                foreach (var cv in worker.MyCVs)
                {
                    if(isLong) Console.WriteLine(cv);
                    else Console.WriteLine(cv.ShortInfo());
                    Console.WriteLine();
                }
            }
        }


        // category filter
        public void ShowCVs(Categories category, bool isLong = true)
        {
            List<Worker> showList = Workers.Where(worker => worker.MyCVs.Any(cv => cv.Category == category)).ToList(); // with filter
            foreach (var worker in showList)
            {
                worker.MyCVs.Sort((c1, c2) => c1.ViewCount.CompareTo(c2.ViewCount));
                foreach (var cv in worker.MyCVs)
                {
                    if(cv.Category==category)
                    {
                        if (isLong) Console.WriteLine(cv);
                        else Console.WriteLine(cv.ShortInfo());
                        Console.WriteLine();
                    }
                }
            }
        }


        // salary filter
        public void ShowCVs(double minimumsalary, bool isLong = true) 
        {
            List<Worker> showList = Workers.Where(worker => worker.MyCVs.Any(cv => cv.WantingSalary >= minimumsalary)).ToList(); // with filter
            foreach (var worker in showList)
            {
                worker.MyCVs.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var cv in worker.MyCVs)
                {
                    if(cv.WantingSalary>=minimumsalary)
                    {
                        if (isLong) Console.WriteLine(cv);
                        else Console.WriteLine(cv.ShortInfo());
                        Console.WriteLine();
                    }
                }
            }
        }











        // premium cv
        public void ShowPremiumCVs(bool isLong = true)
        {
            List<Worker> showList = Workers.Where(worker => worker.MyCVs.Any(cv => cv.Package == Packages.Premium)).ToList(); // with filter
            foreach (var worker in showList)
            {
                worker.MyCVs.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var cv in worker.MyCVs)
                {
                    if(cv.Package==Packages.Premium)
                    {
                        if (isLong) Console.WriteLine(cv);
                        else Console.WriteLine(cv.ShortInfo());
                        Console.WriteLine();
                    }
                }
            }
        }





        // Show with no Filter
        public void ShowVacancies(bool isLong=true)
        {
            foreach (var employer in Employers)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vacancy in employer.MyVacancies)
                {
                    if (isLong) Console.WriteLine(vacancy);
                    else Console.WriteLine(vacancy.ShortInfo());
                }
            }
        }

        // Show specific Category
        public void ShowVacancies(Categories category,bool isLong=true)
        {
            List<Employer> showList = Employers.Where(employer => employer.MyVacancies.Any(vac => vac.Category == category)).ToList(); // with filter
            foreach (var employer in showList)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vac in employer.MyVacancies)
                {
                    if(vac.Category==category)
                    {
                        if (isLong) Console.WriteLine(vac);
                        else Console.WriteLine(vac.ShortInfo());
                        Console.WriteLine();
                    }
                }
            }
        }

        // Show above specific Salary
        public void ShowVacancies(double minimumSalary,bool isLong=true)
        {
            List<Employer> showList = Employers.Where(employer => employer.MyVacancies.Any(vac => vac.OfferedSalary >= minimumSalary)).ToList(); // with filter
            foreach (var employer in showList)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vac in employer.MyVacancies)
                {
                    if(vac.OfferedSalary>=minimumSalary)
                    {
                        if (isLong) Console.WriteLine(vac);
                        else Console.WriteLine(vac.ShortInfo());
                        Console.WriteLine();
                    }
                }
            }
        }


        // Show above specific Experience time
        public void ShowVacancies(int minimumexperiencetime, bool isLong = true)
        {
            List<Employer> showList = Employers.Where(employer => employer.MyVacancies.Any(vac => vac.ExperienceTime >= minimumexperiencetime)).ToList(); // with filter
            foreach (var employer in showList)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vac in employer.MyVacancies)
                {
                    if(vac.ExperienceTime>=minimumexperiencetime)
                    {
                        if (isLong) Console.WriteLine(vac);
                        else Console.WriteLine(vac.ShortInfo());
                        Console.WriteLine();
                    }
                }
            }
        }


        // Show only Premium Vacancies
        public void ShowPremiumVacancies(bool isLong = true)
        {
            List<Employer> showList = Employers.Where(employer => employer.MyVacancies.Any(vac => vac.Package == Packages.Premium)).ToList(); // with filter
            foreach (var employer in showList)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vac in employer.MyVacancies)
                {
                    if(vac.Package==Packages.Premium)
                    {
                        if (isLong) Console.WriteLine(vac);
                        else Console.WriteLine(vac.ShortInfo());
                        Console.WriteLine();
                    }
                }
            }
        }





        public void AddWorker(Worker worker)
        {
            Workers.Add(worker);
            SaveWorkers();
        }
        public void AddEmployer(Employer employer)
        {
            Employers.Add(employer);
            SaveEmployers();
        }



        public void WorkerCvDeletion(string id)
        {

            try
            {
                CurrentWorker!.DeleteCVbyID(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey(true);
                return;
            }
            DefaultAdmin!.Notifications!.Add(new("CV deletion", $"{CurrentWorker.Username} deleted his/her CV with [{id}] id", CurrentWorker.Username));
            SendMail(CurrentWorker.Email, new("CV deleted", $"Your CV with {id} has been deleted successfully", DefaultAdmin.Username));
            DefaultAdmin.AddProcess(new($"{CurrentWorker.Username} has deleted his/her CV with [{id}]"));
            SaveWorkers();
        }
        
        public void EmployerVacancyDeletion(string id)
        {
            try
            {
                CurrentEmployer!.DeleteVacancybyID(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey(true);
                return;
            }
            DefaultAdmin!.Notifications!.Add(new("Vacancy deletion", $"{CurrentEmployer.Username} deleted his/her vacancy with [{id}] id", CurrentEmployer.Username));
            SendMail(CurrentEmployer.Email, new("Vacancy deleted", $"Your vacancy with {id} has been deleted successfully", DefaultAdmin.Username));
            DefaultAdmin.AddProcess(new($"{CurrentEmployer.Username} has deleted his/her vacancy with [{id}]"));
            SaveWorkers();
        }



        public Worker FindWorker(string worker)
        {
            foreach (var item in Workers)
            {
                if (item.Id.ToString()==worker) return item;
            }
            return null;
        }public Employer FindEmployer(string employer)
        {
            foreach (var item in Employers)
            {
                if (item.Id.ToString()==employer) return item;
            }
            return null;
        }



    }

}

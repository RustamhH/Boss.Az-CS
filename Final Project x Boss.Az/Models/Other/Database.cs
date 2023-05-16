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
                    SendMail(item.Email, new Notification("Your Account is deleted", "Your account has been deleted by Boss.Az admins", CurrentAdmin));
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
                    SendMail(item.Email, new Notification("Your Account is deleted", "Your account has been deleted by Boss.Az admins", CurrentAdmin));
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
                SendMail(CurrentAdmin.Email, new("Login Successful", "You have logged in sucessfully", CurrentAdmin));
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
                    DefaultAdmin!.Notifications!.Add(new("New Login", $"{CurrentWorker.Username} logged into his/her account", CurrentWorker));
                    return true;
                }
            }
            foreach (var item in Employers)
            {
                if (item.Equals(user))
                {
                    CurrentEmployer = item;
                    DefaultAdmin!.Processes.Add(new Process($"{CurrentEmployer.Username} logged into his/her account"));
                    DefaultAdmin!.Notifications!.Add(new("New Login", $"{CurrentEmployer.Username} logged into his/her account", CurrentEmployer));
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





        public void ShowCVs()
        {
            foreach (var worker in Workers)
            {
                worker.MyCVs.Sort((c1, c2) => c1.ViewCount.CompareTo(c2.ViewCount));
                foreach (var cv in worker.MyCVs)
                {
                    Console.WriteLine(cv);
                    Console.WriteLine();
                }
            }
        }



        public void ShowCVs(Categories category)
        {
            List<Worker> showList = Workers.Where(worker => worker.MyCVs.Any(cv => cv.Category == category)).ToList(); // with filter
            foreach (var worker in showList)
            {
                worker.MyCVs.Sort((c1, c2) => c1.ViewCount.CompareTo(c2.ViewCount));
                foreach (var cv in worker.MyCVs)
                {
                    Console.WriteLine(cv);
                    Console.WriteLine();
                }
            }
        }

        // Show with no Filter
        public void ShowVacancies()
        {
            foreach (var employer in Employers)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vacancy in employer.MyVacancies)
                {
                    Console.WriteLine(vacancy);
                    Console.WriteLine();
                }
            }
        }

        // Show specific Category
        public void ShowVacancies(Categories category)
        {
            List<Employer> showList = Employers.Where(employer => employer.MyVacancies.Any(vac => vac.Category == category)).ToList(); // with filter
            foreach (var employer in showList)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vac in employer.MyVacancies)
                {
                    Console.WriteLine(vac);
                    Console.WriteLine();
                }
            }
        }

        // Show above specific Salary
        public void ShowVacancies(double minimumSalary)
        {
            List<Employer> showList = Employers.Where(employer => employer.MyVacancies.Any(vac => vac.OfferedSalary >= minimumSalary)).ToList(); // with filter
            foreach (var employer in showList)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vac in employer.MyVacancies)
                {
                    Console.WriteLine(vac);
                    Console.WriteLine();
                }
            }
        }


        // Show above specific Experience time
        public void ShowVacancies(int minimumexperiencetime)
        {
            List<Employer> showList = Employers.Where(employer => employer.MyVacancies.Any(vac => vac.ExperienceTime >= minimumexperiencetime)).ToList(); // with filter
            foreach (var employer in showList)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vac in employer.MyVacancies)
                {
                    Console.WriteLine(vac);
                    Console.WriteLine();
                }
            }
        }


        // Show only Premium Vacancies
        public void ShowPremiumVacancies()
        {
            List<Employer> showList = Employers.Where(employer => employer.MyVacancies.Any(vac => vac.Package == Packages.Premium)).ToList(); // with filter
            foreach (var employer in showList)
            {
                employer.MyVacancies.Sort((v1, v2) => v1.ViewCount.CompareTo(v2.ViewCount));
                foreach (var vac in employer.MyVacancies)
                {
                    Console.WriteLine(vac);
                    Console.WriteLine();
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
            DefaultAdmin!.Notifications!.Add(new("CV deletion", $"{CurrentWorker.Username} deleted his/her CV with [{id}] id", CurrentWorker));
            SendMail(CurrentWorker.Email, new("CV deleted", $"Your CV with {id} has been deleted successfully", DefaultAdmin));
            DefaultAdmin.AddProcess(new($"{CurrentWorker.Username} has deleted his/her CV with [{id}]"));
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
            DefaultAdmin!.Notifications!.Add(new("Vacancy deletion", $"{CurrentEmployer.Username} deleted his/her vacancy with [{id}] id", CurrentEmployer));
            SendMail(CurrentEmployer.Email, new("Vacancy deleted", $"Your vacancy with {id} has been deleted successfully", DefaultAdmin));
            DefaultAdmin.AddProcess(new($"{CurrentEmployer.Username} has deleted his/her vacancy with [{id}]"));
        }



        



    }

}

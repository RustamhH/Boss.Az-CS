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
using Final_Project_x_Boss.Az.Models.Services;
using System.Data;

namespace Final_Project_x_Boss.Az.Models
{
    internal sealed class Database
    {
        public static Admin ?DefaultAdmin { get; set; }
        public static Employer ?CurrentEmployer { get; set; }
        public static Worker ?CurrentWorker { get; set; }

        public List<Employer> Employers { get; set; }
        public List<Worker> Workers { get; set; }

        


        public Database()
        {
            DefaultAdmin = new("Rustem", "Hesenli", 23, "RustamHH","rustamh2006@gmail.com", "rustam2006");
            Employers = new List<Employer>();
            Workers = new List<Worker>();
        }


        public void SetWorkers()
        {
            Workers=JsonFileHandler.Read<List<Worker>>("Workers.json");
        }
        public void SetEmployers()
        {
            Employers=JsonFileHandler.Read<List<Employer>>("Employers.json");
        }
         
        
        // RegisterUser() 
        
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
        }
        
        public void ReadEmployers()
        {
            foreach (var item in Employers)
            {
                Console.WriteLine(item);
            }
        }

        public void DeleteWorker(string id)
        {
            foreach (var item in Workers)
            {
                if (item.Id.ToString()==id)
                {
                    Workers.Remove(item);
                    return;
                }
            }
            throw new Exception("Worker Not Found");
        }
        public void DeleteEmployer(string id)
        {
            foreach (var item in Employers)
            {
                if (item.Id.ToString()==id)
                {
                    Employers.Remove(item);
                    return;
                }
            }
            throw new Exception("Employer Not Found");
        }

        

        public void LoginUser(User user)
        {
            foreach (var item in Workers)
            {
                if (item.Equals(user))
                {
                    CurrentWorker = item;
                    DefaultAdmin!.AddProcess(new Process($"{CurrentWorker.Username} logged into his/her account"));
                    DefaultAdmin!.Notifications!.Add(new("New Login", $"{CurrentWorker.Username} logged into his/her account", CurrentWorker));
                    return;
                }
            }
            foreach (var item in Employers)
            {
                if (item.Equals(user))
                {
                    CurrentEmployer = item;
                    DefaultAdmin!.Processes.Add(new Process($"{CurrentEmployer.Username} logged into his/her account"));
                    DefaultAdmin!.Notifications!.Add(new("New Login", $"{CurrentEmployer.Username} logged into his/her account", CurrentEmployer));
                    return;
                }
            }
            throw new Exception("User Not Found");
        }

        public void RegisterUser(User user)
        {
            
        }


        public void ShowCVs(Categories category)
        {
            var filteredList = Workers.Where(worker => worker.MyCVs.Any(cv => cv.Category == category)).ToList();
            foreach (var worker in filteredList)
            {
                foreach (var cv in worker.MyCVs)
                {
                    Console.WriteLine(cv);
                    Console.WriteLine();
                }
            }
        }

        public void ShowVacancies(Categories category)
        {
            var filteredList = Employers.Where(worker => worker.MyVacancies.Any(cv => cv.Category == category)).ToList();
            foreach (var employer in filteredList)
            {
                foreach (var vacancy in employer.MyVacancies)
                {
                    Console.WriteLine(vacancy);
                    Console.WriteLine();
                }
            }
        }

    }
        
}

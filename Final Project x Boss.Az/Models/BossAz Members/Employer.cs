using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.VacancyNamespace;
using Final_Project_x_Boss.Az.Models.UserNamespace;

namespace Final_Project_x_Boss.Az.Models
{
    namespace EmployerNamespace
    {
        internal sealed class Employer : User, IApply
        {
            public List<Vacancy> MyVacancies { get; set; }



            public void Apply()
            {

            }

            public void AddVacancy(Vacancy vacancy)
            {
                foreach (var item in MyVacancies)
                {
                    if (item.Id == vacancy.Id) throw new Exception("This CV already exist");
                }
                MyVacancies.Add(vacancy);
            }
            public void DeleteVacancybyID(string id)
            {
                foreach (var item in MyVacancies)
                {
                    if (item.Id.ToString() == id)
                    {
                        MyVacancies.Remove(item);
                        return;
                    }
                }
                throw new Exception("CV not found");
            }
            public void EditVacancy(string id)
            {

            }
            public void ChangeVacancyDeadline(string id)
            {
                foreach (var item in MyVacancies)
                {
                    if (item.Id.ToString() == id)
                    {
                        Budget -= 10;
                        item.EndTime.AddMonths(1);
                    }
                }
                throw new Exception("CV not found");
            }
            public void MakeVacancyPremium(string id)
            {
                foreach (var item in MyVacancies)
                {
                    if (item.Id.ToString() == id)
                    {
                        Budget -= 50;
                        item.EndTime.AddYears(1);
                    }
                }
                throw new Exception("Vacancy not found");
            }

            public void ShowMyVacancies()
            {
                foreach (var item in MyVacancies)
                {
                    Console.WriteLine(item);
                }
            }



            public Employer() { MyVacancies = new(); }

            // param constructor

            public Employer(string name, string surname, int age, string username,string email, string password, string city, string phone, double budget) : base(name, surname, age, username,email, password, city, budget, phone)
            {
                MyVacancies = new();
            }

            public override string ToString()
            {
                string Text = base.ToString();
                Text += $"Vacancies Count:{MyVacancies.Count}\n";
                return Text;
            }
        }
    }
    
}

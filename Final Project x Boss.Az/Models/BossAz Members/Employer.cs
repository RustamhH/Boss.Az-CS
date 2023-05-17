using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.VacancyNamespace;
using Final_Project_x_Boss.Az.Models.UserNamespace;
using Final_Project_x_Boss.Az.Models.Other;
using Final_Project_x_Boss.Az.Models.WorkerNamespace;
using static Final_Project_x_Boss.Az.Models.Other.Functions;
using System.Runtime.CompilerServices;
using Final_Project_x_Boss.Az.Models.NotificationNamespace;

namespace Final_Project_x_Boss.Az.Models
{
    namespace EmployerNamespace
    {
        internal sealed class Employer : User, IApply
        {
            public List<Vacancy> MyVacancies { get; set; }



            public void Apply(ref Database database)
            {
                CVSearchAlgorithm(ref database);
                string applyid = FixId();
                foreach (var worker in database.Workers)
                {
                    foreach (var cv in worker.MyCVs)
                    {
                        if(cv.Id.ToString()==applyid)
                        {
                            // processlere elave olunsun , workerin notificationu ve maili
                            Notification notification = new("New Request", $"Dear {worker.Username} , your CV with [{applyid}] id applied by our company . See you in interview . Best regards , {Username}", this);
                            worker.Notifications!.Add(notification);
                            SendMail(worker.Email, notification);
                            database.DefaultAdmin.AddProcess(new($"{Username} applyed {worker.Username} s CV with [{applyid}] id"));
                        }
                    }
                }
            }


            public void VacancyCreation(ref Database database)
            {
                
                int x = 10, y = 2;
                //////////////////////////
                
                
                Categories category;
                Packages package;
                string profession,degree,req;
                double salary;
                ushort minage, maxage;
                int experincetime;
                //////////////////////////
                
                int categoryChoice = Print(Enum.GetNames(typeof(Categories)).ToList(),x,y);
                Enum.TryParse(categoryChoice.ToString(), out category);
                do
                {
                    Console.Write("What profession should our applyer have? ");
                    profession = Console.ReadLine();
                } while (profession == null || profession == "");
                
                do
                {
                    Console.Write("How much salary do you offer to our applyer? ");
                } while (!double.TryParse(Console.ReadLine(), out salary));

                do
                {
                    do
                    {
                        Console.Write("What age should be our applyer (minimum age) ? ");
                    } while (!ushort.TryParse(Console.ReadLine(), out minage));
                    do
                    {
                        Console.Write("What age should be our applyer (maximum age) ? ");
                    } while (!ushort.TryParse(Console.ReadLine(), out maxage));

                } while (maxage<minage);

                do
                {
                    Console.Write("What education degree should our applyer have? ");
                    degree = Console.ReadLine();
                } while (degree == null || degree == "");

                do
                {
                    Console.Write("How much experienced should our applyer be ? ");
                } while (!int.TryParse(Console.ReadLine(), out experincetime));

                do
                {
                    Console.Write("Lastly , Enter your requirements: ");
                    req = Console.ReadLine();
                } while (req == null || req == "");
                Console.Clear();
                int PackageChoice = Print(new List<string> { "Normal [1 month]", "Premium [1 year]" },x,y);
                if(PackageChoice==0)
                {
                    if (Budget < 10) return;
                    Budget -= 10;
                    package = Packages.Basic;
                }
                else
                {
                    if (Budget < 50) return;
                    Budget -= 50;
                    package = Packages.Premium;
                }
                Vacancy vacancy;
                try
                {
                    vacancy = new(this, category, profession, salary, req, minage, maxage, degree, experincetime,package);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey(true);
                    return;
                }
                // admine notification gedib yoxlanilmali,eger admin qebul etse liste elave olunur,
                // employere mail ve notification gedir , processlere elave olunur.
                database.DefaultAdmin!.Notifications!.Add(new("New Vacancy Creation", $"{Username} created a new vacancy.Check your requests to verify this vacancy", this));
                database.DefaultAdmin!.RequestedVacancies.Add(vacancy);
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
            

            public void ShowMyVacancies(bool isLong=true)
            {
                foreach (var item in MyVacancies)
                {
                    if(isLong) Console.WriteLine(item);
                    else Console.WriteLine(item.ShortInfo());
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
                Text += $"\t\tVacancies Count:{MyVacancies.Count}\n";
                return Text;
            }








            




            
        }
    }
    
}

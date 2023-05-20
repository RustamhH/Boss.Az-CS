using Final_Project_x_Boss.Az.Models.CVNamespace;
using Final_Project_x_Boss.Az.Models.Other;
using Final_Project_x_Boss.Az.Models.UserNamespace;
using static Final_Project_x_Boss.Az.Models.Other.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.VacancyNamespace;
using Final_Project_x_Boss.Az.Models.NotificationNamespace;

namespace Final_Project_x_Boss.Az.Models
{
    namespace WorkerNamespace
    {
        internal sealed class Worker : User, IApply
        {
            public List<CV> MyCVs { get; set; }


            public void Apply(ref Database database)
            {
                VacancySearchAlgorithm(ref database);
                string applyid = FixId();
                foreach (var employer in database.Employers)
                {
                    foreach (var vacancy in employer.MyVacancies)
                    {
                        if (vacancy.Id.ToString() == applyid)
                        {
                            Console.WriteLine("Which of your CV you want to apply ? Enter ID: ");
                            ShowMyCVs(false);
                            string id = FixId();
                            CV cv = CVSearchById(id);
                            if(cv!=null)
                            {
                                // processlere elave olunsun , workerin notificationu ve maili
                                database.DefaultAdmin.AddProcess(new($"{Username} applyed {employer.Username} s vacancy with [{applyid}] id"));
                                Notification notification = new("New Request", $"{cv}", Username);
                                employer.Notifications!.Add(notification);
                                SendMail(employer.Email, notification);
                            }

                        }
                    }
                }
            }

            public CV CVSearchById(string id)
            {
                foreach (var item in MyCVs)
                {
                    if (item.Id.ToString() == id) return item; 
                }
                return null;
            }

            public void AddCV(CV cv)
            {
                foreach (var item in MyCVs)
                {
                    if (item.Id == cv.Id) throw new Exception("This CV already exist");
                }
                MyCVs.Add(cv);
            }
            public void DeleteCVbyID(string id)
            {
                foreach (var item in MyCVs)
                {
                    if (item.Id.ToString() == id)
                    {
                        MyCVs.Remove(item);
                        return;
                    }
                }
                throw new Exception("CV not found");
            }
            
            public void ShowMyCVs(bool isLong=true)
            {
                foreach (var item in MyCVs)
                {
                    if(isLong) Console.WriteLine(item);
                    else Console.WriteLine(item.ShortInfo()); 
                }
            }

            public Worker() { MyCVs = new(); }


            public Worker(string name, string surname, int age, string username,string email, string password, string city, string phone, double budget) : base(name, surname, age,username, email, password, city, budget, phone)
            {
                MyCVs = new();
            }


            

            public void CVCreation(ref Database database)
            {
                int x = 10, y = 2;
                
                ///////////////////////////////////////////////////////
                
                Categories category; // +
                Packages package;   
                string profession, school, gitlink, linkedin, skillinput,companyinput,languagesinput; // +
                double unigrade,wantingsalary; // +
                bool hasdiplom; // +
                List<string> languages=new(); // +
                List<string> skills=new(), companies = new(); // +

                ///////////////////////////////////////////////////////
                int categoryChoice = Print(Enum.GetNames(typeof(Categories)).ToList(), x, y);
                Enum.TryParse(categoryChoice.ToString(), out category);

                Console.ForegroundColor = ConsoleColor.White;
                do
                {
                    Console.Write("What profession do you have? ");
                    profession = Console.ReadLine();
                } while (profession == null || profession == "");

                do
                {
                    Console.Write("How much salary do you want? ");
                } while (!double.TryParse(Console.ReadLine(), out wantingsalary));

                do
                {
                    Console.Write("What school did you graduate from? ");
                    school = Console.ReadLine();
                } while (school == null || school == "");

                do
                {
                    Console.Write("What is your university acceptance grade? ");
                } while (!double.TryParse(Console.ReadLine(), out unigrade));

                do
                {
                    Console.Write("Do you have honor diplom? ");
                } while (!bool.TryParse(Console.ReadLine(),out hasdiplom));

                do
                {
                    Console.WriteLine("Skills (separated by commas): ");
                    skillinput = Console.ReadLine();
                } while (skillinput == null || skillinput == "");
                skills = skillinput.Split(',').Select(skill => skill.Trim()).ToList();
                do
                {
                    Console.WriteLine("Companies (separated by commas): ");
                    companyinput = Console.ReadLine();
                } while (companyinput == null || companyinput == ""); 
                companies = companyinput.Split(',').Select(company => company.Trim()).ToList();

                do
                {
                    Console.WriteLine("Languages (separated by commas): ");
                    languagesinput = Console.ReadLine();
                } while (languagesinput == null || languagesinput == "");
                languages = languagesinput.Split(',').Select(language => language.Trim()).ToList();

                do
                {
                    Console.Write("Your GitLink? ");
                    gitlink = Console.ReadLine();
                } while (gitlink == null || gitlink == "");

                do
                {
                    Console.Write("Your Linkedin? ");
                    linkedin = Console.ReadLine();
                } while (linkedin == null || linkedin == "");

                Console.Clear();
                int PackageChoice = Print(new List<string> { "Normal [1 month]", "Premium [1 year]" }, x, y);
                if (PackageChoice == 0)
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



                CV cv;
                try
                {
                    cv = new(category, school, unigrade, skills, companies, languages, hasdiplom, profession, gitlink, linkedin, wantingsalary, package);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey(true);
                    return;
                }
                database.DefaultAdmin!.Notifications!.Add(new("New CV Creation", $"{Username} created a new CV.Check your requests to verify this CV", Username));
                database.DefaultAdmin!.AddRequestedCV(Id,cv);
            }



            public override string ToString()
            {
                string Text = base.ToString();
                Text += $"CV count: {MyCVs.Count}\n";
                return Text;
            }










        }
    }
    
}

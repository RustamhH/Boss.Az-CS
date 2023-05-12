using Final_Project_x_Boss.Az.Models.EmployerNamespace;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.EmployerNamespace;


namespace Final_Project_x_Boss.Az.Models
{
    namespace VacancyNamespace
    {
        internal sealed class Vacancy
        {
            private int _viewcount;
            private int _experiencetime;
            private double _offeredsalary;

            public Employer Offerer { get; set; }
            public Categories Category { get; set; }
            public Guid Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Profession { get; set; }

            public List<string> Requirements { get; set; }
            public string Location { get; set; }
            public ushort MinimumAge { get; set; }
            public ushort MaximumAge { get; set; }
            public string Degree { get; set; }



            public int ViewCount
            {
                get => _viewcount;
                set
                {
                    if (value < 0) throw new Exception("View count can't be negative");
                    _viewcount = value;
                }
            }

            public int ExperienceTime
            {
                get => _experiencetime;
                set
                {
                    if (value < 0) throw new Exception("Experience time can't be negative");
                    _experiencetime = value;
                }
            }
            public double OfferedSalary
            {
                get => _offeredsalary;
                set
                {
                    if (value < 0) throw new Exception("Offered salary can't be negative");
                    _offeredsalary = value;
                }
            }







            public override string ToString()
            {
                string MainText =
                    $@"
                Category:{Category}                     View Count:{ViewCount} | Vacancy Id:{Id}
                    Profession:{Profession}             Phone:{Offerer.Phone}
                    Offered Salary:{OfferedSalary}                 E-Mail:{Offerer.Email}
                    Offerer:{Offerer.Name + " " + Offerer.Surname}
                    City:{Offerer.City}
                    Degree:{Degree}
                    Experience:{ExperienceTime} year
                    Start Time:{StartTime.Date.ToShortDateString()}
                    End Time:{EndTime.Date.ToShortDateString()}
                    
                ";
                MainText += "    Requirements\n";
                foreach (var item in Requirements)
                {
                    MainText += "\t\t\t" + item + "\n";
                }
                return MainText;
            }






            public string ShortInfo()
            {
                return $@"
            {Profession}
            {OfferedSalary} AZN
            {Offerer.Name + " " + Offerer.Surname}
            ";
            }
            public Vacancy() { Id = Guid.NewGuid(); }

            public Vacancy(Employer offerer, Categories category, DateTime endTime, int viewCount,
            string profession, double offeredSalary, List<string> requirements, string location, ushort minimumAge,
            ushort maximumAge, string degree, int experienceTime) : this()
            {
                Offerer = offerer;
                Category = category;
                StartTime = DateTime.Now;
                EndTime = endTime;
                ViewCount = viewCount;
                Profession = profession;
                OfferedSalary = offeredSalary;
                Requirements = requirements;
                Location = location;
                MinimumAge = minimumAge;
                MaximumAge = maximumAge;
                Degree = degree;
                ExperienceTime = experienceTime;
            }


        }
    }
    
}

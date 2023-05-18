using Final_Project_x_Boss.Az.Models.EmployerNamespace;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.Other;
using System.Text.Json.Serialization;

namespace Final_Project_x_Boss.Az.Models
{
    namespace VacancyNamespace
    {
        internal sealed class Vacancy
        {
            private int _viewcount;
            private int _experiencetime;
            private double _offeredsalary;
            Packages _package;

            [JsonIgnore]
            public Employer Offerer { get; set; }
            public Categories Category { get; set; }
            public Guid Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Profession { get; set; }

            public string Requirements { get; set; }
            public ushort MinimumAge { get; set; }
            public ushort MaximumAge { get; set; }
            public string Degree { get; set; }



            public Packages Package { get=>_package;
                set
                {
                    if(value==Packages.Basic) { EndTime = DateTime.Now.AddMonths(1); }
                    else { EndTime = DateTime.Now.AddYears(1); }
                    _package = value;
                } 
            }
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
                Category:{Category}                     View Count:{ViewCount} | Vacancy Id:{Id} | {Package} package vacancy
                    Profession:{Profession}             Phone:{Offerer.Phone}
                    Offered Salary:{OfferedSalary}                 E-Mail:{Offerer.Email}
                    Offerer:{Offerer.Name + " " + Offerer.Surname}
                    City:{Offerer.City}
                    Degree:{Degree}
                    Experience:{ExperienceTime} year
                    Start Time:{StartTime.Date.ToShortDateString()}
                    End Time:{EndTime.Date.ToShortDateString()}
                    
                            Requirements
                    {Requirements}
                    ";
                
                return MainText;
            }






            public string ShortInfo()
            {
                return $@"
            {Id}
            {Profession}
            {OfferedSalary} AZN
            {Offerer.Name + " " + Offerer.Surname}
            ";
            }
            public Vacancy() { Id = Guid.NewGuid(); }

            public Vacancy(Employer offerer, Categories category
            ,string profession, double offeredSalary, string requirements,  ushort minimumAge,
            ushort maximumAge, string degree, int experienceTime, Packages package) : this()
            {
                Offerer = offerer;
                Category = category;
                StartTime = DateTime.Now;
                ViewCount = 0;
                Profession = profession;
                OfferedSalary = offeredSalary;
                Requirements = requirements;
                MinimumAge = minimumAge;
                MaximumAge = maximumAge;
                Degree = degree;
                ExperienceTime = experienceTime;
                Package = package;
            }
        }
    }
    
}

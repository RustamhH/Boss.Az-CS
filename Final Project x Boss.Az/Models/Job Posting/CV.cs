using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.Other;
using Final_Project_x_Boss.Az.Models.WorkerNamespace;

namespace Final_Project_x_Boss.Az.Models
{
    namespace CVNamespace
    {
        internal sealed class CV
        {
            private int _viewcount;
            private double _unigrade;
            private double _wantingsalary;
            private DateTime _endTime;
            private string _gitlink;
            private string _linkedin;


            public Worker Owner { get; set; }
            public Guid Id { get; init; }
            public Categories Category { get; init; }
            public DateTime StartTime { get; init; }

            public string Profession { get; set; }
            public string School { get; set; }
            public List<string> Skills { get; set; }
            public List<string> Companies { get; set; }
            public Dictionary<string, string> Languages { get; set; }
            public bool HasDiplom { get; set; }
            public string LinkedIn { get=>_linkedin; set 
                {
                    if (!value.EndsWith("linkedin.com")) throw new Exception("Invalid Linkedin");
                    _linkedin = value;
                }
            }
            
            
            public string GitLink { get => _gitlink;
                set
                {
                    if (!value.EndsWith("github.com")) throw new Exception("Invalid Git Link");
                    _gitlink = value;
                }
            }
            public double WantingSalary
            {
                get => _wantingsalary;
                set
                {
                    if (value < 0) throw new Exception("Salary can't be negative");
                    _wantingsalary = value;
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

            public double UniversityAcceptanceGrade
            {
                get => _unigrade;
                init
                {
                    if (value < 0) throw new Exception("Uni Grade can't be negative");
                    _unigrade = value;
                }
            }
            public DateTime EndTime
            {
                get => _endTime;
                set
                {
                    if (value < DateTime.Now) throw new Exception("This CV is expired");
                    _endTime = value;
                }
            }




            public CV() { Id = Guid.NewGuid(); }

            public CV(Worker owner, Categories category, string school, double universityAcceptanceGrade,
                  List<string> skills, List<string> companies,
                  DateTime endDate, Dictionary<string, string> languages,
                  bool hasDiploma, int viewcount, string prof, string gitLink, string linkedIn, double wantingSalary) : this()
            {
                Owner = owner;
                Category = category;
                School = school;
                UniversityAcceptanceGrade = universityAcceptanceGrade;
                Skills = skills;
                Companies = companies;
                StartTime = DateTime.Now;
                EndTime = endDate;
                Languages = languages;
                HasDiplom = hasDiploma;
                GitLink = gitLink;
                LinkedIn = linkedIn;
                WantingSalary = wantingSalary;
                ViewCount = viewcount;
                Profession = prof;
            }





            public override string ToString()
            {
                string MainText =
                $@"
        Category: {Category}                                View Count: {ViewCount} | CV Id:{Id}                      
            {Profession}
            {Owner.Name + " " + Owner.Surname} : {WantingSalary} AZN
            
            City: {Owner.City}                     Phone : {Owner.Phone}
            Age:  {Owner.Age}                       E-Mail: {Owner.Email}
            Start Time: {StartTime.Date.ToShortDateString()}         Linkedin: {LinkedIn}
            End Time: {EndTime.Date.ToShortDateString()}           GitHub: {GitLink}
           
            Education:                               
                Profession: {Profession}
                School:{School}
                Uni Acceptance Grade:{UniversityAcceptanceGrade}
                Has Diplom: {HasDiplom}
            ";
                MainText += "\n\t    Skills:\n\n";
                foreach (var item in Skills)
                {
                    MainText += "\t\t\t" + item + "\n";
                }
                MainText += "\n\t    Companies:\n\n";
                foreach (var item in Companies)
                {
                    MainText += "\t\t\t" + item + "\n";
                }
                MainText += "\n\t    Languages:\n\n";
                foreach (var item in Languages)
                {
                    MainText += "\t\t\t" + item.Key + "-" + item.Value + "\n";
                }
                return MainText;
            }



            public string ShortInfo()
            {
                return $@"
            {Profession}
            {WantingSalary} AZN
            {Owner.Name + " " + Owner.Surname}
            ";
            }




        }
    }
    

}

using Final_Project_x_Boss.Az.Models.CVNamespace;
using Final_Project_x_Boss.Az.Models.UserNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_x_Boss.Az.Models
{
    namespace WorkerNamespace
    {
        internal sealed class Worker : User, IApply
        {
            public List<CV> MyCVs { get; set; }


            public void Apply()
            {

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
            public void EditCV(string id)
            {

            }
            public void ChangeCVdeadline(string id)
            {
                foreach (var item in MyCVs)
                {
                    if (item.Id.ToString() == id)
                    {
                        Budget -= 10;
                        item.EndTime.AddMonths(1);
                    }
                }
                throw new Exception("CV not found");
            }
            public void MakeCVPremium(string id)
            {
                foreach (var item in MyCVs)
                {
                    if(item.Id.ToString()==id)
                    {
                        Budget -= 50;
                        item.EndTime.AddYears(1);
                    }
                }
                throw new Exception("CV not found");
            }

            public void ShowMyCVs()
            {
                foreach (var item in MyCVs)
                {
                    Console.WriteLine(item);
                }
            }

            public Worker() { MyCVs = new(); }


            public Worker(string name, string surname, int age, string username,string email, string password, string city, string phone, double budget) : base(name, surname, age,username, email, password, city, budget, phone)
            {
                MyCVs = new();
            }


            public override string ToString()
            {
                string Text = base.ToString();
                Text += $"\t\tCV count: {MyCVs.Count}\n";
                return Text;
            }
        }
    }
    
}

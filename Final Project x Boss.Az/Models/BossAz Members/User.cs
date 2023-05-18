using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.PersonNamespace;
namespace Final_Project_x_Boss.Az.Models
{
    namespace UserNamespace
    {
        internal class User : Person
        {
            private double _budget;
            private string? _phone;

            public double Budget
            {
                get => _budget;
                set
                {
                    if (value < 0) throw new Exception("Budget can't be negative");
                    _budget = value;
                }
            }
            public string? Phone
            {
                get { return _phone!; }
                set
                {

                    if (!value!.StartsWith("070") && !value.StartsWith("077") && !value.StartsWith("051") && !value.StartsWith("050") && !value.StartsWith("055")) throw new Exception("Invalid Phone Start");

                    value = value.Replace("-", "");
                    if (!value.All(char.IsDigit)) throw new Exception("Phone must e only numbers");
                    if (value.Length != 10) throw new Exception("Invalid Phone Number Lenght");
                    _phone = value;
                }
            }
            public string? City { get; set; }

            public User() { }
            public User(string name, string surname, int age, string username,string email, string password, string? city, double budget, string? phone) : base(name, surname, age,username, email, password)
            {
                City = city;
                Budget = budget;
                Phone = phone;
            }




            public override string ToString()
            {
                string Text = base.ToString();
                Text += $"City: {City}\nPhone: {Phone}\nBudget: {Budget}\n";
                return Text;
            }

        }


    }
}

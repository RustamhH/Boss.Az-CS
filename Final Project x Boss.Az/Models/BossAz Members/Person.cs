using Final_Project_x_Boss.Az.Models.NotificationNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_x_Boss.Az.Models
{
    namespace PersonNamespace
    {
        internal abstract class Person : IEquatable<Person>
        {
            private int _age;
            private string _password;
            private string _email;
            private string _username;

            public Guid Id { get; init; }
            public string? Name { get; set; }
            public string? Surname { get; set; }

            public List<Notification>? Notifications { get; set; }

            public int Age
            {
                get { return _age; }
                set
                {
                    if (value < 18) throw new Exception("Age can't be lower than 18");
                    _age = value;
                }
            }

            public string Email
            {
                get { return _email; }
                set
                {
                    if (!value.EndsWith(".com") || !value.Contains('@')|| value.Length <= 10) throw new Exception("Invalid Email");
                    _email = value;
                }
            }



            public string Password
            {
                get { return _password; }
                set
                {
                    if (value.Length < 8) throw new Exception("Invalid Password");
                    _password = value;
                }
            }


            public string Username
            {
                get { return _username; }
                set 
                {
                    if (value.Length < 3) throw new Exception("Invalid Username"); 
                    _username= value;
                }
            }



            public Person() { 
                Id = Guid.NewGuid();
                Notifications = new();
            }
            public Person(string name, string surname, int age, string username,string email, string password) : this()
            {
                Name = name;
                Surname = surname;
                Age = age;
                Email = email;
                Username = username;
                Password = password;
            }

            public override string ToString()
            {
                return
                $@"
Id:{Id}
Name:{Name}
Surname {Surname}
Age:{Age}
Username:{Username}
Email:{Email}
Password:{Password}
";
            }



            public bool Equals(Person? other)
            {
                return other.Email == Email && other.Password == Password;
            }


            public void ShowMyNotifications()
            {
                if (Notifications != null)
                {
                    foreach (var item in Notifications!)
                    {
                        Console.WriteLine(item.LongData());
                    }
                }
            }




        }

    }
}

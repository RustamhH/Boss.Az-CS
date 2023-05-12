using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_x_Boss.Az.Models.Services
{
    internal sealed record Process
    {
        public Guid Id { get; init; }
        public string Content { get; set; }
        public DateTime Time { get; init; }

        public Process() { Id = Guid.NewGuid(); }
        public Process(string content) : this()
        {
            Content = content;
            Time = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Id:{Id}\nContent:\n{Content}\nTime:{Time}\n";
        }

    }
}

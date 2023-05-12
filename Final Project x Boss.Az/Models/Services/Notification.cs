using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final_Project_x_Boss.Az.Models.PersonNamespace;

namespace Final_Project_x_Boss.Az.Models
{
    namespace NotificationNamespace
    {
        internal sealed class Notification
        {
            public Guid Id { get; init; }
            public string? Title { get; init; }
            public string? Content { get; init; }
            public Person? FromWho { get; init; }
            public DateTime SendingTime { get; init; }

            public Notification()
            {
                Id = Guid.NewGuid();
            }

            public Notification(string? title, string? content, Person? fromWho) : this()
            {
                Title = title;
                Content = content;
                FromWho = fromWho;
                SendingTime = DateTime.Now;
            }

            public string ShortData()
            {
                return $"Id:{Id}\nFrom:{FromWho!.Username}";
            }
            public string LongData()
            {
                return $"Id:{Id}\nFrom:{FromWho!.Username}\nSending Time:{SendingTime}\nTitle:{Title}\nContent:\n{Content}";
            }

        }
    }
    
}

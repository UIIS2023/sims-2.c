using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourist_Project.Domain.Models
{
    public class Message
    {
        public bool Type { get; set; }
        public string Content { get; set; }

        public Message()
        {
            Content = string.Empty;   
        }

        public Message(bool type, string content)
        {
            Type = type;
            Content = content;
        }
    }
}

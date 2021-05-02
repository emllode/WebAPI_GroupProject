using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_WebAPI_Template.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public bool IsDone { get; set; }
    }
}
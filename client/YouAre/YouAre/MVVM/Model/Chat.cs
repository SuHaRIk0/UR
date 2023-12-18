using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouAre.MVVM.Model
{
    public class Chat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<int>? Messages { get; set; }
        public List<int>? Members { get; set; }
    }
}

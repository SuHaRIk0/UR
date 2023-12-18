using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouAre.MVVM.Model
{
    public class Account
    {
        public string? Token { get; set; }
        public string? RecoveryToken { get; set; }
        public int UserId { get; set; }

        static public Account Empty = new Account() { Token = " ", RecoveryToken = " " };
    }
}

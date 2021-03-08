using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestServer.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
    }
}

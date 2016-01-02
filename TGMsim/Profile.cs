using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class Profile
    {
        public String name;
        public int decoration = 0;
        public int globalDecoration;
        public bool displayGlobal = false;
        public List<byte> password;
        public bool passProtected = false;
    }
}

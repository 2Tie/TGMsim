using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class G_SEGA : Generator
    {
        public G_SEGA(int nuseed) : base(nuseed)
        {

        }

        public override int pull()
        {
            return read() % 7;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class FrameTimer
    {
        int cycle = 1;
        public long count = 0;

        public FrameTimer()
        {

        }

        public void tick()
        {
            count += 16 + (cycle%1);
            cycle++;
            if (cycle == 4) cycle = 1;
        }
    }
}

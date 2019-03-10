using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class M_Miner : Mode
    {
        public M_Miner(int v)
        {
            ModeName = "MINER";
            border = Color.DarkGreen;
            gradedBy = 4;
            limitType = 4;
            limit = 1;
            sections.Add(999);
            autoGarbage = true;
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 16 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 41 });

            if(v == 1)
            {
                ModeName = "ZEN";
                startEnd = true;
                keepFieldSafe = true;
            }
        }
    }
}

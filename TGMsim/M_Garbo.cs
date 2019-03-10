using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class M_Garbo : Mode
    {

        public M_Garbo()
        {
            ModeName = "GARBAGE CLEAR";
            border = Color.DarkGreen;
            gradedBy = 4;
            limitType = 4;
            limit = 1;
            sections.Add(999);
            startWithRandField = true;
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 16 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 41 });
        }

    }
}

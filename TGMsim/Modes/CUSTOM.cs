using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class Custom : Mode
    {
        public Custom()
        {
            delayTable.Add(new List<int> { 27 });
            delayTable.Add(new List<int> { 27 });
            delayTable.Add(new List<int> { 14 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 40 });
            sections.Add(999);
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;
        }
    }
}

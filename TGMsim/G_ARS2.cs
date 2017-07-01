using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class G_ARS2 : Generator
    {
        public G_ARS2(int nuseed) : base(nuseed)
        {
            rolls = 6;
            history = new List<int> { 1, 1, 2, 2 };
            int temp = 1;
            while (temp == 1 || temp == 2 || temp == 5)
            {
                temp = read() % 7;
            }
            history[0] = temp;
        }

        public override int rand(int s)
        {
            return (int)(((s * 0x41C64E6D) + 12345) & 0xFFFFFFFF);
        }

        public override int pull()
        {
            int temp = 0;
            for (int i = 0; i < rolls; i++)
            {
                temp = read() % 7;
                if (!history.Contains(temp))
                    break;
            }
            updateHistory(temp);
            return history[1];
        }
    }
}

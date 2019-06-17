using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class G_ARS3 : Generator
    {
        List<int> drought_order = new List<int> { 3, 0, 1, 4, 5, 6, 2 };
        List<int> bag = new List<int> { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6 };

        public G_ARS3(int nuseed) : base(nuseed)
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

        public override int pull()
        {
            int temp = 0;
            for (int i = 0; i < rolls; i++)
            {
                temp = read() % 35;
                if (!history.Contains(bag[temp]))
                    break;
            }
            int piece = bag[temp];
            bag[temp] = drought_order[0];

            drought_order.RemoveAt(0);
            drought_order.Add(piece);

            updateHistory(piece);
            
            return history[1];
        }
    }
}

using System.Collections.Generic;

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

        public override int pull()
        {
            int stor = history[0];
            int temp = 0;
            for (int i = 0; i < rolls; i++)
            {
                temp = read() % 7;
                if (!history.Contains(temp))
                    break;
            }
            updateHistory(temp);
            return stor;
        }
    }
}

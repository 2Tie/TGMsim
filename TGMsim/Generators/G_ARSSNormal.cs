using System.Collections.Generic;

namespace TGMsim
{
    class G_ARSSNormal : Generator
    {
        public G_ARSSNormal(int nuseed) : base(nuseed)
        {
            rolls = 4;
            history = new List<int> { -1, -1, -1, -1, -1, -1 };
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
                temp = read() % 7;
                if (!history.Contains(temp))
                    break;
                if(i+1 == rolls)
                {
                    temp = history[read()&4];
                    if (temp == -1)
                        temp = read() % 7;
                }
            }
            updateHistory(temp);
            return history[1];
        }
    }
}

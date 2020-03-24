using System.Collections.Generic;

namespace TGMsim
{
    class G_ARS3 : Generator
    {
        List<int> droughts = new List<int> { 4, 4, 4, 4, 4, 4, 4 };
        List<int> bag = new List<int> { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6 };

        int max = 0;
        int rep = 0;

        public G_ARS3(int nuseed) : base(nuseed)
        {
            rolls = 6;
            history = new List<int> { 1, 1, 2, 2 };
        }

        public override int firstpull()
        {
            int temp = 1;
            while (temp == 1 || temp == 2 || temp == 5)
            {
                temp = read() % 7;
            }
            history[0] = temp;
            return temp;
        }

        public override int pull()
        {
            int temp = 0;
            max = 0;
            rep = 0;
            for (int i = 0; i < rolls; i++)
            {
                temp = read() % 35;
                if (!history.Contains(bag[temp]))
                    break;
                replace_bag(temp);
                temp = read() % 35;//duplicate, needed
            }
            int piece = bag[temp];

            for (int i = 0; i < droughts.Count; i++)
                droughts[i]++;
            droughts[piece] = 0;
            
            replace_bag(temp);

            updateHistory(piece);
            
            return piece;
        }

        private void replace_bag(int place)
        {
            //calc max drought
            for (int i = 0; i < droughts.Count; i++)
            {
                if (droughts[i] > max)
                {
                    max = droughts[i];
                    rep = i;
                }
            }
            bag[place] = rep;
        }
    }
}

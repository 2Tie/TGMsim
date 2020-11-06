using System.Collections.Generic;

namespace TGMsim
{
    class G_ARS3Easy : Generator
    {
        List<int> bag = new List<int> { 0, 3, 4, 5, 6 };
        bool norepeat = true;

        public G_ARS3Easy(int nuseed) : base(nuseed)
        {
            history = new List<int> { 0, 0 };
            int temp = read() % 5;
            history[0] = bag[temp]; //save rolled piece
            bag.RemoveAt(temp); //swipe from bag
        }

        public override int pull()
        {
            int temp = read() % bag.Count;
            while(norepeat && history[0] == bag[temp])
            {
                temp = read() % bag.Count;
            }
            int piece = bag[temp];
            bag.RemoveAt(temp);
            if (bag.Count == 0)
                bag = new List<int> { 0, 3, 4, 5, 6 };
            updateHistory(piece);
            
            return history[1];
        }

        public override void handleFlag()
        {
            norepeat = false;
        }
    }
}

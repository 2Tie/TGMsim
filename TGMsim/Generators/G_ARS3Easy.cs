using System.Collections.Generic;

namespace TGMsim
{
    class G_ARS3Easy : Generator
    {
        //strict 3 history, possibly true random after that? a roll system possibly? hard to know for sure right now
        List<int> pieces = new List<int> { 0, 3, 4, 5, 6 };
        bool norepeat = true;

        public G_ARS3Easy(int nuseed) : base(nuseed)
        {
            history = new List<int> { 7, 7, 7 };
            int temp = read() % 5;
            history[0] = pieces[temp]; //save rolled piece
        }

        public override int pull()
        {
            pieces = new List<int> { 0, 3, 4, 5, 6 }; //refill options
            if (norepeat)//remove options in history
                for (int i = 0; i < history.Count; i++)
                    pieces.Remove(history[i]);

            int temp = read() % pieces.Count;
            int piece = pieces[temp];
            updateHistory(piece);
            
            return history[1];
        }

        public override void handleFlag()
        {
            norepeat = false;
        }
    }
}

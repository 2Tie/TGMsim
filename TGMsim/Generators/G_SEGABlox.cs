using System.Diagnostics;

namespace TGMsim.Generators
{
    class G_SEGABlox : G_SEGA
    {
        int[] IDs;

        public G_SEGABlox(int nuseed) : base(nuseed)
        {
            IDs = new int[] { 0, 1, 2, 5, 6, 4, 3 }; //the internal piece order
        }

        public override int read()
        {
            return rand(seed) & 0x7F;
        }

        public override int pull()
        {
            //take the next position
            int p = read();
            int id = p % 7;
            if ((p & 0x70) == 0x70)
                if (id == 1 || id == 2) //mod table madness!
                    id = 0;
            return IDs[id];
        }

        public override int firstpull()
        {
            //generate the sequence
            //seed = 0x2A6D365B; //shared with flash point
            seed = rand(seed); //advances the lower bytepair but keeps the original high bytepair
            //Debug.WriteLine(seed.ToString("X")); //should be 0x2A6D8010 for POP
            //pull a piece
            return pull();
        }

        public override void reset()
        {
            //intentionally blank
        }
    }
}

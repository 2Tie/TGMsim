namespace TGMsim
{
    class G_SEGA : Generator
    {
        int[] IDs;
        byte[] seq;
        int pos = 0;

        public G_SEGA(int nuseed) : base(nuseed)
        {
            IDs = new int[] { 0, 1, 2, 5, 6, 4, 3}; //the internal piece order
            seq = new byte[1000];
        }

        public override int rand(int s)
        {
            int old = s;
            s *= 41;
            seed = s + (s << 16);
            return ((s & 0xFFFF) + (s>>16)) | (int)(old & 0xFFFF0000);
        }

        private new int read()
        {
            return rand(seed) & 0x3F;
        }

        public override int pull()
        {
            //take the next position
            int p = IDs[seq[pos]];
            pos++;
            if (pos == 1000)
                pos = 0;
            return p;
        }

        public override int firstpull()
        {
            //generate the sequence
            //seed = 0x2A6D365A;//the power-on-pattern, for testing//this is 0x2A6D365B for bloxeed
            for (int i = 0; i < 1000; i++)
            {
                seq[i] = (byte)(read() % 7);
            }
            //pull a piece
            return pull();
        }

        public override void reset()
        {
            pos = 0;
        }
    }
}

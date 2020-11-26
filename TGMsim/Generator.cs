using System.Collections.Generic;

namespace TGMsim
{
    class Generator
    {
        public string type = "BASE";
        public int startingseed;
        public int seed;
        public int rolls;
        public List<int> history = new List<int>(4);

        public Generator(int nuseed)
        {
            seed = nuseed;
            startingseed = seed;

        }

        public void updateHistory(int id)
        {
            history.RemoveAt(history.Count - 1);
            history.Insert(0, id);
        }

        public int read() //calls the rand and saves seed
        {
            seed = rand(seed);
            return (seed >> 10) & 0x7FFF;
        }

        public virtual int rand(int s) //takes a number and returns a new one
        {
            return (int)(((s * 0x41C64E6D) + 12345) & 0xFFFFFFFF);
        }

        public virtual int pull() //grabs next tetromino ID
        {
            return 6;
        }

        public virtual int firstpull()
        {
            return pull();
        }

        public virtual void handleFlag()
        {

        }

        public virtual void reset()
        {
            seed = startingseed;
        }
    }
}

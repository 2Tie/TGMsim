using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class Generator
    {
        public string type;
        public int seed;
        public int rolls;
        public List<int> history = new List<int>(4);

        public Generator(int nuseed)
        {
            seed = nuseed;
        }

        public void updateHistory(int id)
        {
            history.RemoveAt(history.Count - 1);
            history.Insert(0, id);
        }

        public int read()
        {
            seed = rand(seed);
            return (seed >> 10) & 0x7FFF;
        }

        public virtual int rand(int s)
        {
            return (int)(((s * 0x41C64E6D) + 12345) & 0xFFFFFFFF);
        }

        public virtual int pull()
        {
            return 6;
        }
    }
}

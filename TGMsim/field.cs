using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class Field
    {

        public List<Tetromino> nextTet = new List<Tetromino>();
        public List<int> lastTet = new List<int>() { 0, 0, 0, 0};

        public List<List<int>> gameField = new List<List<int>>();

        public List<int> full = new List<int>();

        public Tetromino heldPiece;

        public bool swappedHeld;

        public int x, y, width, height;
        public enum timerType { ARE, DAS, LockDelay, LineClear} ;
        public int currentTimer = 0;
        public int timerCount = 0;
        public int groundTimer = 0;
        public int gravCounter = 0;
        public int gravLevel = 0;
        public int level = 1;
        public int grade = 0;
        public int score = 0;
        public int combo = 1;

        public int softCounter = 0;
        public Field()
        {
            x = 250;
            y = 100;
            width = 250;
            height = 500;
            for (int i = 0; i < 10; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 20; j++)
                {
                    tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                gameField.Add(tempList);
            }
        }

        public void randomize()
        {
            Random rng = new Random();
            for (int i = 0; i < 50; i++)
            {
                gameField[rng.Next(10)][rng.Next(20)] = rng.Next(8);
                
            }
        }
    }
}

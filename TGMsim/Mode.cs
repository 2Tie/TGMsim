using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class Mode
    {
        public List<int> gradePointsTGM1 = new List<int> { 0, 400, 800, 1400, 2000, 3500, 5500, 8000, 12000, 16000, 22000, 30000, 40000, 52000, 66000, 82000, 100000, 120000 };
        public struct Gimmick
        {
            public int type; //fading, vanishing, copygarbage, bones, ice, big, random garbage, preset garbage
            public int startLvl;
            public int endLvl;
            public int parameter;
        }

        public enum GarbType { FIXED, COPY, RANDOM } ;

        public string ModeName = "DUMMY";
        public int id = 0;
        public int endLevel = 999;
        public List<int> sections = new List<int>();
        public int curSection;
        public bool bigmode = false;
        public bool g20 = false;
        public GarbType garbType; //if garbage follows a fixed pattern or not
        public int garbTimer = 0;
        public List<List<int>> garbTemplate = new List<List<int>>();
        public bool raiseGarbOnClear = false;
        public int garbSafeLine = 0;
        public int garbLine = 0;
        public bool garbMeter = false;
        public int garbDelay = 0;
        public int lvlBonus = 0;
        public int level = 0;
        public int gradedBy = 0; //points, grade points, level, bravo, time
        public int limitType = 0; //none, line, level, time, bravo
        public int limit = 0;
        public bool showGrade = true;
        public int initialGrade = 0;
        public int variant = 0;

        public bool comboing = false;
        public bool inCredits = false;
        public bool torikan = false;
        public long torDef = 0;
        public bool toriCredits = true;

        public int creditsType = 0;//normal, vanishing, invisible

        public int score = 0;
        public int combo = 1;
        public int grade = 0;
        public bool shiragrades = false;
        public bool drawSec = true;
        //public int gm2grade = 0;

        public int tetrises = 0;
        public List<int> secTet = new List<int>();

        public List<int> medals = new List<int>() { 0, 0, 0, 0, 0, 0 };

        public List<Gimmick> gimList = new List<Gimmick>();
        public List<List<int>> delayTable = new List<List<int>>();

        public int baseARE = 30;
        public int baseARELine = 30;
        public int baseDAS = 16;
        public int baseLock = 30;
        public int baseLineClear = 41;

        public Color border = Color.LightGray;

        public Mode()
        {

        }

        public virtual void onSpawn()
        {

        }

        public virtual void onTick(long time)
        {

        }

        public virtual void onSoft()
        {

        }

        public virtual void onPut(Tetromino tet, bool clear)
        {

        }

        public virtual void onClear(int lines, Tetromino tet, long time, bool bravo)
        {

        }

        public virtual void updateMusic()
        {

        }

        public virtual void draw(Graphics drawBuffer, Font f_Maestro, bool replay)
        {
            
        }
    }
}

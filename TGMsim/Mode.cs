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
        public struct Gimmick
        {
            public Type type; 
            public int startLvl;
            public int endLvl;
            public int parameter;
            public enum Type { FADING, INVIS, GARBAGE, BONES, ICE, BIG };
        }

        public enum ModeType { MASTER, DEATH, SHIRASE, SPRINT, GARBAGE, ROUNDS,  KONOHA, TRAINING, SEGA, MINER, DYNAMO, ENDURA, BLOX, PLUS, FLASH, MARCH, EASY }; //for the ID stuff

        public enum GarbType { FIXED, COPY, RANDOM, HIDDEN } ;

        public List<string> grades = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "M", "GM" };
        //public List<string> grades = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "M", "MK", "MV", "MO", "MM", "GM", "TM" }; //the old master list, just in case

        public string ModeName = "DUMMY";
        public ModeType modeID = 0;
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
        public int secBonus = 0;
        public int level = 0;
        //public int gradedBy = 0; //points, grade points, level, bravo, time //handled in-mode now
        public int limitType = 0; //none, line, level, time, bravo
        public int limit = 0;
        public bool showGrade = true;
        public int initialGrade = 0;
        public int variant = 0;
        public bool lockSafety = false;
        public bool hasDAD = false; //Delayed Auto Drop; i'm putting it here instead of in GameRules because i'm handling Flash Point as a mode

        public bool comboing = false;
        public bool inCredits = false;
        public bool modeClear = false;
        public bool hasCredits = true;
        public bool hasCreditsPause = true;
        public string creditsSong = "crdtvanish";
        public bool creditsClear = false;
        public bool speedUpCredits = false;
        public bool continueMode = false; //if true, do something such as load another board in sakura when modeClear is true
        public bool clearField = true;
        public bool torikan = false;
        public long torDef = 0;
        public bool toriCredits = true;
        public int toriLevel = 0;

        public int creditsType = 0;//normal, vanishing, invisible

        public int score = 0;
        public int combo = 1;
        public int grade = 0;
        public bool drawSec = true;
        public bool startWithRandField = false;
        public bool autoGarbage = false;
        public bool startEnd = false;
        public bool keepFieldSafe = false;
        public bool presetBoards = false;
        public string boardsFile = "";
        public int boardsProgress = 0;
        public int boardGems = 0;
        public List<int> targets = new List<int>();
        public bool showGhost = true;
        public bool orangeLine = false;
        public bool hasSecretGrade = false;
        public bool secretOnlyOnTopOut = false; //for TGM1. no reason new modes should ever make this true
        public int minSecret = 2;
        public List<string> secretGrades = new List<string> { "L1", "L2", "L3", "L4", "L5", "L6", "L7", "L8", "L9", "L10", "L11", "L12", "L13", "L14", "L15", "L16", "L17", "L18", "MAX", };

        public FrameTimer masteringTime = new FrameTimer();

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
        public bool shadeStack = true;
        public bool outlineStack = true;

        public bool firstPiece = true;

        public Mode()
        {

        }

        public virtual void onSpawn()
        {

        }

        public virtual void onTick(long time)
        {

        }

        public virtual void onTick(long time, Field.timerType type)
        {
            onTick(time);
        }

        public virtual void onSoft()
        {

        }

        public virtual void onPut(Tetromino tet, bool clear)
        {

        }

        public virtual void onClear(int lines, Tetromino tet, long time, bool bravo, bool split)
        {
            onClear(lines, tet, time, bravo);
        }

        public virtual void onClear(int lines, Tetromino tet, long time, bool bravo)
        {

        }

        public virtual void onGameOver()
        {

        }

        public virtual void updateMusic()
        {

        }

        public virtual void draw(Graphics drawBuffer, Font f, SolidBrush textBrush, bool replay)
        {
            
        }
    }
}

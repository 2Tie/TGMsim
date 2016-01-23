using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class Mode
    {
        public struct Gimmick
        {
            public int type; //fading, vanishing, garbage, bones, invisible
            public int startLvl;
            public int endLvl;
        }

        public int endLevel = 999;
        public List<int> themes = new List<int>();
        public bool bigmode = false;
        public bool g20 = false;
        public bool shiraseGrades = false;
        public int gradedBy = 0; //points, grade points, level, bravo, time
        public List<Gimmick> gimList = new List<Gimmick>();

        public void setMode(int mode)
        {
            switch(mode)//Master, Death, shirase
            {
                case 0:
                    endLevel = 999;
                    themes.Add(100);
                    themes.Add(200);
                    themes.Add(300);
                    themes.Add(400);
                    themes.Add(500);
                    themes.Add(600);
                    themes.Add(700);
                    themes.Add(800);
                    themes.Add(900);
                    break;
                case 1:
                    endLevel = 999;
                    themes.Add(100);
                    themes.Add(200);
                    themes.Add(300);
                    themes.Add(400);
                    themes.Add(500);
                    themes.Add(600);
                    themes.Add(700);
                    themes.Add(800);
                    themes.Add(900);
                    g20 = true;
                    gradedBy = 3;
                    break;
                case 2:
                    endLevel = 1299;
                    themes.Add(100);
                    themes.Add(200);
                    themes.Add(300);
                    themes.Add(400);
                    themes.Add(500);
                    themes.Add(600);
                    themes.Add(700);
                    themes.Add(800);
                    themes.Add(900);
                    themes.Add(1000);
                    themes.Add(1100);
                    themes.Add(1200);
                    var gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 700;
                    gL.endLvl = 900;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 3;
                    gL.startLvl = 1000;
                    gL.endLvl = 1299;
                    gimList.Add(gL);
                    gradedBy = 3;
                    shiraseGrades = true;
                    break;
            }
        }
        
    }
}

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

        public int id = 0;
        public int endLevel = 999;
        public List<int> sections = new List<int>();
        public bool bigmode = false;
        public bool g20 = false;
        public bool shiraseGrades = false;
        public bool exam = false;
        public int gradedBy = 0; //points, grade points, level, bravo, time
        public int limitType = 0; //none, line, level, time
        public int limit = 0;
        public List<Gimmick> gimList = new List<Gimmick>();

        public void setMode(int mode)
        {
            switch(mode)//Master, Death, shirase, sprint
            {
                case 0:
                    id = 0;
                    endLevel = 999;
                    sections.Add(100);
                    sections.Add(200);
                    sections.Add(300);
                    sections.Add(400);
                    sections.Add(500);
                    sections.Add(600);
                    sections.Add(700);
                    sections.Add(800);
                    sections.Add(900);
                    break;
                case 1:
                    id = 1;
                    endLevel = 999;
                    sections.Add(100);
                    sections.Add(200);
                    sections.Add(300);
                    sections.Add(400);
                    sections.Add(500);
                    sections.Add(600);
                    sections.Add(700);
                    sections.Add(800);
                    sections.Add(900);
                    g20 = true;
                    gradedBy = 2;
                    break;
                case 2:
                    id = 2;
                    endLevel = 1299;
                    sections.Add(100);
                    sections.Add(200);
                    sections.Add(300);
                    sections.Add(400);
                    sections.Add(500);
                    sections.Add(600);
                    sections.Add(700);
                    sections.Add(800);
                    sections.Add(900);
                    sections.Add(1000);
                    sections.Add(1100);
                    sections.Add(1200);
                    g20 = true;
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
                    gradedBy = 2;
                    shiraseGrades = true;
                    break;
            }
        }
        
    }
}

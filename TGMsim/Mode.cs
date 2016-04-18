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
            public int type; //fading, vanishing, garbage, bones, ice, big
            public int startLvl;
            public int endLvl;
        }

        public int id = 0;
        public int endLevel = 999;
        public List<int> sections = new List<int>();
        public bool bigmode = false;
        public bool g20 = false;
        public bool easyGen = false;
        public bool shiraseGrades = false;
        public int exam = -1;
        public int lvlBonus = 0;
        public int gradedBy = 0; //points, grade points, level, bravo, time
        public int limitType = 0; //none, line, level, time, bravo
        public int limit = 0;
        public bool mute = false;
        public List<Gimmick> gimList = new List<Gimmick>();

        public void setMode(int mode)
        {
            id = mode;
            switch(mode)//Master, Death, shirase, sprint, garbage clear, rounds, konoha
            {
                case 0:
                    gradedBy = 1;
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
                    sections.Add(999);
                    break;
                case 1:
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
                    sections.Add(999);
                    lvlBonus = 5;
                    gradedBy = 2;
                    break;
                case 2:
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
                    lvlBonus = 5;
                    var gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 700;
                    gL.endLvl = 1000;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 3;
                    gL.startLvl = 1000;
                    gL.endLvl = 1300;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 5;
                    gL.startLvl = 1299;
                    gL.endLvl = 1300;
                    gimList.Add(gL);
                    gradedBy = 2;
                    shiraseGrades = true;
                    break;
                case 3:
                    gradedBy = 4;
                    limitType = 1;
                    break;
                case 4:
                    gradedBy = 4;
                    limitType = 4;
                    limit = 1;
                    sections.Add(999);
                    break;
                case 6:
                    endLevel = 0;
                    gradedBy = 3;
                    limitType = 3;
                    limit = 180000;//three minutes
                    sections.Add(999);
                    bigmode = true;
                    easyGen = true;
                    break;
            }
        }
        
    }
}

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
            public int type; //fading, vanishing, garbage, bones, ice, big
            public int startLvl;
            public int endLvl;
            public int parameter; //only used for garbage right now...
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

        public Color border = Color.LightGray;

        public void setMode(int mode)
        {
            id = mode;
            switch(mode)//Master, Death, shirase, sprint, garbage clear, rounds, konoha, grav training
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
                    border = Color.DarkRed;
                    g20 = true;
                    break;
                case 2:
                    endLevel = 1300;
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
                    gL.startLvl = 500;
                    gL.endLvl = 600;
                    gL.parameter = 20;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 600;
                    gL.endLvl = 700;
                    gL.parameter = 18;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 700;
                    gL.endLvl = 800;
                    gL.parameter = 10;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 800;
                    gL.endLvl = 900;
                    gL.parameter = 9;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 900;
                    gL.endLvl = 1000;
                    gL.parameter = 8;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 3;
                    gL.startLvl = 1000;
                    gL.endLvl = 1300;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 5;
                    gL.startLvl = 1300;
                    gL.endLvl = 1301;
                    gimList.Add(gL);
                    gradedBy = 2;
                    shiraseGrades = true;
                    border = Color.DarkBlue;
                    g20 = true;
                    break;
                case 3:
                    gradedBy = 4;
                    limitType = 1;
                    border = Color.DarkGreen;
                    break;
                case 4:
                    gradedBy = 4;
                    limitType = 4;
                    limit = 1;
                    sections.Add(999);
                    break;
                case 5:
                    gradedBy = 2;
                    lvlBonus = 5;
                    endLevel = 1200;
                    g20 = true;

                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 300;
                    gL.endLvl = 400;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 500;
                    gL.endLvl = 600;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 700;
                    gL.endLvl = 800;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 900;
                    gL.endLvl = 1000;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 1100;
                    gL.endLvl = 1200;
                    gimList.Add(gL);
                    border = Color.DarkRed;
                    break;
                case 6:
                    endLevel = 0;
                    gradedBy = 3;
                    limitType = 3;
                    limit = 180000;//three minutes
                    bigmode = true;
                    easyGen = true;
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
                    border = Color.PaleVioletRed;
                    break;
                case 7:
                    endLevel = 200;
                    g20 = true;
                    gradedBy = 4;
                    
                    sections.Add(200);
                    break;
            }
        }
        
    }
}

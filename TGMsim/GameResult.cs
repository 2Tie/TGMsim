using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class GameResult
    {
        
        public int game;
        public Mode.ModeType mode;
        public int score;
        public long time;
        public long rawTime;
        public int grade;
        public int level;
        public string username;
        public List<int> medals;
        public int lineC;
        public bool delay;
        public string code;

        public bool calcCode() //credit to LOst for reversing the format
        {
            List<byte> caScramble = new List<byte>
            {
            0x03, 0x0A, 0x12, 0x01, 0x10, 0x0E, 0x08, 0x06, 0x0E, 0x01, 0x0F, 0x0E, 0x02, 0x03, 0x02, 0x08,
            0x05, 0x10, 0x05, 0x07, 0x01, 0x0B, 0x12, 0x11, 0x0F, 0x0E, 0x09, 0x0F, 0x0A, 0x02, 0x05, 0x0B,
            0x12, 0x0F, 0x07, 0x05, 0x0B, 0x09, 0x06, 0x0F, 0x0C, 0x10, 0x04, 0x03
            };
            List<byte> codeData = new List<byte>(16);
            //encode mode
            ushort m;
            switch (mode)
            {
                case Mode.ModeType.SEGA://normal //TODO: make this NORMAL
                    m = 0;
                    break;
                case Mode.ModeType.MASTER: //master
                    m = 1;
                    break;
                case Mode.ModeType.PLUS: //plus
                    m = 2;
                    break;
                case Mode.ModeType.DEATH: //death
                    m = 3;
                    break;
                default:
                    throw new Exception("wrong mode, can't encode!");
            }
            codeData.Add((byte)(m << 2));
            //encode grade
            codeData[0] += (byte)((grade>>3) & 0x3);
            codeData.Add((byte)((grade & 0x7) << 1));
            //encode level
            codeData[1] += (byte)((level >> 9) & 0x1);
            codeData.Add((byte)((level >> 5) & 0xF));
            codeData.Add((byte)((level >> 1) & 0xF));
            codeData.Add((byte)((level & 0x1) << 3));
            //encode line
            codeData[4] += (byte)(lineC << 1);

            long ltemp = 0;
            //encode score or line, depending on mode
            if(m == 0) //normal
                ltemp = score;
            else
                ltemp = rawTime;
            codeData[4] += (byte)((ltemp >> 19) & 1);
            codeData.Add((byte)((ltemp >> 15) & 0xF));
            codeData.Add((byte)((ltemp >> 11) & 0xF));
            codeData.Add((byte)((ltemp >> 7) & 0xF));
            codeData.Add((byte)((ltemp >> 3) & 0xF));
            codeData.Add((byte)((ltemp & 0x7) << 1));
            //encode medals
            codeData[9] += (byte)((medals[0] >> 1) & 0x1);
            codeData.Add((byte)((medals[0] & 0x1) << 3));
            codeData[10] += (byte)(medals[2] << 1);
            codeData[10] += (byte)((medals[3] >> 1) & 0x1);
            codeData.Add((byte)((medals[3] & 0x1) << 3));
            codeData[11] += (byte)(medals[4] << 1);
            codeData[11] += (byte)((medals[1] >> 1) & 0x1);
            codeData.Add(0);
            codeData.Add(0);
            codeData.Add((byte)((medals[1] & 0x1) << 3));
            codeData[14] += (byte)(medals[5] << 1);

            //select the scramble seed?
            byte seed = (byte)(new Random().Next(32));
            codeData[14] += (byte)((seed >> 4) & 0x1);
            codeData.Add((byte)(seed & 0xF));

            //create the checksum
            byte checksum = 0;
            byte offset = 0;
            for (int i = 0; i < codeData.Count; i++)
                checksum += codeData[i];
            //add the name and seed
            for (int i = 0; i < username.Length; i++)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?. \\[&".Contains(username.Substring(i, 1)))
                    offset += (byte)username.Substring(i, 1).ToCharArray()[0]; //oof
                else
                    throw new Exception("bad username for code gen");
            }
            checksum += offset;
            //store checksum
            codeData[12] += (byte)((checksum >> 4) & 0xF);
            codeData[13] += (byte)(checksum & 0xF);

            //scramble the data: 
            for (int i = 0; i < 12; i++)
            {
                codeData[i] += caScramble[seed];
                codeData[i] %= 20;
                seed++;
            }
            //shift the data as well
            for (int i = 0; i < 16; i++)
                code += "34679CDEFGHJKMPRTWXY".Substring(codeData[((offset&0xF) + i) & 0xF], 1);

            return true;
        }
    }
}

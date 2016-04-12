using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class Profile
    {
        public String name;
        public int decoration = 0;
        public int globalDecoration = 0;
        public bool displayGlobal = false;
        public List<byte> password = new List<byte>();
        public bool passProtected = false;
        public int TIGrade = 0;
        public List<int> TIHistory = new List<int>{0,0,0,0,0,0,0};
        public List<bool> certs = new List<bool>(10); //Official GM certifications (1, 2, tap, tap death, 3, 3 shirase, ACE TM, 4 world, 4 rounds, 4 konoha)

        public bool createUser()
        {
            using (FileStream fsStream = new FileStream("Sav/" + name + ".usr", FileMode.Create))
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                sw.Write(name);
                sw.Write((byte)0x03);//version number
                //one byte for password, the first bit if pass-protected, three bits for length (up to six) and then two bits for each digit (four possible inputs each, ABCH)
                UInt16 passData = new UInt16();
                if (!passProtected)
                {
                    sw.Write(passData);
                }
                else
                {
                    passData += 0x8000;
                    passData += (UInt16)((password.Count & 0x7) << 12);//password length
                    for (int i = 0; i < password.Count; i++)
                    {
                        passData += (UInt16)(password[i] << (10 - 2 * i));
                    }
                    sw.Write(passData);
                    
                }
                sw.Write(new byte[4]);//global points
                sw.Write(new byte[4]);//TGM3 points
                sw.Write(new byte[2]);//Official GM certifications (1, 2, tap, tap death, 3, 3 shirase, ACE TM, 4 world, 4 rounds, 4 konoha)
                sw.Write(new byte[2]);//endless shirase hiscore?
                sw.Write(new byte[6]);//current TI grade + previous seven rankings
                sw.Write(new byte[1]);//ACE grade
                sw.Write(new byte[1]);//unused
            }
            return false;
        }

        public int readPass()
        {
            //read the pass and put it into verifyPass
            BinaryReader file = new BinaryReader(File.OpenRead("Sav/" + name + ".usr"));
            if (file.ReadString() != name)//read name
                return 1; //bad name
            if (file.ReadByte() != 0x03)//read save version, compare to current
                return 2; //incorrect save
            //read and parse the password
            UInt16 passdata = file.ReadUInt16();
            if (passdata >> 15 == 1)//pass protected
            {
                if (((passdata >> 12) & 0x3) == 7)//invalid pass length
                {
                    return 3;
                }
                passProtected = true;
                password.Add((byte)((passdata >> 10) & 0x0003));
                password.Add((byte)((passdata >> 8) & 0x0003));
                password.Add((byte)((passdata >> 6) & 0x0003));
                password.Add((byte)((passdata >> 4) & 0x0003));
                password.Add((byte)((passdata >> 2) & 0x0003));
                password.Add((byte)(passdata & 0x0003));
            }
            else
                passProtected = false;
            return 0;
        }

        public bool readUserData()
        {
            //read the user data and pass it to the game
            BinaryReader file = new BinaryReader(File.OpenRead("Sav/" + name + ".usr"));
            if (file.ReadString() != name)//read name
                return false;
            if (file.ReadByte() != 0x03)//read save version, compare to current
                return false;
            //read and parse the password
            UInt16 passdata = file.ReadUInt16();
            //global points
            globalDecoration = file.ReadInt32();
            //tgm3 points
            decoration = file.ReadInt32();
            //GM certs
            ushort certB = file.ReadUInt16();
            for (int i = 0; i < 10; i++ )
            {
                certs.Add(((certB >> (10 - i)) & 0x1) == 1);
            }
            //shirase
            file.ReadBytes(2);
            //TI grade data
            byte[] tempbyte = new byte[6];
            tempbyte = file.ReadBytes(6);
            TIHistory.Clear();

            TIGrade = (tempbyte[0] & 0xFC) >> 2;
            TIHistory.Add(((tempbyte[0] & 0x03) << 4) + ((tempbyte[1] & 0xF0) >> 4));
            TIHistory.Add(((tempbyte[1] & 0x0F) << 2) + ((tempbyte[2] & 0xC0) >> 6));
            TIHistory.Add(tempbyte[2] & 0x3F);
            TIHistory.Add((tempbyte[3] & 0xFC) >> 2);
            TIHistory.Add(((tempbyte[3] & 0x03) << 4) + ((tempbyte[4] & 0xF0) >> 4));
            TIHistory.Add(((tempbyte[4] & 0x0F) << 2) + ((tempbyte[5] & 0xC0) >> 6));
            TIHistory.Add(tempbyte[5] & 0x3F);


            //ACE grade data
            file.ReadByte();
            //future use
            file.ReadByte();
            return true;
        }

        public bool updateUser()
        {
            using (FileStream fsStream = new FileStream("Sav/" + name + ".usr", FileMode.Truncate))
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                //sw.Seek(21, SeekOrigin.Begin);
                sw.Write(name);
                sw.Write((byte)0x03);//version number
                //one byte for password, the first bit if pass-protected, three bits for length (up to six) and then two bits for each digit (four possible inputs each, ABCH)
                UInt16 passData = new UInt16();
                if (!passProtected)
                {
                    sw.Write(passData);
                }
                else
                {
                    passData += 0x8000;
                    passData += (UInt16)((password.Count & 0x7) << 12);//password length
                    for (int i = 0; i < password.Count; i++)
                    {
                        passData += (UInt16)(password[i] << (10 - 2 * i));
                    }
                    sw.Write(passData);
                }
                sw.Write(globalDecoration);//global points
                sw.Write(decoration);//TGM3 points

                UInt16 certB = new ushort();
                for (int i = 0; i < 10; i++ )
                {
                    if (certs[i])
                        certB += (ushort)(0x1 << (10 - i));
                }
                sw.Write(certB);//Official GM certifications (1, 2, tap, tap death, 3, 3 shirase, ACE TM, 4 world, 4 rounds, 4 konoha)
                sw.Write(new byte[2]);//endless shirase hiscore?

                byte[] tempbyte = new byte[6];
                tempbyte[0] = (byte)((TIGrade << 2) + ((TIHistory[0] & 0x30) >> 4));
                tempbyte[1] = (byte)(((TIHistory[0] & 0x0F) << 4) + ((TIHistory[1] & 0x3C) >> 2));
                tempbyte[2] = (byte)(((TIHistory[1] & 0x03) << 6) + ((TIHistory[2] & 0x3F)));
                tempbyte[3] = (byte)((TIHistory[3] << 2) + ((TIHistory[4] & 0x30) >> 4));
                tempbyte[4] = (byte)(((TIHistory[4] & 0x0F) << 4) + ((TIHistory[5] & 0x3C) >> 2));
                tempbyte[5] = (byte)(((TIHistory[5] & 0x03) << 6) + ((TIHistory[6] & 0x3F)));

                sw.Write(tempbyte);//current TI grade + previous seven rankings
                sw.Write(new byte[1]);//ACE grade
                sw.Write(new byte[1]);//unused
            }
            return true;
        }
    }
}

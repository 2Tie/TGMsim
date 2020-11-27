using System.Collections.Generic;

namespace TGMsim
{
    public class Tetromino
    {
        public enum Piece { I = 1, Z, S, J, L, O, T };

        public Piece id = 0;
        public int rotation = 0;
        public int x = 3;
        public int y = 20;
        public int kicked = 0;
        public bool bone = false;
        public bool big = false;
        public bool swapped = false;
        public bool floored = false;
        public bool spun = false;
        public bool gemmed = false;
        public int gemPip = 0;
        public int groundTimer = 0;
        public ItemType item = 0;

        //stats
        public int soft = 0;
        public int sonic = 0;
        public int life = 0;
        public int rotations = 0;

        public enum ItemType { none, freefall, deleven, delfour, weight16, shot, bomb, bird };

        public List<BlockBit> bits = new List<BlockBit>(); //first four will always be the "small" version

        public Tetromino(Piece i, bool nbig) : this(i, 0, 0, 0, nbig)
        {

        }

        public Tetromino(Piece i, int r, int nx, int ny, bool nbig)
        {
            id = i;
            rotation = r;
            big = nbig;
            switch (i)
            {
                case Piece.I://I
                    switch (r)
                    {
                        case 0:
                            bits.Add(new BlockBit(0, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(3, 1));
                            break;
                        case 2:
                            bits.Add(new BlockBit(3, 1));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(0, 1));
                            break;
                        case 1:
                            bits.Add(new BlockBit(2, 3));
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(2, 0));
                            break;
                        case 3:
                            bits.Add(new BlockBit(2, 0));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(2, 3));
                            break;
                    }
                    break;
                case Piece.Z://Z
                    switch (r)
                    {
                        case 0:
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(0, 1));
                            break;
                        case 2:
                            bits.Add(new BlockBit(0, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(2, 2));
                            break;
                        case 1:
                            bits.Add(new BlockBit(2, 0));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            break;
                        case 3:
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(2, 0));
                            break;
                    }
                    break;
                case Piece.S://S
                    switch (r)
                    {
                        case 0:
                            bits.Add(new BlockBit(0, 2));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(2, 1));
                            break;
                        case 2:
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(0, 2));
                            break;
                        case 1:
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(0, 1));
                            bits.Add(new BlockBit(0, 0));
                            break;
                        case 3:
                            bits.Add(new BlockBit(0, 0));
                            bits.Add(new BlockBit(0, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            break;
                    }
                    break;
                case Piece.J://J
                    switch (r)
                    {
                        case 0:
                            bits.Add(new BlockBit(0, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(2, 2));
                            break;
                        case 1:
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 0));
                            bits.Add(new BlockBit(2, 0));
                            break;
                        case 2:
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(0, 2));
                            bits.Add(new BlockBit(0, 1));
                            break;
                        case 3:
                            bits.Add(new BlockBit(1, 0));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(0, 2));
                            break;
                    }
                    break;
                case Piece.L://L
                    switch (r)
                    {
                        case 0:
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(0, 1));
                            bits.Add(new BlockBit(0, 2));
                            break;
                        case 1:
                            bits.Add(new BlockBit(1, 0));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(2, 2));
                            break;
                        case 2:
                            bits.Add(new BlockBit(0, 2));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(2, 1));
                            break;
                        case 3:
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 0));
                            bits.Add(new BlockBit(0, 0));
                            break;
                    }
                    break;
                case Piece.O://O
                    switch (r)
                    {
                        case 0:
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(1, 1));
                            break;
                        case 1:
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            break;
                        case 2:
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(2, 2));
                            break;
                        case 3:
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(2, 1));
                            break;
                    }
                    break;
                case Piece.T://T
                    switch (r)
                    {
                        case 0:
                            bits.Add(new BlockBit(0, 1));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(2, 1));
                            bits.Add(new BlockBit(1, 2));
                            break;
                        case 1:
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 0));
                            bits.Add(new BlockBit(2, 1));
                            break;
                        case 2:
                            bits.Add(new BlockBit(2, 2));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(0, 2));
                            bits.Add(new BlockBit(1, 1));
                            break;
                        case 3:
                            bits.Add(new BlockBit(1, 0));
                            bits.Add(new BlockBit(1, 1));
                            bits.Add(new BlockBit(1, 2));
                            bits.Add(new BlockBit(0, 1));
                            break;
                    }
                    break;
            }

            int b = 2;
            if (this.big) b = 1;

            for (int p = 0; p < bits.Count; p++)
            {
                bits[p].x = this.x + ((bits[p].x * (2 / b)) - (1 % (3 - b)));
                bits[p].y = this.y - ((bits[p].y * (2 / b)) - (1 % (3 - b)));
            }

            move(nx, ny);
        }

        public void move(int mx, int my)
        {
            for(int i = 0; i < bits.Count; i++)
            {
                bits[i].move(mx, my);
            }
            this.x += mx;
            this.y += my;
            spun = false;
        }

        public Tetromino clone()
        {
            return clone(this.rotation);
        }

        public Tetromino cloneBig(bool rbig)
        {
            Tetromino newTet = new Tetromino(id, this.rotation, this.x - 3, this.y - 20, rbig);
            return copyExtra(newTet);
        }

        public Tetromino clone(int ro)
        {
            Tetromino newTet = new Tetromino(id, ro, this.x - 3, this.y - 20, this.big);
            return copyExtra(newTet);
        }

        public Tetromino clone(bool reset, bool rbig)
        {
            int rot = 0;
            if (reset) rot = this.rotation;
            Tetromino newTet = new Tetromino(id, rot, 0, 0, rbig);
            return copyExtra(newTet);
        }

        private Tetromino copyExtra(Tetromino tet)
        {
            tet.bone = bone;
            tet.kicked = kicked;
            tet.groundTimer = groundTimer;
            tet.soft = soft;
            tet.sonic = sonic;
            tet.life = life;
            tet.rotations = rotations;
            tet.floored = floored;
            tet.spun = spun;
            tet.gemmed = gemmed;
            tet.gemPip = gemPip;
            tet.item = item;
            return tet;
        }
    }
}

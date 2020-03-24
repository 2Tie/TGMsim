namespace TGMsim
{
    public class BlockBit
    {
        public int x, y;
        public BlockBit(int newX, int newY)
        {
            x = newX;
            y = newY;
        }
        public void move(int offX, int offY)
        {
            x += offX;
            y += offY;
        }
    }
}

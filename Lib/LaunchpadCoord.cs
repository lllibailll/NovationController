namespace Lib
{
    public class LaunchpadCoord
    {
        private int _x;
        private int _y;

        public LaunchpadCoord(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X => _x;

        public int Y => _y;
    }
}
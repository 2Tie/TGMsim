using System.Diagnostics;

namespace TGMsim
{
    class GameTimer
    {
        Stopwatch stopwatch;

        public GameTimer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Reset();
        }

        public long elapsedTime
        {
            get { return stopwatch.ElapsedMilliseconds; }
        }

        public void start()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Reset();
                stopwatch.Start();
            }
        }

        public void stop()
        {
            stopwatch.Stop();
        }

        public void reset()
        {
            stopwatch.Reset();
        }
    }
}

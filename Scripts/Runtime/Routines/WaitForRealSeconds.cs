namespace JvLib.Routines
{
    public class WaitForRealSeconds : IRoutineYield
    {
        private readonly float _startTime;
        private readonly float _seconds;

        public WaitForRealSeconds(float pDuration)
        {
            _startTime = UnityEngine.Time.realtimeSinceStartup;
            _seconds = pDuration;
        }

        public bool Running =>
            !(_startTime + _seconds
              < UnityEngine.Time.realtimeSinceStartup);

        public bool Stopped { get; private set; }

        public void Stop()
        {
            Stopped = true;
        }
    }
}

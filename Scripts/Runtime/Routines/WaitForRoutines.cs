using System;
using System.Collections.Generic;

namespace JvLib.Routines
{
    public class WaitForRoutines : IRoutineYield
    {
        private readonly IList<Routine> _routines;

        public WaitForRoutines(IList<Routine> pRoutines)
        {
            _routines = pRoutines;
        }

        public WaitForRoutines(params Routine[] pRoutines)
        {
            _routines = pRoutines;
        }

        public bool Running
        {
            get
            {
                foreach (Routine t in _routines)
                    if (t is {Ended: false})
                        return true;

                return false;
            }
        }

        public bool Stopped { get; private set; }

        public void Stop()
        {
            Stopped = true;
        }
    }
}

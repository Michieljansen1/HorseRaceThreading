using Horserace.Models;

namespace Horserace.Events
{
    /**
     *
     */
    class HorseProgressReport
    {
        //
        private Horse _horse { get;}

        // 
        private int _totalTime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="horse"></param>
        /// <param name="totalTime"></param>
        public HorseProgressReport(Horse horse, int totalTime)
        {
            _horse = horse;
            _totalTime = totalTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotalTime
        {
            get{ return _totalTime; }
        }
    }
}

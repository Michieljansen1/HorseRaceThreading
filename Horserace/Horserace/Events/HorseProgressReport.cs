using System;

namespace Horserace.Events
{
    /**
     *
     */
    class HorseProgressReport : EventArgs
    {
        // 
        private readonly int _totalTime;
        private readonly int _pingIteration;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="totalTime"></param>
        public HorseProgressReport(int totalTime, int pingIteration)
        {
            _totalTime = totalTime;
            _pingIteration = pingIteration;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotalTime
        {
            get{ return _totalTime; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PingIteration {
            get { return _pingIteration; }
        }
    }
}

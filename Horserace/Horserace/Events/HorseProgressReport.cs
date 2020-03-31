using Horserace.Models;
using System;

namespace Horserace.Events
{
    /**
     *
     */
    class HorseProgressReport : EventArgs
    {
        // 
        private int _totalTime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="totalTime"></param>
        public HorseProgressReport(int totalTime)
        {
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

using System;

namespace Horserace.Events
{
    /// <summary>
    ///     EventArgs to use whenever the horse progress changes
    /// </summary>
    class HorseProgressEventArgs : EventArgs
    {
        // 
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="totalTime"></param>
        /// <param name="pingIteration"></param>
        public HorseProgressEventArgs(int totalTime, int pingIteration)
        {
            TotalTime = totalTime;
            PingIteration = pingIteration;
        }

        /// <summary>
        /// </summary>
        public int TotalTime { get; }

        /// <summary>
        /// </summary>
        public int PingIteration { get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private int _totalTime { get; }
        //Constructor
        public HorseProgressReport(Horse horse, int totalTime)
        {
            _horse = horse;
            _totalTime = totalTime;
        }
    }
}

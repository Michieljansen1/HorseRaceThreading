using System;
using Horserace.Models;

namespace Horserace.Events
{
    class HorseChangedEventArgs : EventArgs
    {
        private Horse _horse;

        public HorseChangedEventArgs(Horse horse)
        {
            _horse = horse;
        }

        public Horse Horse => _horse;
    }
}

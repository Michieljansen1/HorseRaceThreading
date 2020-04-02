using System;
using Horserace.Models;

namespace Horserace.Events
{
    /// <summary>
    ///     EventArgs to use whenever a horse changed
    /// </summary>
    class HorseChangedEventArgs : EventArgs
    {
        public HorseChangedEventArgs(Horse horse)
        {
            Horse = horse;
        }

        public Horse Horse { get; }
    }
}
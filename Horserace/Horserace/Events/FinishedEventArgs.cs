using Horserace.Enums;

namespace Horserace.Events
{
    /// <summary>
    ///     EventArgs to define whether a thread is canceled by user or finished by itself.
    /// </summary>
    class FinishedEventArgs
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="finishType"></param>
        public FinishedEventArgs(FinishType finishType)
        {
            Type = finishType;
        }

        /// <summary>
        ///     Gets the set Type
        /// </summary>
        public FinishType Type { get; }
    }
}
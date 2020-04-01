namespace Horserace.Events
{
    /// <summary>
    /// EventArgs to define whether a thread is canceled by user or finished by itself.
    /// </summary>
    class FinishedEventArgs
    {
        public enum FinishType
        {
            FINISHED,
            CANCELED,
            ERROR
        }
        private FinishType _finishType;

        public FinishedEventArgs(FinishType finishType)
        {
            _finishType = finishType;
        }

        public FinishType Type => _finishType;
    }
}

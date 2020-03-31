using Horserace.Events;

namespace Horserace.Models
{

    /**
    *
    */
    class Horse
    {

        private string _name;
        private string _url;
        private int _totalPings;
        private int _distance;

        public Horse(string name, int totalPings, string url)
        {
            _name = name;
            _totalPings = totalPings;
            _url = url;
        }

        public void UpdateProgress(HorseProgressReport progress)
        {
            _distance = progress.TotalTime;
        }

        public void Start()
        {

        }

        public string Name => _name;

        public int Distance => _distance;
    }
}

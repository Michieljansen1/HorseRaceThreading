using Horserace.Common;
using Horserace.Events;

namespace Horserace.Models
{

    /**
    *
    */
    class Horse //: INotifyPropertyChanged
    {

        private string _name;
        private string _url;
        private int _totalPings;
        private int _distance;
        private Ping _ping;

        public Horse(string name, int totalPings, string url)
        {
            _name = name;
            _totalPings = totalPings;
            _url = url;
            _ping = new Ping(url, totalPings);
            _ping._pingReceived += PingReceived;
        }

        private void PingReceived(object sender, HorseProgressReport e)
        {
            _distance = e.TotalTime;
        }

        public void Start()
        {
            _ping.StartPing();
        }

        public string Name => _name;

        //public int Distance
        //{
        //    //get;
        //    //set { _distance = value;
        //    //    NotifyPropertyChanged() }
        //}
    }
}

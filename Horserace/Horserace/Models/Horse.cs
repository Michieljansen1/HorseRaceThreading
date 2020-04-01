using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Horserace.Common;
using Horserace.Events;

namespace Horserace.Models
{

    /**
    *
    */
    class Horse : INotifyPropertyChanged
    {
        //TODO refactor to enum dir
        public enum HorseStatus
        {
            IDLE,
            RUNNING,
            FINISHED
        }
        private readonly string _name;
        private readonly string _url;
        private int _distance;
        private readonly Ping _ping;
        private PageLoader _pageLoader;
        private int _furthestHorseDistance = 0;
        private int _currentRound = 1;
        private HorseStatus _horseStatus;
        public event EventHandler<HorseChangedEventArgs> _horseChanged;
        public event EventHandler _horseFinished;

        public Horse(string name, string url)
        {
            _name = name;
            _url = url;
            _ping = new Ping(url);
            _ping._pingReceived += PingReceived;
            _ping._threadFinished += PingFinished;
            _pageLoader = new PageLoader();
            Distance = 0;
        }

        private async void PingReceived(object sender, HorseProgressReport e)
        {
            Distance = e.TotalTime;
            CurrentRound = e.PingIteration;
        }

        private async void PingFinished(object sender, FinishedEventArgs e) {
            if (e.Type == FinishedEventArgs.FinishType.CANCELED)
            {
                Distance = 0;
            }

            _horseStatus = HorseStatus.FINISHED;
            OnHorseFinished();
        }

        public async void Start(int numberOfPings)
        {
            _horseStatus = HorseStatus.RUNNING;
            _ping.StartPing(numberOfPings);

            var size = await _pageLoader.Run(_url);

            Debug.WriteLine(size);
            Interlocked.Add(ref _distance, int.Parse(size.ToString()));

            // Distance += await _pageLoader.Run(_url);
        }

        public void Stop()
        {
            _horseStatus = HorseStatus.IDLE;
            _ping.StopPing();
        }
        public string Name => _name;

        public int Distance
        {
            get { return _distance; }
            set { 
                _distance = value;
                RaisePropertyChanged();
            }
        }

        public int FurthestHorseDistance {
            get => _furthestHorseDistance;
            set {
                _furthestHorseDistance = value;
                OnFurthestHorseChange();
            }
        }

        public int CurrentRound {
            get => _currentRound;
            set {
                _currentRound = value;
                Debug.WriteLine(_currentRound);
                OnFurthestHorseChange();
            }
        }

        public EventHandler HorseFinishedEvent
        {
            get { return _horseFinished; }
            set { _horseFinished = value; }
        }

        public HorseStatus Status
        {
            get { return _horseStatus; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = "") {
            _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                new DispatchedHandler(() => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                    OnHorseChanged(new HorseChangedEventArgs(this));
                }));
        }

        protected void OnFurthestHorseChange([CallerMemberName] string name = "") {
            _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                new DispatchedHandler(() => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }));
        }

        protected virtual void OnHorseChanged(HorseChangedEventArgs e) {
            EventHandler<HorseChangedEventArgs> handler = _horseChanged;
            if (handler != null) {
                handler(this, e);
            }
        }

        protected virtual void OnHorseFinished() {
            EventHandler handler = _horseFinished;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Horserace.Common;
using Horserace.Enums;
using Horserace.Events;
using Horserace.Utlis;

namespace Horserace.Models
{
    /// <summary>
    /// 
    /// </summary>
    class Horse : INotifyPropertyChanged
    {
        private readonly Ping _ping;
        private readonly string _url;
        private int _currentRound = 1;
        private int _distance;
        private int _furthestHorseDistance;
        private readonly PageLoader _pageLoader;

        public Horse(string name, string url)
        {
            Name = name;
            _url = url;
            _ping = new Ping(url);
            _ping.PingReceived += PingReceived;
            _ping.ThreadFinished += PingFinished;
            _pageLoader = new PageLoader();
            Distance = 0;
        }

        public string Name { get; }

        public int Distance
        {
            get { return _distance; }
            set {
                _distance = value;
                RaisePropertyChanged();
            }
        }

        public int FurthestHorseDistance
        {
            get => _furthestHorseDistance;
            set {
                _furthestHorseDistance = value;
                OnFurthestHorseChange();
            }
        }

        public int CurrentRound
        {
            get => _currentRound;
            set {
                _currentRound = value;
                OnFurthestHorseChange();
            }
        }

        public EventHandler HorseFinishedEvent
        {
            get { return _horseFinished; }
            set { _horseFinished = value; }
        }

        public HorseStatus Status { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<HorseChangedEventArgs> _horseChanged;
        public event EventHandler _horseFinished;

        private void PingReceived(object sender, HorseProgressEventArgs e)
        {
            Distance = e.TotalTime;
            CurrentRound = e.PingIteration;
        }

        private void PingFinished(object sender, FinishedEventArgs e)
        {
            switch (e.Type)
            {
                case FinishType.CANCELED:
                    Distance = 0;
                    break;
                case FinishType.ERROR:
                    ToastUtil.Notify("Disqualified", $"Horse {Name} has been disqualified");
                    Debug.WriteLine($"Horse {Name} has been disqualified");
                    break;
                case FinishType.FINISHED:
                    Status = HorseStatus.FINISHED;
                    OnHorseFinished();
                    break;
            }
        }

        public async void Start(int numberOfPings)
        {
            Status = HorseStatus.RUNNING;
            _ping.StartPing(numberOfPings);
            _ping.AddTime(await _pageLoader.Run(_url));
        }

        public void Stop()
        {
            Status = HorseStatus.IDLE;
            _ping.StopPing();
        }

        protected void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                    OnHorseChanged(new HorseChangedEventArgs(this));
                });
        }

        protected void OnFurthestHorseChange([CallerMemberName] string name = "")
        {
            _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                });
        }

        protected virtual void OnHorseChanged(HorseChangedEventArgs e)
        {
            EventHandler<HorseChangedEventArgs> handler = _horseChanged;
            handler?.Invoke(this, e);
        }

        protected virtual void OnHorseFinished()
        {
            EventHandler handler = _horseFinished;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
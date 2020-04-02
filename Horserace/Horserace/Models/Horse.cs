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
    ///     Main logic for the horse
    /// </summary>
    class Horse : INotifyPropertyChanged
    {
        private readonly PageLoader _pageLoader; // Used to get the DOM size
        private readonly Ping _ping; // Used to ping the server
        private readonly string _url; // Holds the URL of the website to ping / load dom
        private int _currentRound = 1; // Current ping iteration
        private int _distance; // Total distance the horse has ran (in steps)
        private int _furthestHorseDistance; // Distance of the furthest horse (can be another horse)

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
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

        /// <summary>
        /// Getter and setter for distance. Raises property changed event to signal the GUI to update
        /// </summary>
        public int Distance
        {
            get { return _distance; }
            set {
                _distance = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Getter and setter for distance. Raises event on SET to signal the GUI to update
        /// </summary>
        public int FurthestHorseDistance
        {
            get => _furthestHorseDistance;
            set {
                _furthestHorseDistance = value;
                OnFurthestHorseChange();
            }
        }

        /// <summary>
        /// Getter and setter for the CurrentRound. Raises event on SET to signal the GUI to update
        /// </summary>
        public int CurrentRound
        {
            get => _currentRound;
            set {
                _currentRound = value;
                OnFurthestHorseChange();
            }
        }

        /// <summary>
        /// Getter and setter for HorseFinishedEvent.
        /// </summary>
        public EventHandler HorseFinishedEvent
        {
            get { return _horseFinished; }
            set { _horseFinished = value; }
        }

        public HorseStatus Status { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged; // Used to signal the UI to update the binded values
        public event EventHandler<HorseChangedEventArgs> _horseChanged; // Used to signal the GameController that a horse changed
        public event EventHandler _horseFinished; // Used to signal the GameController that the horse finished

        /// <summary>
        /// Sets the horse to status running and starts pinging/ fetching dom size
        /// </summary>
        /// <param name="numberOfPings"></param>
        public async void Start(int numberOfPings)
        {
            Status = HorseStatus.RUNNING;
            _ping.StartPing(numberOfPings);
            _ping.AddTime(await _pageLoader.Run(_url));
        }

        /// <summary>
        /// Sets the horse status to IDLE and signals the ping thread to stop
        /// </summary>
        public void Stop()
        {
            Status = HorseStatus.IDLE;
            _ping.StopPing();
        }

        /// <summary>
        /// Signals the GUI to update the binded elements and signals the GameController that the horse changed a property
        /// </summary>
        /// <param name="name"></param>
        protected void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                    OnHorseChanged(new HorseChangedEventArgs(this));
                });
        }

        /// <summary>
        /// Signals the GUI to update the GUI
        /// </summary>
        /// <param name="name"></param>
        protected void OnFurthestHorseChange([CallerMemberName] string name = "")
        {
            _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                });
        }

        /// <summary>
        /// Signals all listening events that the horse changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnHorseChanged(HorseChangedEventArgs e)
        {
            var handler = _horseChanged;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Signals all listening events that the horse finished racing
        /// </summary>
        protected virtual void OnHorseFinished()
        {
            var handler = _horseFinished;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Triggers when a new ping event is received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Current horse progress</param>
        private void PingReceived(object sender, HorseProgressEventArgs e)
        {
            Distance = e.TotalTime;
            CurrentRound = e.PingIteration;
        }

        /// <summary>
        /// Triggers when the ping thread is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event with the finish type</param>
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
    }
}
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Horserace.Events;
using Horserace.Models;

namespace Horserace.Controllers
{
    class GameController : INotifyPropertyChanged
    {
        private ObservableCollection<Horse> _horses;
        private int _furthestHorseDistance = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameController()
        {
            _horses = new ObservableCollection<Horse>();
        }

        private void HorseChanged(object sender, HorseChangedEventArgs e)
        {
            // TODO: BindingExpression path error: 'FurthestHorseDistance' property not found on 'Horserace.Models.Horse'
            if (_furthestHorseDistance < e.Horse.Distance)
            {
                FurthestHorseDistance = e.Horse.Distance;
            }
        }

        /// <summary>
        /// Starts the horse racing game
        /// </summary>
        public void Start()
        {
            //TODO: only start game when enough valid horses have been added to the race
            foreach (var horse in _horses)
            {
                horse.Start();   
            }
        }

        /// <summary>
        /// Creates a new horse and adds it to the horse list
        /// </summary>
        /// <param name="name">Name of the horse</param>
        /// <param name="totalPings">Total amount of pings that will be executed</param>
        /// <param name="url">The url that will be pinged</param>
        public void AddHorse(string name, int totalPings, string url)
        {
            Horse horse = new Horse(name, totalPings, url);
            horse._horseChanged += HorseChanged;
            _horses.Add(horse);
        }

        /// <summary>
        /// Clears all the horses from the game
        /// </summary>
        public void Reset()
        {
            _horses.Clear();
        }

        public ObservableCollection<Horse> Horses => _horses;

        public int FurthestHorseDistance
        {
            get => _furthestHorseDistance;
            set {
                _furthestHorseDistance = value;
                Debug.WriteLine("Max distance = " + value);
                RaisePropertyChanged();
            }
        }
      
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = "") {
            _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                new DispatchedHandler(() => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }));
        }
    }
}

using System.Collections.ObjectModel;
using Horserace.Events;
using Horserace.Models;

namespace Horserace.Controllers
{
    class GameController
    {
        private ObservableCollection<Horse> _horses;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameController()
        {
            _horses = new ObservableCollection<Horse>();
        }

        private void HorseChanged(object sender, HorseChangedEventArgs e)
        {
            int distance = 1;
            foreach (Horse horse in _horses)
            {
                if (distance < horse.Distance)
                {
                    distance = horse.Distance;
                }
            }

            foreach (Horse horse in _horses)
            {
                horse.FurthestHorseDistance = distance + (distance / 10);
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

    }
}

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        /// <summary>
        /// Receives an event whenever the horse distance changes and
        /// sets the progress bar's maximum based on the horse that has traveled the longest distance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">EventArgs with the horse that changed</param>
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

        private void HorseFinished(object sender, EventArgs e) {
            bool AllHorsesFinished = true;
            Horse bestHorse = null;

            foreach (Horse horse in _horses) {
                if (horse.Status != Horse.HorseStatus.FINISHED)
                {
                    return;
                }

                if (bestHorse == null || bestHorse.Distance < horse.Distance)
                {
                    bestHorse = horse;
                }
            }

            Debug.WriteLine($"Horse {bestHorse.Name} won!");
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
            horse.HorseFinishedEvent += HorseFinished;
            _horses.Add(horse);
        }

        /// <summary>
        /// Clears all the horses from the game
        /// </summary>
        public void Reset()
        {
            foreach (var horse in _horses)
            {
                horse.Stop();
            }
            _horses.Clear();
        }

        /// <summary>
        /// Restarts the game with the current added horses
        /// </summary>
        public void Restart()
        {
            foreach (var horse in _horses) {
                horse.Stop();
            }
        }

        public ObservableCollection<Horse> Horses => _horses;

    }
}

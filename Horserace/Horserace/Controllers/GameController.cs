using System;
using System.Collections.ObjectModel;
using System.Linq;
using Horserace.Enums;
using Horserace.Events;
using Horserace.Models;
using Horserace.Utlis;

namespace Horserace.Controllers
{
    /// <summary>
    ///     This class keeps track of the main game logic
    /// </summary>
    class GameController
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public GameController()
        {
            Horses = new ObservableCollection<Horse>();
        }

        /// <summary>
        ///     Observable list of horses in order to bind the objects to the GUI
        /// </summary>
        public ObservableCollection<Horse> Horses { get; }


        /// <summary>
        ///     Starts the horse racing game
        /// </summary>
        public void Start(int numberOfPings)
        {
            if (Horses.Count > 0)
            {
                foreach (var horse in Horses)
                {
                    horse.Start(numberOfPings);
                }
            } else
            {
                ToastUtil.Notify("No horses", "Add at least 1 horse to start");
            }
        }

        /// <summary>
        ///     Creates a new horse and adds it to the horse list
        /// </summary>
        /// <param name="name">Name of the horse</param>
        /// <param name="url">The url that will be pinged</param>
        public void AddHorse(string name, int totalPings, string url)
        {
            var horse = new Horse(name, url);
            horse._horseChanged += HorseChanged;
            horse.HorseFinishedEvent += HorseFinished;
            Horses.Add(horse);
        }

        /// <summary>
        ///     Clears all the horses from the game
        /// </summary>
        public void Reset()
        {
            foreach (var horse in Horses)
            {
                horse.Stop();
            }

            Horses.Clear();
        }

        /// <summary>
        ///     Restarts the game with the current added horses
        /// </summary>
        public void Restart()
        {
            foreach (var horse in Horses)
            {
                horse.Stop();
            }
        }

        /// <summary>
        ///     Receives an event whenever the horse distance changes and
        ///     sets the progress bar's maximum based on the horse that has traveled the longest distance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">EventArgs with the horse that changed</param>
        private void HorseChanged(object sender, HorseChangedEventArgs e)
        {
            // Getting the horse with the furthest distance
            var distance = Horses.Max(horse => horse.Distance);

            foreach (Horse horse in Horses)
            {
                horse.FurthestHorseDistance = distance + (distance / 10);
            }
        }

        /// <summary>
        ///     Event gets triggered whenever a horse is finished pinging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HorseFinished(object sender, EventArgs e)
        {
            Horse bestHorse = null;

            foreach (Horse horse in Horses)
            {
                if (horse.Status != HorseStatus.FINISHED)
                {
                    return;
                }

                if (bestHorse == null || bestHorse.Distance < horse.Distance)
                {
                    bestHorse = horse;
                }
            }

            if (bestHorse == null) return;
            ToastUtil.Notify($"{bestHorse.Name} is the winner!", $"With a total distance of: {bestHorse.Distance}");
            MediaUtil.PlaySound("trumpet1.mp3");
        }
    }
}
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Horserace.Controllers;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using Horserace.Utlis;

namespace Horserace
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        private GameController _gameController;

        public HomeView()
        {
            this.InitializeComponent();

            _gameController = new GameController();
            DataContext = _gameController.Horses;
        }
        
        private void Btn_addHorse_OnClick(object sender, RoutedEventArgs e)
        {
            bool canBeSubmitted = true;
            if (string.IsNullOrEmpty(txt_horseName.Text))
            {
                //TODO throw toast
                ToastUtil.Notify("Error", "Invoerveld van naam is leeg");
                Debug.WriteLine($"Empty text");
                canBeSubmitted = false;
            }

            if (string.IsNullOrEmpty(txt_horseUrl.Text))
            {
                //TODO throw toast
                ToastUtil.Notify("Error", "Invoerveld van de url is leeg");

                canBeSubmitted = false;
            }

            if (!canBeSubmitted) return;

            _gameController.AddHorse(txt_horseName.Text, 10, txt_horseUrl.Text);

            // Clearing input fields after adding horse
            txt_horseName.Text = "";
            txt_horseUrl.Text = "";
        }

        private void Btn_clearHorses_OnClick(object sender, RoutedEventArgs e)
        {
            _gameController.Reset();

        }

        private async void Btn_startRace_OnClick(object sender, RoutedEventArgs e)
        {
           _gameController.Start();

            MediaUtil.PlaySound("gun-shot.mp3");
            
            MediaUtil.PlaySound("galopandcrowd.mp3");
        }

        private void Btn_resetGame_OnClick(object sender, RoutedEventArgs e) {
            _gameController.Restart();
        }
    }
}

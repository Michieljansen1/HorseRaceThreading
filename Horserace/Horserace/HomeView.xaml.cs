using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Horserace.Common;
using Horserace.Controllers;
using Horserace.Utlis;

namespace Horserace
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        private GameController _gameController;
        /// <summary>
        /// 
        /// </summary>
        public HomeView()
        {
            this.InitializeComponent();

            _gameController = new GameController();
            DataContext = _gameController.Horses;


            PageLoader pageLoader = new PageLoader();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_addHorse_OnClick(object sender, RoutedEventArgs e)
        {
            var canBeSubmitted = true;
            if (string.IsNullOrEmpty(txt_horseName.Text))
            {
                ToastUtil.Notify("No name", "Name cannot be empty");
                Debug.WriteLine($"Empty text");
                canBeSubmitted = false;
            }

            if (string.IsNullOrEmpty(txt_horseUrl.Text))
            {
                ToastUtil.Notify("No url", "Url cannot be empty");
                canBeSubmitted = false;
            }

            if (!canBeSubmitted) return;

            _gameController.AddHorse(txt_horseName.Text, 10, txt_horseUrl.Text);

            // Clearing input fields after adding horse
            txt_horseName.Text = "";
            txt_horseUrl.Text = "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_clearHorses_OnClick(object sender, RoutedEventArgs e)
        {
            _gameController.Reset();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn_startRace_OnClick(object sender, RoutedEventArgs e)
        {
           _gameController.Start();

            MediaUtil.PlaySound("gun-shot.mp3");
            
            MediaUtil.PlaySound("galopandcrowd.mp3");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_resetGame_OnClick(object sender, RoutedEventArgs e) {
            _gameController.Restart();
        }
    }
}

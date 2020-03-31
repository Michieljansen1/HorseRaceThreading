using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Horserace.Controllers;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

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
        
        private void Btn_addHorse_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: Input validation
            _gameController.AddHorse(txt_horseName.Text, 10, txt_horseUrl.Text);

            // Clearing input fields after adding horse
            txt_horseName.Text = "";
            txt_horseUrl.Text = "";
        }

        private void Btn_clearHorses_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            _gameController.Reset();

        }

        private void Btn_startRace_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
           _gameController.Start();
        }

        private void Btn_restartGame_OnClick_OnClick(object sender, RoutedEventArgs e) {
            _gameController.Restart();
        }
    }
}

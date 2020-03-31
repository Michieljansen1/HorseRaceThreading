using System.Linq;
using Windows.UI.Xaml.Controls;
using Horserace.Controllers;
using Horserace.Models;

namespace Horserace
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        public HomeView()
        {
            this.InitializeComponent();

            GameController gameController = new GameController();

            DataContext = Enumerable.Range(1, 10)
                .Select(x => new Horse("Test horse", 13, "google.com"));
            
        }
    }
}

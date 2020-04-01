using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

namespace Horserace.Utlis
{
    class MediaUtil
    {
        /// <summary>
        /// 
        /// </summary>
        private static MediaPlayer _player = new MediaPlayer();
        public static event EventHandler PlayerFinished;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public static async void PlaySound(string name, bool looped = false)
        {
            _player.MediaEnded += PlayerOnMediaEnded; 

            // MediaElement gunshot = new MediaElement();
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync(name);

            _player.Source = MediaSource.CreateFromStorageFile(file);
            _player.IsLoopingEnabled = looped;

            _player.Play();
        }

        private static void PlayerOnMediaEnded(MediaPlayer sender, object args)
        {
            OnPlayerFinished();
        }

        public static async void Mute()
        {
            _player.Pause();
        }

        private static void OnPlayerFinished()
        {
            EventHandler handler = PlayerFinished;
            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
        }

    }
}

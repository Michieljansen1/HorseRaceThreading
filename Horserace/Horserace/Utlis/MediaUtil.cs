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
        private static MediaPlayer _player;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public static async void PlaySound(string name, bool looped = false)
        {
            _player = new MediaPlayer();

            MediaElement gunshot = new MediaElement();
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync(name);

            _player.Source = MediaSource.CreateFromStorageFile(file);
            _player.IsLoopingEnabled = looped;

            _player.Play();
        }
    }
}

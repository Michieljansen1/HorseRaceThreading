using System;
using Windows.Media.Core;
using Windows.Media.Playback;

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
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync(name);

            _player.Source = MediaSource.CreateFromStorageFile(file);
            _player.IsLoopingEnabled = looped;
            _player.Play();
        }

        public static async void Mute()
        {
            _player.Pause();
        }
    }
}

using System;
using Windows.ApplicationModel;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace Horserace.Utlis
{
    /// <summary>
    ///     Manages the audio for the game
    /// </summary>
    class MediaUtil
    {
        /// <summary>
        ///     Player is used to play the audio
        /// </summary>
        private static readonly MediaPlayer _player = new MediaPlayer();

        /// <summary>
        ///     Loads the given audio file and plays it
        /// </summary>
        /// <param name="name">Filename</param>
        /// <param name="looped">Loop the audio file</param>
        public static async void PlaySound(string name, bool looped = false)
        {
            StorageFolder folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync(name);

            _player.Source = MediaSource.CreateFromStorageFile(file);
            _player.IsLoopingEnabled = looped;
            _player.Play();
        }
    }
}
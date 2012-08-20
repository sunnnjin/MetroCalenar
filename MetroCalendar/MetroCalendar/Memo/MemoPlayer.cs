// <copyright file="MemoPlayer.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Audio;

namespace MetroCalendar.Memo
{
    public delegate void PlaybackCompletedEventHander();

    public class MemoPlayer
    {
        #region event
        public event PlaybackCompletedEventHander PlaybackCompleted;
        #endregion

        #region internal data
        private DynamicSoundEffectInstance _playback;
        private const int SampleRate = 16000;
        private static MemoPlayer _memoPlayer;
        private bool _isPlaying;
        private bool _isPaused;
        #endregion

        #region external methods
        public void Play(string fileName)
        {
            // If another one is playing, stop it
            _playback.Stop();

            // Fetch the waveform data from isolated storage and play it
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = storage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    _playback.SubmitBuffer(buffer);
                }
            }
            _playback.Play();
        }

        public void Resume()
        {
            if (_isPaused)
            {
                _playback.Stop();

                _isPlaying = true;
                _isPaused = false;
            }
        }

        public void Pause()
        {
            if (_isPlaying)
            {
                _playback.Pause();

                _isPlaying = false;
                _isPaused = true;
            }
        }

        public void Stop()
        {
            _playback.Stop();

            _isPlaying = false;
            _isPaused = false;
        }
        #endregion

        #region constructor
        public static MemoPlayer Instance()
        {
            if (_memoPlayer == null)
            {
                _memoPlayer = new MemoPlayer();
            }
            return _memoPlayer;
        }

        public MemoPlayer()
        {
            // Create new DynamicSoundEffectInstace for playback
            _playback = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Mono);
            _playback.BufferNeeded += OnPlaybackBufferNeeded;
        }
        #endregion

        #region internal methods
        private void OnPlaybackBufferNeeded(object sender, EventArgs args)
        {
            // The whole buffer has been submitted for playing, 
            //  so this merely updates the play button if no buffers are pending
            if (_playback.PendingBufferCount == 0)
            {
                NotifyPlaybackCompleted();

                Stop();
            }
        }

        private void NotifyPlaybackCompleted()
        {
            if (PlaybackCompleted != null)
            {
                PlaybackCompleted();
            }
        }
        #endregion
    }
}

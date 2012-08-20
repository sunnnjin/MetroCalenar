// <copyright file="MemoPlayerViewModel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows.Threading;
using MetroCalendar.Memo;

namespace MetroCalendar.ViewModels
{
    public delegate void PlayCompletedEventHandler();

    public class MemoPlayerViewModel : INotifyPropertyChanged
    {
        #region event
        public event PlayCompletedEventHandler PlayCompleted;
        #endregion

        #region internal data
        // Current entry
        private Entry _currentEntry;
        // Create time
        private DateTime _createTime;
        // Recording time text
        private string _recordingTimeText;
        // Memo player
        private MemoPlayer _memoPlayer;
        // Timer for play time
        private DispatcherTimer _playTimer;
        // Play time (u:seconds)
        private int _playTime = 0;
        private int _maxPlayTime = 0;
        private bool _playCompleted;
        private const int SecondsPerMinute = 60;
        #endregion

        #region property
        public string FormattedPlayTimeText
        {
            get
            {
                string formattedPlayTimeText = string.Empty;

                int time = _playTime;
                if (time / SecondsPerMinute > 0)
                {
                    formattedPlayTimeText = string.Format("{0}'", time / SecondsPerMinute);
                    time %= SecondsPerMinute;
                }

                formattedPlayTimeText += ((time > 0) ?
                    string.Format("{0}s", time) : string.Empty);

                formattedPlayTimeText = string.IsNullOrEmpty(formattedPlayTimeText) ? 
                    "0s" : formattedPlayTimeText;

                return formattedPlayTimeText;
            }
        }

        public string RecordingTimeText
        {
            get
            {
                return _recordingTimeText;
            }
            set
            {
                string newRecordingTimeText = (string)value;
                if (_recordingTimeText != newRecordingTimeText)
                {
                    _recordingTimeText = newRecordingTimeText;
                    RaisePropertyChanged("RecordingTimeText");
                }
            }
        }

        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
            set
            {
                DateTime newCreateTime = (DateTime)value;
                if (_createTime != newCreateTime)
                {
                    _createTime = newCreateTime;
                    RaisePropertyChanged("CreateTime");
                }
            }
        }
        #endregion

        #region external methods
        public void Load(Entry entry)
        {
            _currentEntry = entry;
            CreateTime = _currentEntry.CreateTime;
            _playTime = 0;
            _maxPlayTime = Parse(_currentEntry.ExtraInfo);
            _playCompleted = false;
            RecordingTimeText = string.Format("0s / {0}", _currentEntry.ExtraInfo);
        }

        public void Play()
        {
            _memoPlayer.Play(_currentEntry.AttachmentFile);
            _playTimer.Start();
        }

        public void Stop()
        {
            _memoPlayer.Stop();

            _playTime = 0;
            _playTimer.Stop();
        }

        public void Pause()
        {
            _memoPlayer.Pause();
            _playTimer.Stop();
        }

        public void Resume()
        {
            _memoPlayer.Resume();
            _playTimer.Start();
        }

        public void Remove()
        {
            EntryDataContext.Instance.RemoveEntry(_currentEntry.EntryId);
            // Delete the attched file as well
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                storage.DeleteFile(_currentEntry.AttachmentFile);
            }
        }
        #endregion

        #region constructor
        public MemoPlayerViewModel()
        {
            try
            {
                // Create new memo player and set event handler
                _memoPlayer = MemoPlayer.Instance();

                _playTimer = new DispatcherTimer();
                _playTimer.Tick += new EventHandler(OnTimerTick);
                _playTimer.Interval = TimeSpan.FromSeconds(1);

                CreateTime = DateTime.Now;
            }
            catch
            {
                // Remove exception on ui designer
            }
        }
        #endregion

        #region internal methods
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_playCompleted)
            {
                _playTimer.Stop();

                if (PlayCompleted != null)
                {
                    PlayCompleted();
                }
            }
            else
            {
                ++_playTime;
                RecordingTimeText = string.Format("{0} / {1}",
                    FormattedPlayTimeText, _currentEntry.ExtraInfo);
                if (_maxPlayTime == _playTime)
                {
                    _memoPlayer.Stop();
                    _playCompleted = true;
                }
            }
        }

        private int Parse(string text)
        {
            int seconds = 0;
            const int SecondsPerMintue = 60;

            char[] delimiterChars = { '\'', 's' };
            string[] words = text.Split(delimiterChars);

            int offset = 0;
            seconds += text.Contains('\''.ToString()) ? Convert.ToInt32(words[offset++]) * SecondsPerMintue : 0;
            seconds += text.Contains('s'.ToString()) ? Convert.ToInt32(words[offset]) : 0;

            return seconds;
        }
        #endregion

        #region INotifyPropertyChanged member
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

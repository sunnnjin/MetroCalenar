// <copyright file="MemoRecorderViewModel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows.Threading;
using MetroCalendar.Memo;
using MetroCalendar.Resources.Localizations;
using MetroCalendar.Utilities;
using Microsoft.Xna.Framework.Audio;

namespace MetroCalendar.ViewModels
{
    public delegate void RecorderInitializedEventHandler();

    public class MemoRecorderViewModel : INotifyPropertyChanged
    {
        #region event
        public event RecorderInitializedEventHandler RecorderInitialized;
        #endregion

        #region internal data
        // Create time
        private DateTime _createTime;
        // Status text
        private string _statusText;
        // Recording time text
        private string _recordingTimeText;
        // Formatted space
        private string _formattedSpace;
        // XNA objects for record and playback
        private Microphone _microphone;
        // Data context of record button
        private SpaceTime _spaceTime = new SpaceTime();
        // Used for storing captured buffers
        private List<byte[]> _memoBufferCollection = new List<byte[]>();
        // Timer for max recording timeout(1.5 minutes)
        private DispatcherTimer _timer;
        // Start date time
        private DateTime _startTime;

        private DispatcherTimer _statusTimer;
        private int _initTicker;
        private string _loadingStr;
        private string _extraLoadingStr = "";
        private static readonly string[] ExtraLoadingArray = {
                ".", "..", "...", "" };
        #endregion

        #region property
        public string FormattedSpace
        {
            get
            {
                return _formattedSpace;
            }
            set
            {
                string newFormattedSpace = (string)value;
                if (_formattedSpace != newFormattedSpace)
                {
                    _formattedSpace = newFormattedSpace;
                    RaisePropertyChanged("FormattedSpace");
                }
            }
        }

        public string StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                string newStatusText = (string)value;
                if (_statusText != newStatusText)
                {
                    _statusText = newStatusText;
                    RaisePropertyChanged("StatusText");
                }
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

        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    RaisePropertyChanged("StartTime");
                }
            }
        }
        #endregion

        #region external methods
        public void InitRecording()
        {
            // Initialize UI text 
            StatusText = _loadingStr = AppResources.InitializingText;
            RecordingTimeText = "00:00 / 01:30";
            FormattedSpace = "0 K";
        }

        public void StartRecording(DateTime startTime)
        {
            if (_microphone.State == MicrophoneState.Stopped)
            {
                CreateTime = DateTime.Now;
                StartTime = startTime;

                // Clear the collection for storing buffers
                _memoBufferCollection.Clear();

                // Start recording
                _microphone.Start();

                // Start timer
                _timer.Start();
                _statusTimer.Start();
                _initTicker = 0;
            }
        }

        public void StopRecording()
        {
            // Get the last partial buffer
            int sampleSize = _microphone.GetSampleSizeInBytes(_microphone.BufferDuration);
            byte[] extraBuffer = new byte[sampleSize];
            int extraBytes = _microphone.GetData(extraBuffer);

            // Stop recording
            _microphone.Stop();

            // Stop timer
            _timer.Stop();
            _statusTimer.Stop();

            // Create MemoInfo object and add at top of collection
            int totalSize = _memoBufferCollection.Count * sampleSize + extraBytes;
            TimeSpan duration = _microphone.GetSampleDuration(totalSize);
            MemoInfo memoInfo = new MemoInfo(DateTime.UtcNow, totalSize, duration);

            // Save data in isolated storage
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = storage.CreateFile(memoInfo.FileName))
                {
                    // Write buffers from collection
                    foreach (byte[] buffer in _memoBufferCollection)
                        stream.Write(buffer, 0, buffer.Length);

                    // Write partial buffer
                    stream.Write(extraBuffer, 0, extraBytes);
                }
            }

            StoreEntry(memoInfo);
        }
        #endregion

        #region constructor
        public MemoRecorderViewModel()
        {
            try
            {
                // Create new Microphone and set event handler
                _microphone = Microphone.Default;
                _microphone.BufferReady += OnMicrophoneBufferReady;

                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(OnTimerTick);
                _timer.Interval = TimeSpan.FromSeconds(3);

                _statusTimer = new DispatcherTimer();
                _statusTimer.Tick += new EventHandler(OnStatusTimerTick);
                _statusTimer.Interval = TimeSpan.FromMilliseconds(500);

                CreateTime = DateTime.Now;
            }
            catch
            {
                // Remove exception on ui designer
            }
        }
        #endregion

        #region internal methods
        private void OnStatusTimerTick(object sender, EventArgs e)
        {
            ++_initTicker;
            if (_initTicker == 2)
            {
                _loadingStr = AppResources.RecordingText;
                if (RecorderInitialized != null)
                {
                    RecorderInitialized();
                }
            }
            _extraLoadingStr = ExtraLoadingArray[_extraLoadingStr.Length];
            StatusText = _loadingStr + _extraLoadingStr;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            // Get the last partial buffer
            int sampleSize = _microphone.GetSampleSizeInBytes(_microphone.BufferDuration);
            byte[] extraBuffer = new byte[sampleSize];
            int extraBytes = _microphone.GetData(extraBuffer);

            // Create MemoInfo object and add at top of collection
            int totalSize = _memoBufferCollection.Count * sampleSize + extraBytes;
            TimeSpan duration = _microphone.GetSampleDuration(totalSize);
            MemoInfo memoInfo = new MemoInfo(DateTime.UtcNow, totalSize, duration);

            FormattedSpace = memoInfo.SpaceTime.FormattedSpace;
        }

        private void OnMicrophoneBufferReady(object sender, EventArgs args)
        {
            // Get buffer from microphone and add to collection
            byte[] buffer = new byte[_microphone.GetSampleSizeInBytes(_microphone.BufferDuration)];
            int bytesReturned = _microphone.GetData(buffer);
            _memoBufferCollection.Add(buffer);

            TimeSpan timeSpan = DateTime.Now - CreateTime;
            DateTime dateTime = Convert.ToDateTime(timeSpan.ToString());
            string temp = dateTime.ToString("mm:ss");
            RecordingTimeText = temp + " / 01:30";

            // Check for 10-minute recording limit.
            // With the default sample rate of 16000, this is about 19M,
            //      which takes a few seconds to record to isolated storage.
            // Probably don't want to go much higher without saving the
            //      file incrementally, and providing more protection 
            //      against storage problems.
            if (_spaceTime.Time > TimeSpan.FromMinutes(10))
            {
                StopRecording();
            }
        }

        private void StoreEntry(MemoInfo memoInfo)
        {
            TimeSpan timeSpan = DateTime.Now - CreateTime;
            string minStr = 
                timeSpan.Minutes == 0 ? string.Empty : string.Format(@"{0}'", timeSpan.Minutes);
            string secStr =
                timeSpan.Seconds == 0 ? string.Empty : string.Format(@"{0}s", timeSpan.Seconds);

            if (!(string.IsNullOrEmpty(minStr) && string.IsNullOrEmpty(secStr)))
            {
                Entry entry = new Entry
                {
                    EntryId = System.Guid.NewGuid().ToString(),
                    EntryType = EntryType.Memo.ToString(),
                    CreateTime = CreateTime,
                    StartTime = StartTime,
                    ExpirationTime = DateCalculator.LastTime(StartTime),
                    RepeatType = RepeatType.NotRepeated.ToString(),
                    ExtraInfo = minStr + secStr,
                    AlarmOn = false,
                    AlarmTime = DateTime.Now,
                    AttachmentFile = memoInfo.FileName
                };

                EntryDataContext.Instance.EntryTable.InsertOnSubmit(entry);
                EntryDataContext.Instance.SubmitChanges();
            }
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

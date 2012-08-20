// <copyright file="Entry.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using MetroCalendar.Utilities;

namespace MetroCalendar.ViewModels
{
    public enum EntryType
    {
        Memo,
        Alarm,
        Anniversary
    }

    [Table]
    public class Entry : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region internal data
        // Define ID: private field, public property, and database column.
        private string _entryId; // Key column
        private string _entryType;
        private DateTime _createTime;
        private DateTime _startTime;
        private DateTime _expirationTime;
        private string _subject;
        private string _repeatType;
        private string _extraInfo;
        private string _attachmentFile;
        private bool _alarmOn;
        private DateTime _alarmTime;
        private string _ringTone;
        private bool _vibrate;
        #endregion

        #region property
        public Object Self { get { return this; } }

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public string EntryId
        {
            get { return _entryId; }
            set
            {
                if (_entryId != value)
                {
                    NotifyPropertyChanging("EntryId");
                    _entryId = value;
                    NotifyPropertyChanged("EntryId");
                }
            }
        }

        [Column]
        public string EntryType
        {
            get { return _entryType; }
            set
            {
                if (_entryType != value)
                {
                    NotifyPropertyChanging("EntryType");
                    _entryType = value;
                    NotifyPropertyChanged("EntryType");
                }
            }
        }

        [Column]
        public DateTime CreateTime
        {
            get { return _createTime; }
            set
            {
                if (_createTime != value)
                {
                    NotifyPropertyChanging("CreateTime");
                    _createTime = value;
                    NotifyPropertyChanged("CreateTime");
                }
            }
        }

        [Column]
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    NotifyPropertyChanging("StartTime");
                    _startTime = value;
                    NotifyPropertyChanged("StartTime");
                }
            }
        }

        [Column]
        public DateTime ExpirationTime
        {
            get { return DateCalculator.LastTime(_expirationTime); }
            set
            {
                if (_expirationTime != value)
                {
                    NotifyPropertyChanging("ExpirationTime");
                    _expirationTime = value;
                    NotifyPropertyChanged("ExpirationTime");
                }
            }
        }

        [Column]
        public string Subject
        {
            get { return _subject; }
            set
            {
                if (_subject != value)
                {
                    NotifyPropertyChanging("Subject");
                    _subject = value;
                    NotifyPropertyChanged("Subject");
                }
            }
        }

        [Column]
        public string RepeatType
        {
            get { return _repeatType; }
            set
            {
                if (_repeatType != value)
                {
                    NotifyPropertyChanging("RepeatType");
                    _repeatType = value;
                    NotifyPropertyChanged("RepeatType");
                }
            }
        }

        [Column]
        public string ExtraInfo
        {
            get { return _extraInfo; }
            set
            {
                if (_extraInfo != value)
                {
                    NotifyPropertyChanging("ExtraInfo");
                    _extraInfo = value;
                    NotifyPropertyChanged("ExtraInfo");
                }
            }
        }

        [Column]
        public string AttachmentFile
        {
            get { return _attachmentFile; }
            set
            {
                if (_attachmentFile != value)
                {
                    NotifyPropertyChanging("AttachmentFile");
                    _attachmentFile = value;
                    NotifyPropertyChanged("AttachmentFile");
                }
            }
        }

        [Column]
        public bool AlarmOn
        {
            get { return _alarmOn; }
            set
            {
                if (_alarmOn != value)
                {
                    NotifyPropertyChanging("AlarmOn");
                    _alarmOn = value;
                    NotifyPropertyChanged("AlarmOn");
                }
            }
        }

        [Column]
        public DateTime AlarmTime
        {
            get { return _alarmTime; }
            set
            {
                if (_alarmTime != value)
                {
                    NotifyPropertyChanging("AlarmTime");
                    _alarmTime = value;
                    NotifyPropertyChanged("AlarmTime");
                }
            }
        }

        [Column]
        public string RingTone
        {
            get { return _ringTone; }
            set
            {
                if (_ringTone != value)
                {
                    NotifyPropertyChanging("RingTone");
                    _ringTone = value;
                    NotifyPropertyChanged("RingTone");
                }
            }
        }

        [Column]
        public bool Vibrate
        {
            get { return _vibrate; }
            set
            {
                if (_vibrate != value)
                {
                    NotifyPropertyChanging("Vibrate");
                    _vibrate = value;
                    NotifyPropertyChanged("Vibrate");
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                PropertyChanged(this, new PropertyChangedEventArgs("Self"));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        protected void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}

// <copyright file="AlarmViewModel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MetroCalendar.Resources.Localizations;
using MetroCalendar.Utilities;
using Microsoft.Phone.Scheduler;

namespace MetroCalendar.ViewModels
{
    public class AlarmViewModel : INotifyPropertyChanged
    {
        #region internal data
        private ObservableCollection<string> _repeatTypeList;
        private ObservableCollection<string> _ringToneList;
        private Entry _currentEntry;

        private string _subject;
        private RepeatType _repeatType;
        private DateTime _alarmTime;
        private DateTime _expirationTime;
        private bool _vibrate;
        private RingTone _ringTone;
        #endregion

        #region property
        public ObservableCollection<string> RepeatTypeList
        {
            get
            {
                return _repeatTypeList;
            }
        }

        public ObservableCollection<string> RingToneList
        {
            get
            {
                return _ringToneList;
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                string newSubject = (string)value;
                if (_subject != newSubject)
                {
                    _subject = newSubject;
                    RaisePropertyChanged("Subject");
                }
            }
        }

        public RepeatType RepeatType
        {
            get
            {
                return _repeatType;
            }
            set
            {
                RepeatType newRepeatType = (RepeatType)value;
                if (_repeatType != newRepeatType)
                {
                    _repeatType = newRepeatType;
                    RaisePropertyChanged("RepeatType");
                }
            }
        }
		
        public DateTime AlarmTime
        {
            get
            {
                return _alarmTime;
            }
            set
            {
                DateTime newAlarmTime = (DateTime)value;
                if (_alarmTime != newAlarmTime)
                {
                    _alarmTime = newAlarmTime;
                    RaisePropertyChanged("AlarmTime");
                }
            }
        }

        public DateTime ExpirationTime
        {
            get
            {
                return _expirationTime;
            }
            set
            {
                DateTime newExpirationTime = 
                    DateCalculator.LastTime((DateTime)value);
                if (_expirationTime != newExpirationTime)
                {
                    _expirationTime = newExpirationTime;
                    RaisePropertyChanged("ExpirationTime");
                }
            }
        }

        public bool Vibrate
        {
            get
            {
                return _vibrate;
            }
            set
            {
                bool newVibrate = (bool)value;
                if (_vibrate != newVibrate)
                {
                    _vibrate = newVibrate;
                    RaisePropertyChanged("Vibrate");
                }
            }
        }

        public RingTone RingTone
        {
            get
            {
                return _ringTone;
            }
            set
            {
                RingTone newRingTone = (RingTone)value;
                if (_ringTone != newRingTone)
                {
                    _ringTone = newRingTone;
                    RaisePropertyChanged("RingTone");
                }
            }
        }
        #endregion

        #region external method
        public void Store(DateTime startTime)
        {
            Entry entry = new Entry
            {
                EntryId = System.Guid.NewGuid().ToString(),
                EntryType = EntryType.Alarm.ToString(),
                CreateTime = DateTime.Now,
                StartTime = startTime.Date,
                ExpirationTime = this.ExpirationTime,
                Subject = this.Subject,
                RepeatType = this.RepeatType.ToString(),
                ExtraInfo = this.AlarmTime.ToShortTimeString(),
                AlarmOn = true,
                AlarmTime = this.AlarmTime,
                RingTone = this.RingTone.ToString(),
                Vibrate = this.Vibrate
            };

            EntryDataContext.Instance.AddEntry(entry);
            AlarmUtilities.AddAlarm(entry);
        }

        public void Update()
        {
            _currentEntry.Subject = this.Subject;
            _currentEntry.RepeatType = this.RepeatType.ToString();
            _currentEntry.AlarmTime = this.AlarmTime;
            _currentEntry.ExpirationTime = this.ExpirationTime;
            _currentEntry.Vibrate = this.Vibrate;
            _currentEntry.RingTone = this.RingTone.ToString();
            _currentEntry.ExtraInfo = this.AlarmTime.ToShortTimeString();

            EntryDataContext.Instance.UpdateEntry(_currentEntry);
            AlarmUtilities.UpdateAlarm(_currentEntry);
        }

        public void Load(string entryId)
        {
            if (EntryDataContext.Instance.TryGetEntry(entryId, out _currentEntry))
            {
                Subject = _currentEntry.Subject;
                AlarmTime = _currentEntry.AlarmTime;
                ExpirationTime = _currentEntry.ExpirationTime;
                RepeatType = (RepeatType)Enum.Parse(typeof(RepeatType), _currentEntry.RepeatType, true);
                Vibrate = _currentEntry.Vibrate;
                RingTone = (RingTone)Enum.Parse(typeof(RingTone), _currentEntry.RingTone, true);
            }
        }

        public void Remove()
        {
            EntryDataContext.Instance.RemoveEntry(_currentEntry.EntryId);
            AlarmUtilities.RemoveAlarm(_currentEntry.EntryId);
        }
        #endregion

        #region constructor
        public AlarmViewModel()
        {
            _repeatTypeList = new ObservableCollection<string>();
            _repeatTypeList.Add(AppResources.NotRepeatedText);
            _repeatTypeList.Add(AppResources.DailyText);
            _repeatTypeList.Add(AppResources.WeeklyText);
            //_repeatTypeList.Add(AppResources.FortnightlyText);
            _repeatTypeList.Add(AppResources.MonthlyText);
            _repeatTypeList.Add(AppResources.YearlyText);

            _ringToneList = new ObservableCollection<string>();
            _ringToneList.Add("School bell");
            _ringToneList.Add("Chaotic");
            _ringToneList.Add("Ding tone");

            Subject = AppResources.EnterSubjectText;
            RepeatType = RepeatType.NotRepeated;
            AlarmTime = DateTime.Now.AddMinutes(1);
            ExpirationTime = DateCalculator.LastTime(DateTime.Now);
            RingTone = RingTone.SchoolBell;
            Vibrate = true;
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

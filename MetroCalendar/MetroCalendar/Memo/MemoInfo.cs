// <copyright file="MemoInfo.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Windows;

namespace MetroCalendar.Memo
{
    public class MemoInfo : INotifyPropertyChanged
    {
        bool _isPlaying;

        // Event is only fired for IsPlaying property
        public event PropertyChangedEventHandler PropertyChanged;

        public MemoInfo(string filename, long filesize, TimeSpan duration)
        {
            this.FileName = filename;

            this.SpaceTime = new SpaceTime
            {
                Space = filesize,
                Time = duration
            };
        }

        public MemoInfo(DateTime datetime, long filesize, TimeSpan duration) : this(null, filesize, duration)
        {
            // Convert DateTime to packed string
            FileName = String.Format("{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}{6:D3}",
                                     datetime.Year, datetime.Month, datetime.Day, 
                                     datetime.Hour, datetime.Minute, datetime.Second, 
                                     datetime.Millisecond);
        }
 
        public bool IsPlaying 
        {
            set
            {
                if (value != _isPlaying)
                {
                    _isPlaying = value;

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsPlaying"));
                }
            }
            get
            {
                return _isPlaying;
            }
        }

        public bool IsPaused { set; get; }

        public string FileName { protected set; get; }

        public SpaceTime SpaceTime { protected set; get; }

        public DateTime DateTime 
        {
            get
            {
                // Convert packed string to DateTime
                int year = int.Parse(FileName.Substring(0, 4));
                int mon = int.Parse(FileName.Substring(4, 2));
                int day = int.Parse(FileName.Substring(6, 2));
                int hour = int.Parse(FileName.Substring(8, 2));
                int min = int.Parse(FileName.Substring(10, 2));
                int sec = int.Parse(FileName.Substring(12, 2));
                int msec = int.Parse(FileName.Substring(14, 3));

                DateTime dt = new DateTime(year, mon, day, hour, min, sec, msec, DateTimeKind.Utc);
                return dt.ToLocalTime();
            }
        }
    }
}

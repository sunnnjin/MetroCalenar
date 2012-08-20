// <copyright file="CalendarPanelViewModel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using MetroCalendar.Resources.Localizations;

namespace MetroCalendar.ViewModels
{
    public delegate void CurrentDayUpdatedEventHandler(DateTime dateTime);

    public class CalendarPanelViewModel : INotifyPropertyChanged
    {
        #region event
        public event CurrentDayUpdatedEventHandler CurrentDayUpdated;
        #endregion

        #region internal data
        private string _year;
        private string _month;
        private string _day;
        private DateTime _date;
        #endregion

        #region property
        public string Year
        {
            get
            {
                return _year;
            }
            set
            {
                string newYear = (string)value;
                if (_year != newYear)
                {
                    _year = newYear;
                    RaisePropertyChanged("Year");
                }
            }
        }

        public string Month
        {
            get
            {
                return _month;
            }
            set
            {
                string newMonth = (string)value;
                if (_month != newMonth)
                {
                    _month = newMonth;
                    RaisePropertyChanged("Month");
                }
            }
        }

        public string Day
        {
            get
            {
                return _day;
            }
            set
            {
                string newDay = (string)value;
                if (_day != newDay)
                {
                    _day = newDay;
                    RaisePropertyChanged("Day");
                }
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                DateTime newDate = (DateTime)value;
                if (_date != newDate)
                {
                    _date = newDate;
                    RaisePropertyChanged("Date");

                    Year = Date.Year.ToString();
                    Month = LocalMonth(Date.Month);
                    Day = Date.Day.ToString();

                    if (CurrentDayUpdated != null)
                    {
                        CurrentDayUpdated(_date);
                    }
                }
            }
        }
        #endregion

        #region constructor
        public CalendarPanelViewModel()
        {
            Date = DateTime.Now;
        }
        #endregion

        #region internal method
        public string LocalMonth(int month)
        {
            string localMonth = string.Empty;

            switch (month)
            {
                case 1:
                    localMonth = AppResources.JanText;
                    break;
                case 2:
                    localMonth = AppResources.FebText;
                    break;
                case 3:
                    localMonth = AppResources.MarText;
                    break;
                case 4:
                    localMonth = AppResources.AprText;
                    break;
                case 5:
                    localMonth = AppResources.MayText;
                    break;
                case 6:
                    localMonth = AppResources.JunText;
                    break;
                case 7:
                    localMonth = AppResources.JulText;
                    break;
                case 8:
                    localMonth = AppResources.AugText;
                    break;
                case 9:
                    localMonth = AppResources.SepText;
                    break;
                case 10:
                    localMonth = AppResources.OctText;
                    break;
                case 11:
                    localMonth = AppResources.NovText;
                    break;
                case 12:
                    localMonth = AppResources.DecText;
                    break;
                default:
                    break;
            }

            return localMonth;
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

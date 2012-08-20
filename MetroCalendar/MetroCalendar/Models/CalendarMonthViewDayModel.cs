// <copyright file="CalendarMonthViewDayModel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace MetroCalendar.Models
{
    [Flags]
    public enum DayType
    {
        NotCurrentMonth = 0x01,
        Workday         = 0x02,
        Weekend         = 0x04,
        Festival         = 0x08,
        Anniversary     = 0x10,
        Today           = 0x20,
        CurrentDay      = 0x40
    }

    /// <summary>
    /// Encapsulates model for one calendar day in <see cref="CalendarMonthView"/> with support for dependency binding.
    /// </summary>
    public class CalendarMonthViewDayModel : DependencyObject, INotifyPropertyChanged
    {
        #region DayOfMonthProperty
        /// <summary>
        /// Identifies the <see cref="DayOfMonth"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="DayOfMonth"/> dependency property.</returns>
        public static readonly DependencyProperty DayOfMonthProperty = DependencyProperty.Register("DayOfMonth", typeof(int), typeof(CalendarMonthViewDayModel), new PropertyMetadata(0));
        #endregion
        #region IsCurrentMonthProperty
        /// <summary>
        /// Identifies the <see cref="IsCurrentMonth"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="IsCurrentMonth"/> dependency property.</returns>
        public static readonly DependencyProperty IsCurrentMonthProperty = DependencyProperty.Register("IsCurrentMonth", typeof(bool), typeof(CalendarMonthViewDayModel), new PropertyMetadata(false));
        #endregion
        #region IsTodayProperty
        /// <summary>
        /// Identifies the <see cref="IsToday"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="IsToday"/> dependency property.</returns>
        public static readonly DependencyProperty IsTodayProperty = DependencyProperty.Register("IsToday", typeof(bool), typeof(CalendarMonthViewDayModel), new PropertyMetadata(false));
        #endregion
        #region MonthPositionProperty
        /// <summary>
        /// Identifies the <see cref="MonthPosition"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="MonthPosition"/> dependency property.</returns>
        public static readonly DependencyProperty MonthPositionProperty = DependencyProperty.Register("MonthPosition", typeof(DayInMonthViewPosition), typeof(CalendarMonthViewDayModel), new PropertyMetadata(null));
        #endregion
        #region DayTypeProperty
        /// <summary>
        /// Identifies the <see cref="DayType"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="DayType"/> dependency property.</returns>
        public static readonly DependencyProperty DayTypeProperty = DependencyProperty.Register("DayType", typeof(DayType), typeof(CalendarMonthViewDayModel), new PropertyMetadata(null));
        #endregion
        #region DateTimeProperty
        /// <summary>
        /// Identifies the <see cref="DateTime"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="DateTime"/> dependency property.</returns>
        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register("DateTime", typeof(DateTime), typeof(CalendarMonthViewDayModel), new PropertyMetadata(null));
        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        #region DayOfMonth
        /// <summary>
        /// Gets or sets the day of month.
        /// </summary>
        /// <value>The day of month.</value>
        public int DayOfMonth
        {
            get { return (int)GetValue(DayOfMonthProperty); }
            set
            {
                NotifyPropertyChanged("DayOfMonth");
                SetValue(DayOfMonthProperty, value);
            }
        }
        #endregion
        #region IsToday
        /// <summary>
        /// Gets or sets a value indicating whether this day control should be displayed as today.
        /// </summary>
        /// <value><c>true</c> if this day control should be displayed as; otherwise, <c>false</c>.</value>
        public bool IsToday
        {
            get { return (bool)GetValue(IsTodayProperty); }
            set
            {
                NotifyPropertyChanged("IsToday");
                SetValue(IsTodayProperty, value);
            }
        }
        #endregion
        #region IsCurrentMonth
        /// <summary>
        /// Gets or sets a value indicating whether this day control should be displayed as day is current month.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this day control should be displayed as day is current month; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrentMonth
        {
            get { return (bool)GetValue(IsCurrentMonthProperty); }
            set
            {
                NotifyPropertyChanged("IsCurrentMonth");
                SetValue(IsCurrentMonthProperty, value);
            }
        }
        #endregion
        #region Subjects
        /// <summary>
        /// Gets or sets the subjects.
        /// </summary>
        /// <value>The subjects.</value>
        public ObservableCollection<CalendarItemModel> CalendarItems { get; private set; }
        #endregion                   
        #region MonthPosition

        /// <summary>
        /// Gets or sets the month position.
        /// </summary>
        /// <value>The month position.</value>
        public DayInMonthViewPosition MonthPosition
        {
            get { return (DayInMonthViewPosition)GetValue(MonthPositionProperty); }
            set
            {
                NotifyPropertyChanged("MonthPosition");
                SetValue(MonthPositionProperty, value);
            }
        }
        #endregion

        #region DayType
        /// <summary>
        /// Gets or sets the day type.
        /// </summary>
        /// <value>The day type.</value>
        public DayType DayType
        {
            get { return (DayType)GetValue(DayTypeProperty); }
            set
            {
                NotifyPropertyChanged("DayType");
                SetValue(DayTypeProperty, value);
            }
        }
        #endregion

        #region DateTime
        /// <summary>
        /// Gets or sets the DateTime of day.
        /// </summary>
        /// <value>The day type.</value>
        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set
            {
                NotifyPropertyChanged("DateTime");
                SetValue(DateTimeProperty, value);
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarMonthViewDayModel"/> class.
        /// </summary>
        public CalendarMonthViewDayModel()
        {
            CalendarItems = new ObservableCollection<CalendarItemModel>();
        }
    }
}

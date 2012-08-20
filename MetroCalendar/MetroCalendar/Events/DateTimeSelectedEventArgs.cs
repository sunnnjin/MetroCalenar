// <copyright file="DateTimeSelectedEventArgs.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;

namespace MetroCalendar.Events
{
    /// <summary>
    /// Provides wrapper class for <see cref="CalendarMonthView.DaySelected"/> event.
    /// </summary>
    public class DateTimeSelectedEventArgs : EventArgs
    {
        #region SelectedDate
        /// <summary>
        /// Gets the selected date.
        /// </summary>
        /// <value>The selected date.</value>
        public DateTime SelectedDate { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        public DateTimeSelectedEventArgs(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
        }
    }
}

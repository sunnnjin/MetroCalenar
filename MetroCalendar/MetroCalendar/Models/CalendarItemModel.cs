// <copyright file="CalendarItemModel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Windows;

namespace MetroCalendar.Models
{
    /// <summary>
    /// Encapsulates model for one calendar item with support for dependency binding. 
    /// Derive from this class if you would like to extend by custom properties.
    /// </summary>
    public class CalendarItemModel : DependencyObject, IComparable<CalendarItemModel>, INotifyPropertyChanged
    {
        #region SubjectProperty
        /// <summary>
        /// Identifies the <see cref="Subject"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="Subject"/> dependency property.</returns>
        public static readonly DependencyProperty SubjectProperty = DependencyProperty.Register("Subject", typeof(string), typeof(CalendarItemModel), new PropertyMetadata(null));
        #endregion
        #region DurationProperty
        /// <summary>
        /// Identifies the <see cref="Duration"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="Duration"/> dependency property.</returns>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(CalendarItemModel), new PropertyMetadata(TimeSpan.MinValue));
        #endregion
        #region StartDate
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate { get; private set; }
        #endregion
        #region Duration

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set
            {
                NotifyPropertyChanged("Duration");
                SetValue(DurationProperty, value);
            }
        }
        #endregion
        #region Subject

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject
        {
            get { return (string)GetValue(SubjectProperty); }
            set
            {
                NotifyPropertyChanged("Subject");
                SetValue(SubjectProperty, value);
            }
        }
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
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemModel"/> class.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="duration">The time span.</param>
        public CalendarItemModel(DateTime startDate, string subject, TimeSpan duration)
        {
            StartDate = startDate;
            Subject = subject;
            Duration = duration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemModel"/> class.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="subject">The subject.</param>
        public CalendarItemModel(DateTime startDate, string subject)
            : this(startDate, subject, TimeSpan.Zero)
        { }

        #region IComparable<CalendarItem> Members
         
        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other"> An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects
        /// being compared. The return value has the following meanings: Value Meaning
        /// Less than zero This object is less than the other parameter.Zero This object
        /// is equal to other. Greater than zero This object is greater than other.</returns>
        public int CompareTo(CalendarItemModel other)
        {
            return StartDate.CompareTo(other.StartDate);
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            CalendarItemModel model = obj as CalendarItemModel;

            if (null == model)
                return base.Equals(obj);
            else
                return StartDate.Equals(model.StartDate) && Duration.Equals(model.Duration) &&
                    Subject == model.Subject;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (int)((long)base.GetHashCode() ^ StartDate.Ticks ^ Duration.Ticks);
        }
    }
}

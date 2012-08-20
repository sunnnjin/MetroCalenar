// <copyright file="DateTimeToLongDateStringConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroCalendar.Converters
{
    public class DateTimeToLongDateStringConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string shortDateString = ((DateTime)value).ToShortDateString();
            string longTimeString = ((DateTime)value).ToLongTimeString();

            return shortDateString + " " + longTimeString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

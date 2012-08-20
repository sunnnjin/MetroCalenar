// <copyright file="LowercaseMonthNameConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MetroCalendar.Converters
{
    public class LowercaseMonthNameConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Format(CultureInfo.CurrentCulture, "{0:MMMM}", value).ToLower(CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {      
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}

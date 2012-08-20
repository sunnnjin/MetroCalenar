// <copyright file="CalendarMonthViewTitleVisibilityConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MetroCalendar.Converters
{
    public class CalendarMonthViewTitleVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool inverse;
            if (!Boolean.TryParse((string)parameter, out inverse))
                inverse = false;

            return (((bool)value && !inverse) || (!(bool)value && inverse)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}

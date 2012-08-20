// <copyright file="CalendarMonthViewBorderDayConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MetroCalendar.Models;

namespace MetroCalendar.Converters
{
    public class CalendarMonthViewBorderDayConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DayInMonthViewPosition model = value as DayInMonthViewPosition;

            return new Thickness(model.Column == 0 && !model.IsPortrait ? 1 : 0, model.Row == 0 ? 
                1 : 0, (model.Column == CultureInfo.InvariantCulture.DateTimeFormat.DayNames.Length - 1 && model.IsPortrait) ? 0 : 1, 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}

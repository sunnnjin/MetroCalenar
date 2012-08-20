// <copyright file="CalendarDayTypeBackgroundConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MetroCalendar.Models;

namespace MetroCalendar.Converters
{
    public class CalendarDayTypeBackgroundConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = null;

            DayType type = (DayType)value;
            if (HasCurrentDayFlag(type))
            {
                type &= (~DayType.CurrentDay);
            }

            switch (type)
            {
                case DayType.NotCurrentMonth:
                    brush = new SolidColorBrush(Colors.White);
                    break;
                case DayType.Workday:
                    brush = new SolidColorBrush(Color.FromArgb(255, 148, 0, 76));
                    break;
                case DayType.Weekend:
                    brush = new SolidColorBrush(Color.FromArgb(255, 173, 133, 0));
                    break;
                case DayType.Festival:
                    brush = new SolidColorBrush(Color.FromArgb(255, 222, 20, 24));
                    break;
                case DayType.Anniversary:
                    brush = new SolidColorBrush(Color.FromArgb(255, 56, 121, 198));
                    break;
                case DayType.Today:
                    brush = new SolidColorBrush(Color.FromArgb(255, 37, 131, 15));
                    break;
                case DayType.CurrentDay: // Current day doesn't have background.
                default:
                    break;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public static bool HasCurrentDayFlag(DayType type)
        {
            return (type & DayType.CurrentDay) > 0;
        }
        #endregion
    }
}

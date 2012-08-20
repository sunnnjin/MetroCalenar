// <copyright file="CalendarDayTypeForegroundConverter.cs" author="Sun Jinbo">
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
    public class CalendarDayTypeForegroundConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = null;

            DayType type = (DayType)value;
            if (HasCurrentDayFlag(type))
            {
                brush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 140));
            }
            else
            {
                switch ((DayType)value)
                {
                    case DayType.NotCurrentMonth:
                        brush = new SolidColorBrush(Colors.LightGray);
                        break;
                    case DayType.Workday:
                        brush = new SolidColorBrush(Colors.White);
                        break;
                    case DayType.Weekend:
                        brush = new SolidColorBrush(Colors.LightGray);
                        break;
                    case DayType.Festival:
                        brush = new SolidColorBrush(Colors.Black);
                        break;
                    case DayType.Anniversary:
                        brush = new SolidColorBrush(Colors.White);
                        break;
                    case DayType.Today:
                        brush = new SolidColorBrush(Colors.White);
                        break;
                    default:
                        break;
                }
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

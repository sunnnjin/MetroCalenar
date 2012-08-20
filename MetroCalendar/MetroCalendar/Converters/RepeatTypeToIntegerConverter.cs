// <copyright file="RepeatTypeToIntegerConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroCalendar.Converters
{
    public class RepeatTypeToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int integer = -1;

            if (value is RepeatType)
            {
                integer = (int)value;
            }

            return integer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RepeatType repeatType = RepeatType.NotRepeated;

            if (value is int)
            {
                switch ((int)value)
                {
                    case 0:
                        repeatType = RepeatType.NotRepeated;
                        break;
                    case 1:
                        repeatType = RepeatType.Daily;
                        break;
                    case 2:
                        repeatType = RepeatType.Weekly;
                        break;
                    case 3:
                        repeatType = RepeatType.Monthly;
                        break;
                    case 4:
                        repeatType = RepeatType.Yearly;
                        break;
                    default:
                        break;
                }
            }
            return repeatType;
        }
    }
}

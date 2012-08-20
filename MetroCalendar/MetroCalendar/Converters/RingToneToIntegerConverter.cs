// <copyright file="RingToneToIntegerConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroCalendar.Converters
{
    public class RingToneToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int integer = -1;

            if (value is RingTone)
            {
                integer = (int)value;
            }

            return integer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RingTone ringTone = RingTone.SchoolBell;

            if (value is int)
            {
                ringTone = (RingTone)value;
            }

            return ringTone;
        }
    }
}

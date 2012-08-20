// <copyright file="YearlyRepeatToBooleanConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroCalendar.Converters
{
    public class YearlyRepeatToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = false;

            if (value is RepeatType)
            {
                if ((RepeatType)value == RepeatType.Yearly)
                {
                    isChecked = true;
                }
            }

            return isChecked;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RepeatType repeatType = RepeatType.NotRepeated;

            if ((bool)value)
            {
                repeatType = RepeatType.Yearly;
            }

            return repeatType;
        }
    }
}

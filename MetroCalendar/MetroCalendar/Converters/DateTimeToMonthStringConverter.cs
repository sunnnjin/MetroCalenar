// <copyright file="DateTimeToMonthStringConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;
using MetroCalendar.Resources.Localizations;

namespace MetroCalendar.Converters
{
    public class DateTimeToMonthStringConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string monthString = string.Empty;

            switch(((DateTime)value).Month)
            {
                case 1: // Jan
                    monthString = AppResources.JanText;
                    break;
                case 2: // Feb
                    monthString = AppResources.FebText;
                    break;
                case 3: // Mar
                    monthString = AppResources.MarText;
                    break;
                case 4: // Apr
                    monthString = AppResources.AprText;
                    break;
                case 5: // May
                    monthString = AppResources.MayText;
                    break;
                case 6: // Jun
                    monthString = AppResources.JunText;
                    break;
                case 7: // Jul
                    monthString = AppResources.JulText;
                    break;
                case 8: // Aug
                    monthString = AppResources.AugText;
                    break;
                case 9: // Sep
                    monthString = AppResources.SepText;
                    break;
                case 10: // Nov
                    monthString = AppResources.NovText;
                    break;
                case 11: // Oct
                    monthString = AppResources.OctText;
                    break;
                case 12: // Dec
                    monthString = AppResources.DecText;
                    break;
                default:
                    break;
            }

            return monthString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

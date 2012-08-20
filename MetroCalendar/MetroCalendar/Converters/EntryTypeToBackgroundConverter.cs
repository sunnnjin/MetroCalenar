// <copyright file="EntryTypeToBackgroundConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MetroCalendar.ViewModels;

namespace MetroCalendar.Converters
{
    public class EntryTypeToBackgroundConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = null;

            EntryType type = (EntryType)Enum.Parse(typeof(EntryType), (string)value, true);

            switch (type)
            {
                case EntryType.Memo:
                    brush = new SolidColorBrush(Color.FromArgb(255, 0, 177, 243));
                    break;
                case EntryType.Alarm:
                    brush = new SolidColorBrush(Color.FromArgb(255, 37, 131, 15));
                    break;
                case EntryType.Anniversary:
                    brush = new SolidColorBrush(Colors.Black);
                    break;
                default:
                    break;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

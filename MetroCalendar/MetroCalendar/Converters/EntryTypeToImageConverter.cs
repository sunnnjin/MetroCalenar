// <copyright file="EntryTypeToImageConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;
using MetroCalendar.ViewModels;

namespace MetroCalendar.Converters
{
    public class EntryTypeToImageConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string image = string.Empty;

            EntryType type = (EntryType)Enum.Parse(typeof(EntryType), (string)value, true);

            switch (type)
            {
                case EntryType.Memo:
                    image = "/Resources/Images/Entry/Memo.png";
                    break;
                case EntryType.Alarm:
                    image = "/Resources/Images/Entry/Alarm.png";
                    break;
                case EntryType.Anniversary:
                    image = "/Resources/Images/Entry/Anniversary.png";
                    break;
                default:
                    break;
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

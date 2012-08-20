// <copyright file="EntryToExtraImageConverter.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;
using MetroCalendar.ViewModels;

namespace MetroCalendar.Converters
{
    public class EntryToExtraImageConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string image = string.Empty;

            Entry entry = (Entry)value;

            EntryType type = (EntryType)Enum.Parse(typeof(EntryType), entry.EntryType, true);

            switch (type)
            {
                case EntryType.Memo:
                    image = "/Resources/Images/Entry/ExtraMemo.png";
                    break;
                case EntryType.Alarm:
                    image = "/Resources/Images/Entry/ExtraAlarm.png";
                    break;
                case EntryType.Anniversary:
                    if (entry.AlarmOn)
                    {
                        image = "/Resources/Images/Entry/ExtraAnniversary_alarm.png";
                    }
                    else
                    {
                        image = "/Resources/Images/Entry/ExtraAnniversary.png";
                    }
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

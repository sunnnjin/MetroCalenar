// <copyright file="VibrateUtilities.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using Microsoft.Devices;

namespace MetroCalendar.Utilities
{
    public enum VibrateType
    {
        ShortVibrate,
        LongVibrate
    }

    public class VibrateUtilities
    {
        private static VibrateUtilities _vibrateUtilities;
        private VibrateController vc;
        
        public bool IsVibrate{ get;set; }

        public void Vibrate(VibrateType type)
        {
            if (IsVibrate) return;

            switch (type)
            {
                case VibrateType.ShortVibrate:
                    vc.Start(TimeSpan.FromMilliseconds(80));
                    break;
                case VibrateType.LongVibrate:
                    vc.Start(TimeSpan.FromMilliseconds(300));
                    break;
                default:
                    break;
            }
        }

        public static VibrateUtilities Instance()
        {
            if (_vibrateUtilities == null)
            {
                _vibrateUtilities = new VibrateUtilities();
            }

            return _vibrateUtilities;
        }

        private VibrateUtilities()
        {
            IsVibrate = false;
            vc = VibrateController.Default;
        }
    }
}

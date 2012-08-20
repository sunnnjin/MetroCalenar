// <copyright file="LocalizedStrings.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using MetroCalendar.Resources.Localizations;

namespace MetroCalendar.Utilities
{
    public class LocalizedStrings
    {
        #region internal data
        private static AppResources localizedResources = new AppResources();
        #endregion

        #region property
        public AppResources LocalizedResources
        {
            get
            {
                return localizedResources;
            }
        }
        #endregion
    }
}

// <copyright file="TileUtilities.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Linq;
using MetroCalendar.Resources.Localizations;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;

namespace MetroCalendar.Utilities
{
    public static class TileUtilities
    {
        // Set all the properties of the Application Tile.
        public static void Update()
        {
            // Application Tile is always the first Tile, even if it is not pinned to Start.
            ShellTile TileToFind = ShellTile.ActiveTiles.First();

            if (TileToFind != null)
            {
                // Create the tile object and set some initial properties for the tile.
                // The Count value of 12 will show the number 12 on the front of the Tile. 
                // Valid values are 1-99.
                // A Count value of 0 will indicate that the Count should not be displayed.
                int alarmCount = AlarmUtilities.AlarmCount();
                Alarm alarm = AlarmUtilities.NearestAlarm();

                StandardTileData NewTileData = new StandardTileData
                {
                    BackgroundImage = new Uri("/Resources/Images/Tiles/BackgroundImage.png", UriKind.Relative),
                    Title = "Metro Calendar",
                    Count = alarmCount,
                    BackTitle = ((alarm == null) ? AppResources.NextAlarmText : alarm.BeginTime.ToShortTimeString()),
                    BackContent = ((alarm == null) ? AppResources.NoAlarmText : alarm.Content),
                    BackBackgroundImage = new Uri("/Resources/Images/Tiles/BackBackgroundImage.png", UriKind.Relative)
                };

                TileToFind.Update(NewTileData);
            }
        }
    }
}

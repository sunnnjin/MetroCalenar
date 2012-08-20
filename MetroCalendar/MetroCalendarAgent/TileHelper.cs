// <copyright file="TileHelper.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;

namespace MetroCalendarAgent
{
    public static class TileHelper
    {
        public static void Update()
        {
            try
            {
                // Application Tile is always the first Tile, even if it is not pinned to Start.
                ShellTile TileToFind = ShellTile.ActiveTiles.First();

                if (TileToFind != null)
                {
                    // Create the tile object and set some initial properties for the tile.
                    // The Count value of 12 will show the number 12 on the front of the Tile. 
                    // Valid values are 1-99.
                    // A Count value of 0 will indicate that the Count should not be displayed.
                    int alarmCount = AlarmHelper.AlarmCount();
                    Alarm alarm = AlarmHelper.NearestAlarm();

                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("/Resources/Images/Tiles/BackgroundImage.png", UriKind.Relative),
                        Title = "Metro Calendar",
                        Count = alarmCount,
                        BackTitle = ((alarm == null) ? "Next alarm" : alarm.BeginTime.ToShortTimeString()),
                        BackContent = ((alarm == null) ? "No alarm" : alarm.Content),
                        BackBackgroundImage = new Uri("/Resources/Images/Tiles/BackBackgroundImage.png", UriKind.Relative)
                    };

                    TileToFind.Update(NewTileData);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}

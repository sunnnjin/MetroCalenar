// <copyright file="DayInMonthViewPosition.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

namespace MetroCalendar.Models
{
    public class DayInMonthViewPosition
    {
        #region Row
        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        /// <value>The row.</value>
        public int Row { get; private set; }
#endregion
        #region Column
        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        public int Column { get; private set; }
        #endregion
        #region IsPortrait
        /// <summary>
        /// Gets or sets a value indicating whether control is in portrait mode.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this is in portrait mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsPortrait { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DayInMonthViewPosition"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <param name="isPortrait">if set to <c>true</c> control is in portrait mode.</param>
        public DayInMonthViewPosition(int row, int column, bool isPortrait)
        {
            Row = row;
            Column = column;
            IsPortrait = isPortrait;
        }
    }
}

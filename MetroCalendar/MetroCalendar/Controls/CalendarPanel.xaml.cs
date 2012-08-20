// <copyright file="CalendarPanel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System.Windows.Controls;
using System.Windows.Input;
using MetroCalendar.Utilities;
using MetroCalendar.ViewModels;

namespace MetroCalendar.Controls
{
    public partial class CalendarPanel : UserControl
    {
        private CalendarPanelViewModel _calendarPanelViewModel;

        public CalendarPanel()
        {
            InitializeComponent();

            _calendarPanelViewModel = new CalendarPanelViewModel();
            this.DataContext = _calendarPanelViewModel;
        }

        private void OnDatePickerTap(object sender, GestureEventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.CalendarPanelStart);
        }
    }
}

// <copyright file="MetroCalendar.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MetroCalendar.Events;
using MetroCalendar.Models;
using MetroCalendar.Utilities;
using MetroCalendar.ViewModels;
using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace MetroCalendar.Controls
{
    public delegate void MonthViewVisibleRowUpdatedEventHandler(int row);
    public delegate void MonthViewCurrentDayUpdatedEventHandler(DateTime dateTime);

    public partial class CalendarMonthView : UserControl, INotifyPropertyChanged
    {
        #region event
        public event MonthViewVisibleRowUpdatedEventHandler MonthViewVisibleRowUpdated;
        public event MonthViewCurrentDayUpdatedEventHandler MonthViewCurrentDayUpdated;
        #endregion

        #region internal data
        private const float FlickVelocity = 500;
        private CalendarMonthViewDayModel _currentDayModel;
        #endregion

        #region DateProperty
        /// <summary>
        /// Identifies the <see cref="Date"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="Date"/> dependency property.</returns>
        public static readonly DependencyProperty DateProperty = 
            DependencyProperty.Register(
                "Date", 
                typeof(DateTime), 
                typeof(CalendarMonthView), 
                new PropertyMetadata(
                    DateTime.MinValue,
                    new PropertyChangedCallback(OnDateChanged)));
        #endregion
        #region ItemsProperty
        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="Items"/> dependency property.</returns>
        public static readonly DependencyProperty ItemsProperty = 
            DependencyProperty.Register("Items", 
            typeof(IEnumerable<CalendarItemModel>), 
            typeof(CalendarMonthView), 
            new PropertyMetadata(null, new PropertyChangedCallback(OnItemsChanged)));
        #endregion        
        #region IsInPortraitProperty
        /// <summary>
        /// Identifies the <see cref="IsInPortrait"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="IsInPortrait"/> dependency property.</returns>
        public static readonly DependencyProperty IsInPortraitProperty = DependencyProperty.Register("IsInPortrait", typeof(bool), typeof(CalendarMonthView), new PropertyMetadata(true));
        #endregion
        #region DayItemTemplateProperty
        /// <summary>
        /// Identifies the <see cref="DayItemTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for <see cref="DayItemTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty DayItemTemplateProperty = 
                            DependencyProperty.Register("DayItemTemplate", 
                            typeof(ControlTemplate), 
                            typeof(CalendarMonthView), 
                            new PropertyMetadata(null));
        #endregion

        #region Date
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get { return (DateTime)this.GetValue(DateProperty); }
            set
            {
                if (Date.Date != value.Date)
                {
                    this.SetValue(DateProperty, value);
                    NotifyPropertyChanged("Date");
                } 
            }
        }
        #endregion
        #region Items

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IEnumerable<CalendarItemModel> Items
        {
            get { return (IEnumerable<CalendarItemModel>)GetValue(ItemsProperty); }
            set
            {
                NotifyPropertyChanged("Items");
                UpdateItemsCollectionObservers(Items, value);

                SetValue(ItemsProperty, value);
            }
        }

        #endregion
        #region Orientation
        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        private static PageOrientation Orientation
        {
            get
            {
                PhoneApplicationFrame applicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                return null == applicationFrame ? PageOrientation.Portrait : applicationFrame.Orientation;
            }
        }
        #endregion        
        #region IsInPortrait

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="CalendarMonthView"/> is in portrait mode.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if <see cref="CalendarMonthView"/> is in portrait mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsInPortrait
        {
            get { return (bool)GetValue(IsInPortraitProperty); }
            set
            {
                NotifyPropertyChanged("IsInPortrait");
                SetValue(IsInPortraitProperty, value);

                foreach (ContentControl control in _DayControls.Values)
                {
                    CalendarMonthViewDayModel model = control.DataContext as CalendarMonthViewDayModel;
                    model.MonthPosition = new DayInMonthViewPosition(model.MonthPosition.Row, model.MonthPosition.Column, value);
                }
            }
        }
        #endregion
        #region DayItemTemplate
        /// <summary>
        /// Gets or sets the day item style displayed in month view.
        /// </summary>
        /// <value>The day item style.</value>
        public ControlTemplate DayItemTemplate
        {
            get
            {
                return (ControlTemplate)(GetValue(DayItemTemplateProperty));
            }
            set
            {
                SetValue(DayItemTemplateProperty, value);
                NotifyPropertyChanged("DayItemTemplate");
            }
        }
        #endregion

        #region Property
        public int VisibleRow { get; set; }

        public DateTime CurrentDay
        {
            get
            {
                return _currentDayModel != null ? 
                    _currentDayModel.DateTime : DateTime.Now;
            }
            set
            {
                if (_currentDayModel != null)
                {
                    _currentDayModel.DayType &= (~DayType.CurrentDay);
                }

                _currentDayModel = GetDayModel((DateTime)value);
                //Debug.Assert(_currentDayModel == null);
                if (_currentDayModel != null)
                {
                    _currentDayModel.DayType = _currentDayModel.DayType | DayType.CurrentDay;
                    NotifyMonthViewCurrentDayUpdated(_currentDayModel.DateTime);
                }
            }
        }
        #endregion

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when one day is selected from month view.
        /// </summary>
        public event EventHandler<DateTimeSelectedEventArgs> DaySelected;

        /// <summary>
        /// Occurs when user changes month or year.
        /// </summary>
        public event EventHandler<DateTimeSelectedEventArgs> MonthYearChanged;    
        private Dictionary<DateTime, ContentControl> _DayControls = 
            new Dictionary<DateTime, ContentControl>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarMonthView"/> class.
        /// </summary>
        public CalendarMonthView()
        {
            InitializeComponent();
            
            DayItemTemplate = (ControlTemplate)Resources["DayItemTemplate"];

            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 7; column++)
                {
                    CalendarMonthViewDayModel model = new CalendarMonthViewDayModel { MonthPosition = new DayInMonthViewPosition(row, column, IsInPortrait) };
                    ContentControl dayControl = new ContentControl { DataContext = model };

                    dayControl.SetBinding(ContentControl.TemplateProperty, new Binding("DayItemTemplate") { Source = this });
                    dayControl.Tap += new EventHandler<GestureEventArgs>(DayControl_Tap);
                    
                    Grid.SetRow(dayControl, row);
                    Grid.SetColumn(dayControl, column);
                    CalendarGrid.Children.Add(dayControl);
                }
            }

            SetValue(DateProperty, new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
            UpdateDayNames();
            CalendarMonthViewDayModel todayModel = GetTodayModel();
            if (todayModel != null)
            {
                CurrentDay = todayModel.DateTime;
            }
        }

        private void DayControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            KeyValuePair<DateTime, ContentControl> item = _DayControls.FirstOrDefault(x => x.Value == (ContentControl)sender);

            if (null != DaySelected)
                DaySelected(this, new DateTimeSelectedEventArgs(item.Key));

            DateTime dt = ((sender as ContentControl).DataContext as CalendarMonthViewDayModel).DateTime;
            if (dt.Year != Date.Year ||
                dt.Month != Date.Month)
            {
                Date = new DateTime(dt.Year, dt.Month, 1);
            }

            CurrentDay = dt;
        }

        private CalendarMonthViewDayModel GetDayModel(DateTime dateTime)
        {
            CalendarMonthViewDayModel dayModel = null;

            foreach (KeyValuePair<DateTime, ContentControl> control in _DayControls)
            {
                CalendarMonthViewDayModel model = 
                    control.Value.DataContext as CalendarMonthViewDayModel;
                if (model.DateTime.Year == dateTime.Year &&
                    model.DateTime.Month == dateTime.Month &&
                    model.DateTime.Day == dateTime.Day)
                {
                    dayModel = model;
                    break;
                }
            }

            return dayModel;
        }

        private CalendarMonthViewDayModel GetTodayModel()
        {
            CalendarMonthViewDayModel dayModel = null;

            foreach (KeyValuePair<DateTime, ContentControl> control in _DayControls)
            {
                CalendarMonthViewDayModel model =
                    control.Value.DataContext as CalendarMonthViewDayModel;
                if (model.IsToday)
                {
                    dayModel = model;
                    break;
                }
            }

            return dayModel;
        }

        private static void OnDateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CalendarMonthView userControl = (CalendarMonthView)obj;

            userControl.UpdateView();
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            List<DateTime> updateDates = new List<DateTime>();
            List<CalendarItemModel> itemsList = new List<CalendarItemModel>();

            if (null != e.NewItems)
                itemsList.AddRange(e.NewItems.Cast<CalendarItemModel>());
            if (null != e.OldItems)
                itemsList.AddRange(e.OldItems.Cast<CalendarItemModel>());

            updateDates.AddRange((from i in itemsList select i.StartDate.Date).Distinct());
            
            foreach (DateTime date in updateDates)
            {
                if (_DayControls.ContainsKey(date))
                    UpdateDayControlSubjects(date);
            }
        }

        private static void OnItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CalendarMonthView userControl = (CalendarMonthView)obj;

            userControl.UpdateItemsCollectionObservers(args.OldValue as IEnumerable<CalendarItemModel>, args.NewValue as IEnumerable<CalendarItemModel>);
        }

        private void UpdateItemsCollectionObservers(IEnumerable<CalendarItemModel> oldValue, IEnumerable<CalendarItemModel> newValue)
        {
            ObservableCollection<CalendarItemModel> observable = oldValue as ObservableCollection<CalendarItemModel>;

            if (null != observable)
                observable.CollectionChanged -= new NotifyCollectionChangedEventHandler(Items_CollectionChanged);

            observable = newValue as ObservableCollection<CalendarItemModel>;

            if (null != observable)
                observable.CollectionChanged += new NotifyCollectionChangedEventHandler(Items_CollectionChanged);

            UpdateView();
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UpdateDayNames()
        {
            string[] dayOfWeek = { "S", "M", "T", "W", "T", "F", "S" }; 

            DayNamesGrid.Children.Clear();
            DayOfWeek currentDay = DayOfWeek.Monday;
            for (int i = 0; i < 7; i++)
            {
                Border border = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.White),
                    Width = 68,
                    Height = 68,
                    BorderThickness = new Thickness(1)
                };

                TextBlock dayBlock = new TextBlock
                {
                    Text = dayOfWeek[i],
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 40,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                border.Child = dayBlock;

                Grid.SetRow(border, 0);
                Grid.SetColumn(border, DayNamesGrid.Children.Count);
                DayNamesGrid.Children.Add(border);

                currentDay = currentDay == DayOfWeek.Saturday ? DayOfWeek.Sunday : (currentDay + 1);
            }
        }

        public void UpdateView()
        {
            Debug.WriteLine("UpdateView()");

            if (Date > DateTime.MinValue)
            {
                DateTime calendarDay =
                    Date.AddDays(DayOfWeek.Sunday - Date.DayOfWeek).Date;
                int rowLenght = 7;

                ObservableCollection<Entry> entries;
                EntryDataContext.Instance.TryGetAnniversaryEntries(Date.Year,
                    Date.Month, out entries);

                _DayControls.Clear();

                VisibleRow = 0;

                for (int row = 0; row < 6; row++)
                {
                    bool isRowCollapsed = row > 0 && calendarDay.Month != Date.Month; // Hide last row if not needed

                    VisibleRow = isRowCollapsed ? VisibleRow : VisibleRow + 1;

                    for (int column = 0; column < rowLenght; column++)
                    {
                        ContentControl dayControl = CalendarGrid.Children[row * rowLenght + column] as ContentControl;                        
                        dayControl.Visibility = isRowCollapsed ? Visibility.Collapsed : Visibility.Visible;

                        CalendarMonthViewDayModel model = dayControl.DataContext as CalendarMonthViewDayModel;
                        model.DayOfMonth = calendarDay.Day;
                        model.IsCurrentMonth = calendarDay.Month == Date.Month;
                        model.IsToday = calendarDay == DateTime.Today;
                        model.DateTime = calendarDay;
                        
                        if (calendarDay == DateTime.Today)
                        {
                            model.DayType = DayType.Today;
                        }
                        else if (calendarDay.Month != Date.Month)
                        {
                            model.DayType = DayType.NotCurrentMonth;
                        }
                        else if (HasAnniversaryEntry(entries, calendarDay.Day))
                        {
                            model.DayType = DayType.Anniversary;
                        }
                        else if (DateCalculator.IsFestival(calendarDay))
                        {
                            model.DayType = DayType.Festival;
                        }
                        else if (IsWeekend(calendarDay))
                        {
                            model.DayType = DayType.Weekend;
                        }
                        else
                        {
                            model.DayType = DayType.Workday;
                        }

                        _DayControls.Add(calendarDay, dayControl);
                        UpdateDayControlSubjects(calendarDay);

                        calendarDay = calendarDay.AddDays(1);
                    }
                }

                LayoutGridCanlendar();
                if (MonthViewVisibleRowUpdated != null)
                {
                    MonthViewVisibleRowUpdated(VisibleRow);
                }
            }
        }

        private bool HasAnniversaryEntry(ObservableCollection<Entry> entries, int day)
        {
            bool hasAnniversaryEntry = false;
            if (entries != null)
            {
                foreach (Entry entry in entries)
                {
                    if (entry.StartTime.Day == day)
                    {
                        hasAnniversaryEntry = true;
                        break;
                    }
                }
            }
            return hasAnniversaryEntry;
        }

        private bool IsWeekend(DateTime dateTime)
        {
            return ((dateTime.DayOfWeek == DayOfWeek.Saturday) ||
                    (dateTime.DayOfWeek == DayOfWeek.Sunday));
        }

        private void UpdateDayControlSubjects(DateTime calendarDay)
        {
            if (null != Items)
            {
                ObservableCollection<CalendarItemModel> items = (_DayControls[calendarDay].DataContext as CalendarMonthViewDayModel).CalendarItems;
                items.Clear();

                foreach (CalendarItemModel item in (from i in Items where i.StartDate.Date == calendarDay select i))
                    items.Add(item);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PhoneApplicationFrame phoneFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (null != phoneFrame)
                phoneFrame.OrientationChanged += rootVisual_OrientationChanged;

            UpdateIsInPortraitProperty();
        }

        void rootVisual_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            UpdateIsInPortraitProperty();
        }

        private void UpdateIsInPortraitProperty()
        {
            IsInPortrait = (Orientation & PageOrientation.Portrait) == PageOrientation.Portrait;            
        }

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            if (e.VerticalVelocity > FlickVelocity)
            {
                SoundUtilities.Instance().Play(SoundType.Slipping);
                Date = Date.AddMonths(1);
                CurrentDay = Date;
            }

            if (e.VerticalVelocity < -FlickVelocity)
            {
                SoundUtilities.Instance().Play(SoundType.Slipping);
                Date = Date.AddMonths(-1);
                CurrentDay = Date;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            PhoneApplicationFrame phoneFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (null != phoneFrame)
                phoneFrame.OrientationChanged -= rootVisual_OrientationChanged;
        }

        /// <summary>
        /// Called when month or year is changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnMonthYearChanged(DateTime newValue)
        {
            if (null != MonthYearChanged)
                MonthYearChanged(this, new DateTimeSelectedEventArgs (newValue));
        }

        private void LayoutGridCanlendar()
        {
            int currentRow = CalendarGrid.RowDefinitions.Count;
            int differ = currentRow - VisibleRow;
            if (differ != 0)
            {
                if (differ > 0)
                {
                    // Need to shrink
                    for (int i = 0; i < differ; i++)
                    {
                        CalendarGrid.RowDefinitions.RemoveAt(currentRow - i - 1);
                        CalendarGrid.Height -= 68;
                    }
                }
                else // differ < 0, need to expansion
                {
                    // Need to shrink
                    for (int i = 0; i < Math.Abs(differ); i++)
                    {
                        RowDefinition rowDefinition = new RowDefinition();
                        rowDefinition.Height = new GridLength(68, GridUnitType.Pixel);
                        CalendarGrid.RowDefinitions.Add(rowDefinition);
                        CalendarGrid.Height += 68;
                    }
                }
            }
        }

        private void NotifyMonthViewCurrentDayUpdated(DateTime dateTime)
        {
            if (MonthViewCurrentDayUpdated != null)
            {
                MonthViewCurrentDayUpdated(dateTime);
            }
        }
    }
}

// <copyright file="MainPage.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using MetroCalendar.Resources.Localizations;
using MetroCalendar.Utilities;
using MetroCalendar.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MetroCalendar
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region internal data
        private enum MainPageMode
        {
            MainView,
            MemoRecording,
            MemoPlaying
        }

        private MainPageMode _mode;
        private MainViewModel _mainViewModel;
        private MemoPlayerViewModel _memoPlayerViewModel;
        private MemoRecorderViewModel _memoRecorderViewModel;
        private CalendarPanelViewModel _calendarPanelViewModel;
        private bool _isFromAnniversaryPage;

        ApplicationBarIconButton _memoAppbarIconButton;
        ApplicationBarIconButton _alarmAppbarIconButton;
        ApplicationBarIconButton _anniversaryAppbarIconButton;
        ApplicationBarIconButton _aboutAppbarIconButton;
        ApplicationBarIconButton _stopAppbarIconButton;
        ApplicationBarIconButton _pauseAppbarIconButton;
        ApplicationBarIconButton _resumeAppbarIconButton;
        ApplicationBarIconButton _deleteAppbarIconButton;
        #endregion

        #region constructor
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _mode = MainPageMode.MainView;

            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.IsMenuEnabled = true;
            this.ApplicationBar.IsVisible = true;
            this.ApplicationBar.Opacity = 1.0;
            this.ApplicationBar.ForegroundColor = Colors.White;
            this.ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 0, 140);

            _memoAppbarIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Memo.png", UriKind.Relative));
            _memoAppbarIconButton.Text = AppResources.MemoAppbarText;
            _memoAppbarIconButton.Click += new EventHandler(OnMemoClick);

            _alarmAppbarIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Alarm.png", UriKind.Relative));
            _alarmAppbarIconButton.Text = AppResources.AlarmAppbarText;
            _alarmAppbarIconButton.Click += new EventHandler(OnAlarmClick);

            _anniversaryAppbarIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Anniversary.png", UriKind.Relative));
            _anniversaryAppbarIconButton.Text = AppResources.AnniversaryAppbarText;
            _anniversaryAppbarIconButton.Click += new EventHandler(OnAnniversaryClick);

            _aboutAppbarIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/About.png", UriKind.Relative));
            _aboutAppbarIconButton.Text = AppResources.AboutAppbarText;
            _aboutAppbarIconButton.Click += new EventHandler(OnAboutClick);

            _stopAppbarIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Stop.png", UriKind.Relative));
            _stopAppbarIconButton.Text = AppResources.StopAppbarText;
            _stopAppbarIconButton.Click += new EventHandler(OnStopClick);

            _pauseAppbarIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Pause.png", UriKind.Relative));
            _pauseAppbarIconButton.Text = AppResources.PauseAppbarText;
            _pauseAppbarIconButton.Click += new EventHandler(OnPauseClick);

            _resumeAppbarIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Play.png", UriKind.Relative));
            _resumeAppbarIconButton.Text = AppResources.ResumeAppbarText;
            _resumeAppbarIconButton.Click += new EventHandler(OnResumeClick);

            _deleteAppbarIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Delete.png", UriKind.Relative));
            _deleteAppbarIconButton.Text = AppResources.DeleteAppbarText;
            _deleteAppbarIconButton.Click += new EventHandler(OnDeleteClick);

            this.ApplicationBar.Buttons.Add(_memoAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_alarmAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_anniversaryAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_aboutAppbarIconButton);

            _mainViewModel = new MainViewModel();
            _listbox.ItemsSource = _mainViewModel.Items;

            _calendarMonthView.MonthViewVisibleRowUpdated += OnMonthViewVisibleRowUpdated;
            _calendarMonthView.MonthViewCurrentDayUpdated += OnMonthViewCurrentDayUpdated;

            _calendarPanelViewModel = _calendarPanel.DataContext as CalendarPanelViewModel;
            _calendarPanelViewModel.CurrentDayUpdated += OnCurrentDayUpdated;

            _memoRecorderViewModel = _memoRecorderPanel.DataContext as MemoRecorderViewModel;
            _memoRecorderViewModel.RecorderInitialized += OnRecorderInitialized;
            _memoRecorderPanel.RecordingTimeout += OnRecordingTimeout;

            _memoPlayerViewModel = _memoPlayerPanel.DataContext as MemoPlayerViewModel;
            _memoPlayerViewModel.PlayCompleted += OnPlayCompleted;

            BeginRecorderStoryboard.Completed += OnBeginRecorderStoryboardCompleted;
            StopRecorderStoryboard.Completed += OnStopRecorderStoryboardCompleted;
            BeginPlayerStoryboard.Completed += OnBeginPlayerStoryboardCompleted;
            StopPlayerStoryboard.Completed += OnStopPlayerStoryboardCompleted;
        }
        #endregion

        #region internal methods
        private void OnRecorderInitialized()
        {
            _mode = MainPageMode.MemoRecording;
            EnableApplicationBar();
        }

        private void OnCurrentDayUpdated(DateTime dateTime)
        {
            if (dateTime.Year != _calendarMonthView.Date.Year ||
                dateTime.Month != _calendarMonthView.Date.Month)
            {
                _calendarMonthView.Date = 
                    new DateTime(dateTime.Year, dateTime.Month, 1);
            }

            _calendarMonthView.CurrentDay = dateTime;

            _mainViewModel.Load(dateTime);
        }

        private void OnMonthViewCurrentDayUpdated(DateTime dateTime)
        {
            if (_calendarPanelViewModel.Date.Year != dateTime.Year ||
                _calendarPanelViewModel.Date.Month != dateTime.Month ||
                _calendarPanelViewModel.Date.Day != dateTime.Day)
            {
                _calendarPanelViewModel.Date = dateTime;
            }
        }

        private void OnPlayCompleted()
        {
            _mode = MainPageMode.MainView;

            _memoPlayerViewModel.Stop();
            StopPlayerStoryboard.Begin();

            this.ApplicationBar.Buttons.Clear();
            this.ApplicationBar.Buttons.Add(_memoAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_alarmAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_anniversaryAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_aboutAppbarIconButton);
        }

        private void OnMonthViewVisibleRowUpdated(int row)
        {
            const int MaxVisibleRow = 6;
            _listboxBorder.Height = (MaxVisibleRow - row + 1) * 50;
            _listbox.Height = (MaxVisibleRow - row + 1) * 50;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel.Load(_calendarMonthView.CurrentDay);
            _memoRecorderPanel.Reset();

            if (_isFromAnniversaryPage)
            {
                _isFromAnniversaryPage = false;
                //DateTime dateTime = _calendarMonthView.CurrentDay;
                //_calendarMonthView.UpdateView();
                //_calendarMonthView.CurrentDay = dateTime;
            }
        }

        private void OnRecordingTimeout()
        {
            _mode = MainPageMode.MainView;

            StopRecorderStoryboard.Begin();

            this.ApplicationBar.Buttons.Clear();
            this.ApplicationBar.Buttons.Add(_memoAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_alarmAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_anniversaryAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_aboutAppbarIconButton);

            DisableApplicationBar();
        }

        private void OnMemoClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            BeginRecorderStoryboard.Begin();

            this.ApplicationBar.Buttons.Clear();
            this.ApplicationBar.Buttons.Add(_stopAppbarIconButton);

            DisableApplicationBar();
        }

        private void OnAlarmClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            string startTimeStr = _calendarMonthView.CurrentDay.ToLongDateString();
            this.NavigationService.Navigate(
                new Uri("/AlarmPage.xaml?StartTime=" + startTimeStr, UriKind.Relative));
        }

        private void OnAnniversaryClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            _isFromAnniversaryPage = true;
            string startTimeStr = _calendarMonthView.CurrentDay.ToLongDateString();
            this.NavigationService.Navigate(
                new Uri("/AnniversaryPage.xaml?StartTime=" + startTimeStr, UriKind.Relative));
        }

        private void OnAboutClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            this.NavigationService.Navigate(
                new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void OnStopClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);
            _mode = MainPageMode.MainView;
            StopMemoRecording();
        }

        private void OnPauseClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            _memoPlayerViewModel.Pause();
            _memoPlayerPanel.Stop();

            this.ApplicationBar.Buttons.Remove(_pauseAppbarIconButton);
            this.ApplicationBar.Buttons.Insert(0, _resumeAppbarIconButton);
        }

        private void OnResumeClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            _memoPlayerViewModel.Resume();
            _memoPlayerPanel.Start();

            this.ApplicationBar.Buttons.Remove(_resumeAppbarIconButton);
            this.ApplicationBar.Buttons.Insert(0, _pauseAppbarIconButton);
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);
            _mode = MainPageMode.MainView;
            _memoPlayerViewModel.Remove();
            StopMemoPlaying();
        }

        private void OnBeginRecorderStoryboardCompleted(object sender, EventArgs e)
        {
            _memoRecorderPanel.Start(_calendarMonthView.CurrentDay);
        }

        private void OnStopRecorderStoryboardCompleted(object sender, EventArgs e)
        {
            _mainViewModel.Load(_calendarMonthView.CurrentDay);
            _memoRecorderPanel.Reset();

            EnableApplicationBar();
        }

        private void OnBeginPlayerStoryboardCompleted(object sender, EventArgs e)
        {
            _mode = MainPageMode.MemoPlaying;

            _memoPlayerViewModel.Play();
            EnableApplicationBar();
        }

        private void OnStopPlayerStoryboardCompleted(object sender, EventArgs e)
        {
            _mainViewModel.Load(_calendarMonthView.CurrentDay);
            _memoPlayerPanel.Stop();

            EnableApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TileUtilities.Update();

            base.OnNavigatedTo(e);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            switch (_mode)
            {
                case MainPageMode.MainView:
                    base.OnBackKeyPress(e);
                    break;

                case MainPageMode.MemoRecording:
                    _mode = MainPageMode.MainView;
                    StopMemoRecording();
                    e.Cancel = true;
                    break;

                case MainPageMode.MemoPlaying:
                    _mode = MainPageMode.MainView;
                    StopMemoPlaying();
                    e.Cancel = true;
                    break;

                default:
                    break;
            }
        }

        private void StopMemoRecording()
        {
            _memoRecorderPanel.Stop();
            StopRecorderStoryboard.Begin();

            this.ApplicationBar.Buttons.Clear();
            this.ApplicationBar.Buttons.Add(_memoAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_alarmAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_anniversaryAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_aboutAppbarIconButton);

            DisableApplicationBar();
        }

        private void StopMemoPlaying()
        {
            _memoPlayerViewModel.Stop();
            StopPlayerStoryboard.Begin();

            this.ApplicationBar.Buttons.Clear();
            this.ApplicationBar.Buttons.Add(_memoAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_alarmAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_anniversaryAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_aboutAppbarIconButton);

            DisableApplicationBar();
        }

        private void OnDoubleListboxTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBox _listbox = sender as ListBox;
            Entry entry = _listbox.SelectedItem as Entry;
            if (entry != null)
            {
                EntryType entryType =
                    (EntryType)Enum.Parse(typeof(EntryType), entry.EntryType, true);
                switch (entryType)
                {
                    case EntryType.Memo:
                        PlayMemo(entry);
                        break;
                    case EntryType.Alarm:
                        NavigationService.Navigate(
                            new Uri("/AlarmPage.xaml?EntryId=" + entry.EntryId, UriKind.Relative));
                        break;
                    case EntryType.Anniversary:
                        _isFromAnniversaryPage = true;
                        NavigationService.Navigate(
                            new Uri("/AnniversaryPage.xaml?EntryId=" + entry.EntryId, UriKind.Relative));
                        break;
                    default:
                        break;
                }
            }
        }

        private void PlayMemo(Entry entry)
        {
            BeginPlayerStoryboard.Begin();
            _memoPlayerPanel.Start();

            _memoPlayerViewModel.Load(entry);

            this.ApplicationBar.Buttons.Clear();
            this.ApplicationBar.Buttons.Add(_pauseAppbarIconButton);
            this.ApplicationBar.Buttons.Add(_deleteAppbarIconButton);

            DisableApplicationBar();
        }

        private void EnableApplicationBar()
        {
            for (int i = 0; i < ApplicationBar.Buttons.Count; ++i)
            {
                (ApplicationBar.Buttons[i] as ApplicationBarIconButton).IsEnabled = true;
            }
        }

        private void DisableApplicationBar()
        {
            for (int i = 0; i < ApplicationBar.Buttons.Count; ++i)
            {
                (ApplicationBar.Buttons[i] as ApplicationBarIconButton).IsEnabled = false;
            }
        }
        #endregion
    }
}

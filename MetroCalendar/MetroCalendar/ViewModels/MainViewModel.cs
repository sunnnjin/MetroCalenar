// <copyright file="MainViewModel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MetroCalendar.Resources.Localizations;

namespace MetroCalendar.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region internal data
        private DateTime _currentDate;
        #endregion

        public ObservableCollection<Entry> Items { get; private set; }

        #region property
        public DateTime CurrentDate
        {
            get
            {
                return _currentDate;
            }
            set
            {
                DateTime newCreateDate = (DateTime)value;
                if (_currentDate != newCreateDate)
                {
                    _currentDate = newCreateDate;
                    RaisePropertyChanged("CurrentDate");
                }
            }
        }
        #endregion

        public MainViewModel()
        {
            Items = new ObservableCollection<Entry>();

            CurrentDate = DateTime.Now;
        }

        public void Load(DateTime dateTime)
        {
            ObservableCollection<Entry> entryCollection;
            EntryDataContext.Instance.TryGetEntries(dateTime, out entryCollection);
            if (entryCollection != null)
            {
                Items.Clear();
                foreach (Entry entry in entryCollection)
                {
                    if (entry.EntryType == EntryType.Memo.ToString())
                    {
                        entry.Subject = AppResources.VoiceMemoText;
                    }

                    Items.Add(entry);
                }
            }
        }

        #region INotifyPropertyChanged member
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

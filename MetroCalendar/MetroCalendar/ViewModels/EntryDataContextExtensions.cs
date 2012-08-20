// <copyright file="EntryDataContextExtensions.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;

namespace MetroCalendar.ViewModels
{
    public static class EntryDataContextExtensions
    {
        public static void AddEntry(this EntryDataContext db, Entry entry)
        {
            try
            {
                EntryDataContext.Instance.EntryTable.InsertOnSubmit(entry);
                // Submit the changes to the database.
                EntryDataContext.Instance.SubmitChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpdateEntry(this EntryDataContext db, Entry entry)
        {
            try
            {
                // Query the database for the existing entry to be updated.
                var existingEntry = (from Entry e in db.EntryTable
                           where e.EntryId == entry.EntryId
                           select e).First();

                // Updates existing entry here.
                existingEntry.EntryType = entry.EntryType;
                existingEntry.CreateTime = entry.CreateTime;
                existingEntry.StartTime = entry.StartTime.Date;
                existingEntry.ExpirationTime = entry.ExpirationTime;
                existingEntry.Subject = entry.Subject;
                existingEntry.RepeatType = entry.RepeatType;
                existingEntry.ExtraInfo = entry.ExtraInfo;
                existingEntry.AttachmentFile = entry.AttachmentFile;
                existingEntry.AlarmOn = entry.AlarmOn;
                existingEntry.AlarmTime = entry.AlarmTime;
                existingEntry.RingTone = entry.RingTone;
                existingEntry.Vibrate = entry.Vibrate;
                // Insert any additional changes to column values.

                // Submit the changes to the database.
                EntryDataContext.Instance.SubmitChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void RemoveEntry(this EntryDataContext db, string id)
        {
            try
            {
                // Query the database for the existing entry to be removed.
                var data = from Entry e in db.EntryTable
                           where e.EntryId == id
                           select e;

                foreach (var entry in data)
                {
                    db.EntryTable.DeleteOnSubmit(entry);
                }

                // Submit the changes to the database.
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static bool TryGetEntry(
            this EntryDataContext db, 
            string id, out Entry entry)
        {
            bool ret = true;
            entry = null;

            try
            {
                // Query the database for the entries by id.
                var entries = from Entry e in db.EntryTable
                           where e.EntryId == id
                           select e;

                foreach (var e in entries)
                {
                    entry = e as Entry;
                    break;
                }
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

        public static bool TryGetEntries(
            this EntryDataContext db, 
            DateTime dateTime, 
            out ObservableCollection<Entry> collection)
        {
            bool ret = true;
            collection = null;

            try
            {
                // Query the database for the entries by date time.
                var entries = from Entry entry in db.EntryTable
                              where (((entry.RepeatType == RepeatType.NotRepeated.ToString()) &&
                                             (entry.StartTime == dateTime.Date)) 
                                     || ((entry.RepeatType == RepeatType.Daily.ToString()) &&
                                             ((entry.StartTime <= dateTime.Date) && 
                                             (dateTime <= entry.ExpirationTime)))
                                     || ((entry.RepeatType == RepeatType.Weekly.ToString()) &&
                                             ((entry.StartTime <= dateTime.Date) && 
                                             (dateTime <= entry.ExpirationTime) && 
                                             (entry.StartTime.DayOfWeek == dateTime.DayOfWeek)))
                                     || ((entry.RepeatType == RepeatType.Fortnightly.ToString()) &&
                                             ((entry.StartTime <= dateTime.Date) && 
                                             (dateTime <= entry.ExpirationTime) && 
                                             (dateTime.Date - entry.StartTime.Date).Days % 14 == 0))
                                     || ((entry.RepeatType == RepeatType.Monthly.ToString()) &&
                                             ((entry.StartTime <= dateTime.Date) && 
                                             (entry.StartTime <= entry.ExpirationTime) && 
                                             (entry.StartTime.Day == dateTime.Day))
                                     || (entry.RepeatType == RepeatType.Yearly.ToString()) &&
                                             ((entry.StartTime <= dateTime.Date) && 
                                             (entry.StartTime <= entry.ExpirationTime) &&
                                             (entry.StartTime.Month == dateTime.Month) &&
                                             (entry.StartTime.Day == dateTime.Day))))
                              select entry;

                collection = new ObservableCollection<Entry>(entries);
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

        public static bool TryGetAnniversaryEntries(
            this EntryDataContext db, 
            int year, int month,
            out ObservableCollection<Entry> collection)
        {
            bool ret = true;
            collection = null;

            try
            {
                // Query the database for the entries by year and month
                var entries = from Entry entry in db.EntryTable
                              where ((entry.EntryType == EntryType.Anniversary.ToString()) &&
                                (entry.StartTime.Year == year) &&
                                (entry.StartTime.Month == month))
                              select entry;

                collection = new ObservableCollection<Entry>(entries);
            }
            catch
            {
                ret = false;
            }

            return ret;
        }
    }
}

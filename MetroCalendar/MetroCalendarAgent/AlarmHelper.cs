// <copyright file="AlarmHelper.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Phone.Scheduler;

namespace MetroCalendarAgent
{
    public static class AlarmHelper
    {
        public enum RepeatType
        {
            NotRepeated,
            Daily,
            Weekly,
            Fortnightly,
            Monthly,
            Yearly
        }

        private static string MetroCalendarAlarm = "MetroCalendar:";

        public static int AlarmCount()
        {
            int alarmCount = 0;

            List<Alarm> alarms = (List<Alarm>)ScheduledActionService.GetActions<Alarm>();
            foreach (Alarm alarm in alarms)
            {
                if (alarm.Name.Contains(MetroCalendarAlarm))
                {
                    if (alarm.IsEnabled && alarm.IsScheduled)
                    {
                        ++alarmCount;
                    }
                }
            }

            return alarmCount;
        }

        public static Alarm NearestAlarm()
        {
            Alarm nearestAlarm = null;

            List<Alarm> alarms = (List<Alarm>)ScheduledActionService.GetActions<Alarm>();
            foreach (Alarm alarm in alarms)
            {
                if (alarm.Name.Contains(MetroCalendarAlarm))
                {
                    if (alarm.IsEnabled && alarm.IsScheduled)
                    {
                        if (nearestAlarm == null)
                        {
                            nearestAlarm = alarm;
                        }
                        else
                        {
                            if (Compare(alarm, nearestAlarm) < 0)
                            {
                                nearestAlarm = alarm;
                            }
                        }
                    }
                }
            }

            return nearestAlarm;
        }

        private static RecurrenceInterval Recurrence(string recurrenceStr)
        {
            RecurrenceInterval recurrence = RecurrenceInterval.None;

            RepeatType repeatType =
                (RepeatType)Enum.Parse(typeof(RepeatType), recurrenceStr, true);

            switch (repeatType)
            {
                case RepeatType.NotRepeated:
                    recurrence = RecurrenceInterval.None;
                    break;
                case RepeatType.Daily:
                    recurrence = RecurrenceInterval.Daily;
                    break;
                case RepeatType.Weekly:
                    recurrence = RecurrenceInterval.Weekly;
                    break;
                case RepeatType.Fortnightly:
                    throw new NotSupportedException("Not support Fortnightly!");
                case RepeatType.Monthly:
                    recurrence = RecurrenceInterval.Monthly;
                    break;
                case RepeatType.Yearly:
                    recurrence = RecurrenceInterval.Yearly;
                    break;
                default:
                    break;
            }

            return recurrence;
        }

        private static int Compare(Alarm firstAlarm, Alarm secondAlarm)
        {
            DateTime firstDateTime = NextAlarmTime(firstAlarm);
            DateTime secondDateTime = NextAlarmTime(secondAlarm);

            if (firstDateTime > secondDateTime)
            {
                return 1;
            }
            else if (firstDateTime < secondDateTime)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private static DateTime NextAlarmTime(Alarm alarm)
        {
            DateTime dateTime = DateTime.MinValue;

            for (DateTime dt = alarm.BeginTime; dt <= alarm.ExpirationTime; )
            {
                if (DateTime.Now < dt)
                {
                    dateTime = dt;
                    break;
                }

                switch (alarm.RecurrenceType)
                {
                    case RecurrenceInterval.None:
                        break;
                    case RecurrenceInterval.Daily:
                        dt.AddDays(1);
                        break;
                    case RecurrenceInterval.Weekly:
                        dt.AddDays(7);
                        break;
                    case RecurrenceInterval.Monthly:
                        dt.AddMonths(1);
                        break;
                    case RecurrenceInterval.EndOfMonth:
                        break;
                    case RecurrenceInterval.Yearly:
                        dt.AddYears(1);
                        break;
                    default:
                        break;
                }
            }

            return dateTime;
        }
    }
}

// <copyright file="DateCalculator.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;

namespace MetroCalendar.Utilities
{
    public static class DateCalculator
    {
        private static int[] MonthAdd = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
        private static int[] LunarData = {2635,333387,1701,1748,267701,694,2391,133423,1175,396438 
                                             ,3402,3749,331177,1453,694,201326,2350,465197,3221,3402 
                                             ,400202,2901,1386,267611,605,2349,137515,2709,464533,1738 
                                             ,2901,330421,1242,2651,199255,1323,529706,3733,1706,398762 
                                             ,2741,1206,267438,2647,1318,204070,3477,461653,1386,2413 
                                             ,330077,1197,2637,268877,3365,531109,2900,2922,398042,2395 
                                             ,1179,267415,2635,661067,1701,1748,398772,2742,2391,330031 
                                             ,1175,1611,200010,3749,527717,1452,2742,332397,2350,3222 
                                             ,268949,3402,3493,133973,1386,464219,605,2349,334123,2709 
                                             ,2890,267946,2773,592565,1210,2651,395863,1323,2707,265877};

        public static DateTime LastTime(DateTime dateTime)
        {
            return dateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public static bool IsFestival(DateTime dtDay)
        {
            // return value
            bool isFestival = false;

            // lunar year, month, date
            int lunaryear = 0;
            int lunarmonth = 0;
            int lunardate = 0;
            // gregorian year, month, date
            int nyear = 0;
            int nmonth = 0;
            int nday = 0;

            string sYear = dtDay.Year.ToString();
            string sMonth = dtDay.Month.ToString();
            string sDay = dtDay.Day.ToString();

            int year = 0;
            int month = 0;
            int day = 0;

            try
            {
                year = int.Parse(sYear);
                if (year > 2020)
                    year = 2020;
                month = int.Parse(sMonth);
                day = int.Parse(sDay);
            }
            catch
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
                day = DateTime.Now.Day;
            }

            // initialize nyear, nmonth, nday
            nyear = year;
            nmonth = month;
            nday = day;

            int nTheDate;
            int nIsEnd;
            int k, m, n, nBit, i;
            string calendar = string.Empty;
            nTheDate = (year - 1921) * 365 + (year - 1921) / 4 + day + MonthAdd[month - 1] - 38;
            if ((year % 4 == 0) && (month > 2))
                nTheDate += 1;

            nIsEnd = 0;
            m = 0;
            k = 0;
            n = 0;

            while (nIsEnd != 1)
            {
                if (LunarData[m] < 4095)
                    k = 11;
                else
                    k = 12;
                n = k;
                while (n >= 0)
                {
                    nBit = LunarData[m];
                    for (i = 1; i < n + 1; i++)
                        nBit = nBit / 2;
                    nBit = nBit % 2;
                    if (nTheDate <= (29 + nBit))
                    {
                        nIsEnd = 1;
                        break;
                    }
                    nTheDate = nTheDate - 29 - nBit;
                    n = n - 1;
                }
                if (nIsEnd == 1)
                    break;
                m = m + 1;
            }

            year = 1921 + m;
            month = k - n + 1;
            day = nTheDate;

            if (k == 12)
            {
                if (month == LunarData[m] / 65536 + 1)
                    month = 1 - month;
                else if (month > LunarData[m] / 65536 + 1)
                    month = month - 1;
            }

            lunaryear = year;
            lunarmonth = month;
            lunardate = day;

            if ((lunardate == 9 && lunarmonth == 9) ||
                (lunardate == 15 && lunarmonth == 8) ||
                (lunardate == 7 && lunarmonth == 7) ||
                (lunardate == 5 && lunarmonth == 5) ||
                (lunardate == 15 && lunarmonth == 1) ||
                (lunardate == 1 && lunarmonth == 1))
            {
                isFestival = true;
            }

            if ((nday == 1 && nmonth == 1) ||
                (nday == 1 && nmonth == 5) ||
                (nday == 1 && nmonth == 6) ||
                (nday == 1 && nmonth == 10))
            {
                isFestival = true;
            }

            return isFestival;
        }
    }
}

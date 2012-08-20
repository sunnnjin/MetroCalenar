// <copyright file="EntryDataContext.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System.Data.Linq;

namespace MetroCalendar.ViewModels
{
    public class EntryDataContext : DataContext
    {
        #region internal data
        private static EntryDataContext _instance;
        #endregion

        #region constructor
        // singleton
        public static EntryDataContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Specify the local database connection string.
                    string DBConnectionString = "Data Source=isostore:/calendar.sdf";

                    // Create the database if it does not exist.
                    _instance = new EntryDataContext(DBConnectionString);

                    if (_instance.DatabaseExists() == false)
                    {
                        // Create the local database.
                        _instance.CreateDatabase();
                    }
                }
                return _instance;
            }
        }

        // Pass the connection string to the base class.
        private EntryDataContext(string connectionString)
            : base(connectionString)
        { }
        #endregion

        #region property
        // Specify a table for the entries.
        public Table<Entry> EntryTable;
        #endregion
    }
}

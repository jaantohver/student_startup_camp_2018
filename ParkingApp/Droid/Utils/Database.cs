using System;
using System.Linq;
using System.Collections.Generic;

using SQLite;

namespace ParkingApp.Droid
{
    public class HivescaleDB
    {
        public static HivescaleDB Instance { get; set; }

        SQLiteConnection connection;

        readonly object dblock = new object ();

        readonly string DBPath;

        public HivescaleDB (string DBPath)
        {
            this.DBPath = DBPath ?? throw new ArgumentException ("DB path is empty");

            CreateTables ();
        }

        void Connect ()
        {
            connection = new SQLiteConnection (DBPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex) { BusyTimeout = TimeSpan.FromSeconds (60) };
        }

        void CloseConnection ()
        {
            if (connection == null)
                return;

            connection.Close ();
            connection.Dispose ();
            connection = null;
        }

        void CreateTables ()
        {
            lock (dblock) {
                Connect ();
                connection.CreateTable<Shame> ();
                CloseConnection ();
            }
        }

        public List<Shame> GetShames ()
        {
            lock (dblock) {
                Connect ();

                List<Shame> shames = connection.Table<Shame> ().ToList ();

                CloseConnection ();

                return shames;
            }
        }

        public void AddShame (Shame s)
        {
            lock (dblock) {
                Connect ();

                connection.Insert (s);

                CloseConnection ();
            }
        }

        public void UpdateShame (Shame s)
        {
            lock (dblock) {
                Connect ();

                int res = connection.Update (s);

                CloseConnection ();

                Console.WriteLine (res);
            }
        }

        public void RemoveShame (Shame s)
        {
            lock (dblock) {
                Connect ();

                int res = connection.Delete (s);

                CloseConnection ();

                Console.WriteLine (res);
            }
        }
    }
}
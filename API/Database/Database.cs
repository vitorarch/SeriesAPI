using System.Data.SQLite;

namespace API.Database
{
    public class Database
    {

        public static string Connection
        {
            get
            {
                string DataBasePath = @"Data Source = D:\Vitor\SeriesAPI\Series.db";//; Version = 3";
                return DataBasePath;
            }
        }

        public static SQLiteConnection Connect
        {
            get
            {
                var conn = new SQLiteConnection(Connection);
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    return conn;
                }
                else return conn;
            }
        }

        public SQLiteConnection Disconnect
        {
            get
            {
                var conn = new SQLiteConnection(Connection);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                    return conn;
                }
                return conn;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataBase_Connection
{
    #region Class SQL
    namespace SQL
    {
        public class ClassDbSql
        {
            #region propertys
            private static SqlCommand _com;
            private static SqlDataAdapter _da;
            private static DataSet _ds;
            private static DataTable _dt;
            private static SqlDataReader _dr;
            #endregion

            #region Connection

            private const string DbName = @"Personal_Accounting_Db"; //Enter the database name is here
            //private static string _startupPath = Application.StartupPath + "\\" + DbName;

            //private static SqlConnection con = new SqlConnection(@"Data Source=.;AttachDbFilename=" + StartupPath + ".mdf; Integrated Security = True");//method 1
            private static readonly SqlConnection Con = new SqlConnection(@"server=.;databas=" + DbName + "; trusted_connection=true");//method 2
            #endregion

            #region Methods ConnectionState
            public static int IsConnectionOk()
            {
                var cmd = new SqlCommand("SELECT 1", Con);
                Con.Open();
                var i = (int)cmd.ExecuteScalar();
                return i;
            }
            public static SqlConnection OpenConnection()
            {
                if (Con.State == ConnectionState.Closed || Con.State == ConnectionState.Broken)
                {
                    Con.Open();
                }
                return Con;
            }
            public static SqlConnection CloseConnection()
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                return Con;
            }
            #endregion

            #region Methods Execute
          
            // نمونه کد برای این بخش 
            // اولین آرگومان دستور کوئری شما بصورت رشته
            // دومین آرگومان پارامتر های می باشد مثل
            // ClassDB_SQL.ExecuteNonQuery("insert into NameTable (NameFild) values(@a)", new Dictionary<string, object>()
            //{ {"a","your value" } });
            // <param name="sqlQuery"></param>
            // <param name="parameter"></param>
            
            public static bool ExecuteNonQuery(string sqlQuery, Dictionary<string, object> paramter)
            {
                _com = new SqlCommand {CommandText = sqlQuery, Connection = OpenConnection()};
                foreach (var param in paramter)
                {
                    _com.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
                _com.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            public static SqlCommand ExecuteScalar(string sqlQuery, Dictionary<string, object> paramters)
            {
                _com = new SqlCommand {CommandText = sqlQuery, Connection = OpenConnection()};
                foreach (var param in paramters)
                {
                    _com.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
                _com.ExecuteScalar();
                return _com;
            }
            public static SqlDataReader ExecuteReader(string sqlQuery, Dictionary<string, object> paramters)
            {
                _com = new SqlCommand {CommandText = sqlQuery, Connection = OpenConnection()};
                foreach (var param in paramters)
                {
                    _com.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
                _dr = _com.ExecuteReader();
                return _dr;
            }
            #endregion

            #region DataTable and DataSet
            public static DataTable ReturnDataTable(string sqlQuery)
            {
                _dt = new DataTable();
                _com = new SqlCommand();
                _da = new SqlDataAdapter(sqlQuery, OpenConnection());
                _dt.Reset();
                _da.Fill(_dt);
                return _dt;
            }
            // <summary>
            //نمونه کد
            //  DataSet ds = ClassDB_SQL.returnDataSet("select * from my-table");
            // dataGridView1.DataSource = ds.Tables[0];
            // <param name="sqlQuery"></param>
            public static DataSet ReturnDataSet(string sqlQuery)
            {
                _ds = new DataSet();
                _com = new SqlCommand();
                _da = new SqlDataAdapter(sqlQuery, OpenConnection());
                _ds.Reset();
                _da.Fill(_ds);
                return _ds;
            }
            #endregion

            #region Methods Backup and Restore Database
            public static void BackUp_DB(string nameDataBase)
            {
                var sfd = new SaveFileDialog
                {
                    OverwritePrompt = true,
                    Filter = @"SQL BackUp Files ALL Files (*.*) |*.*| (*.Bak)|*.Bak",
                    DefaultExt = "Bak",
                    FilterIndex = 1,
                    FileName = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss"),
                    Title = @"BackUp SQL Files"
                };

                if (sfd.ShowDialog() != DialogResult.OK) return;
                OpenConnection();
                ExecuteNonQuery(@"BACKUP DATABASE [" + nameDataBase + "] TO  DISK='" + sfd.FileName + "'",
                    new Dictionary<string, object>());
                CloseConnection();
            }
            public void Restore_DB(string nameDataBase)
            {
                var ofd = new OpenFileDialog
                {
                    Filter = @"SQL BackUp Files ALL Files (*.*) |*.*| (*.Bak)|*.Bak",
                    FilterIndex = 1,
                    Title = @"BackUp SQL Files",
                    FileName = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")
                };

                if (ofd.ShowDialog() != DialogResult.OK) return;
                OpenConnection();
                ExecuteNonQuery(@"Alter DATABASE [" + nameDataBase + "] SET SINGLE_USER with ROLLBACK IMMEDIATE " + "USE master " + " RESTORE DATABASE [" + nameDataBase + "] FROM DISK =N'" + ofd.FileName + "' with RECOVERY,REPLACE", new Dictionary<string, object>());
                CloseConnection();
            }
            #endregion

            #region Method Check Run Service SQL Server
            // service name : "MSSQLSERVER"
            // <param name="serviceName"></param>
            public static string CheckEngine(string serviceName)
            {
                //Step 1 = add reference ServiceProcess

                //step 2 = add using System.ServiceProcess;

                //step 3 = Uncomment all code

                //ServiceController sc = new ServiceController(SERVICENAME);

                //switch (sc.Status)
                //{
                //    case ServiceControllerStatus.Running:
                //        return "Running";
                //    case ServiceControllerStatus.Stopped:
                //        return "Stopped";
                //    case ServiceControllerStatus.Paused:
                //        return "Paused";
                //    case ServiceControllerStatus.StopPending:
                //        return "Stopping";
                //    case ServiceControllerStatus.StartPending:
                //        return "Starting";

                //    default:
                //        return "Status Changing";
                //}

                return null;//Then Uncomment the code above to delete this line code
            }
            #endregion
        }
    }
    #endregion

    #region Class Access
    namespace OLEDB
    {
        public class ClassDbAccess
        {
            #region propertys
            private static OleDbCommand _com;
            private static OleDbDataAdapter _da;
            private static DataSet _ds;
            private static DataTable _dt;
            private static OleDbDataReader _dr;
            #endregion

            #region Connection

            private const string DbName = "MyDB"; //Enter the database name is here
            private static readonly OleDbConnection Con = new OleDbConnection("provider=microsoft.jet.oledb.4.0; data source=" + DbName + ".mdb");
            #endregion

            #region Methods Connection State

            public static OleDbConnection OpenConnection()
            {
                if (Con.State == ConnectionState.Closed || Con.State == ConnectionState.Broken)
                    Con.Open();
                return Con;
            }
            public static OleDbConnection CloseConnection()
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                return Con;
            }

            #endregion

            #region Methods Execute
            public static bool RunQuery(string sqlQuery)
            {
                var command = sqlQuery;
                _com = new OleDbCommand {Connection = OpenConnection(), CommandText = command};
                _com.ExecuteNonQuery();
                return true;
            }
            public static OleDbDataReader ReturnDataReader(string sqlQuery)
            {
                var command = sqlQuery;
                _com = Con.CreateCommand();
                _com.CommandText = command;
                _dr = _com.ExecuteReader();
                return _dr;
            }
            #endregion

            #region DataTable and Dataset
            public static DataTable ReturnDataTable(string sqlQuery)
            {
                _da = new OleDbDataAdapter();
                _com = new OleDbCommand();
                var command = sqlQuery; _dt = new DataTable();
                _com.Connection = OpenConnection();
                _com.CommandText = command;
                _da.SelectCommand = _com;
                _da.Fill(_dt);
                return _dt;
            }
            public static DataSet ReturnDataSet(string sqlQuery)
            {
                _com = new OleDbCommand();
                var command = sqlQuery;
                _ds = new DataSet();
                OpenConnection();
                _com = Con.CreateCommand();
                _da = new OleDbDataAdapter(command, OpenConnection());
                _ds.Reset();
                _da.Fill(_ds);
                return _ds;
            }
            #endregion
        }
    }
    #endregion
}

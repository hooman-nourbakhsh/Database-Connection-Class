using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Connection_Class
{
    /// <summary>
    ///This is a class called "Connection_Class" that is for SQL insert(Update Delete Search), and displaying data in DataGridView and 
    ///using commands (Scaler, DataReader).
    ///It also performs database backup and recovery operations.
    /// می باشد و (Scaler , DataReader) نمایش دیتا در دیتا گرید ویو و استفاده از دستورات (Insert, Update Delete Search) SQL است که برای دستورات پایه "Connection_Class" این یک کلاس با نام 
    ///همچنین عملیات بکاپ گیری از دیتابیس و بازیابی آن را هم انجام می دهد
    /// (Signature (HooMaN) Version 1.0)
    /// </summary>

    public class Connection_Query
    {
        //string ConnectionString = "Data Source=.;AttachDBFileName=|DataDirectory|\\Db\\Personal_Accounting_DB.mdf;Integrated Security=True";//method 1
        private static string ConnectionString = @"server=.;database=Personal_Accounting_D;trusted_connection=true";//method 2
        private static SqlConnection con;

        public static void OpenConection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }
        public static void CloseConnection()
        {
            con.Close();
        }
        /// <summary>
        /// نمونه کد
        ///  var q = Connection_Query.ExecuteNonQuery("select Count(*) from tblUsers where U_name = @Username and U_pass = @Password",new Dictionary<string,object>() { { "Username",txtUserName.Text },{ "Password",HashPassword } });
        /// <param name="Query_"></param>
        /// </summary>
        public static void ExecuteNonQuery(string Query_, Dictionary<string, object> Paramter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.Connection = con;
            cmd.CommandText = Query_;
            foreach (var Param in Paramter)
            {
                cmd.Parameters.AddWithValue("@" + Param.Key, Param.Value);
            }
            cmd.ExecuteNonQuery();

        }

        public static SqlCommand ExecuteScaler(string Query_, Dictionary<string, object> Paramter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.Connection = con;
            cmd.CommandText = Query_;
            foreach (var Param in Paramter)
            {
                cmd.Parameters.AddWithValue("@" + Param.Key, Param.Value);
            }
            return cmd;
        }
        /// <summary>
        /// نمونه کد
        /// SqlDataReader dr = ClassObject.DataReader("Select * From Student");  
        /// dr.Read();  
        /// textBox1.Text = dr["Stdnt_Name"].tostring();
        /// <param name="Query_"></param> 
        /// </summary>
        public static SqlDataReader DataReader(string Query_, Dictionary<string, object> Paramter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query_;
            //cmd.Parameters.Clear();
            foreach (var Param in Paramter)
            {
                cmd.Parameters.AddWithValue("@" + Param.Key, Param.Value);
            }
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
        /// <summary>
        /// نمونه کد
        /// dataGridView1.datasource = ClassObject.ShowDataInGridView("Select * From Student")
        /// <param name="Query_"></param>
        /// </summary>

        public static object ShowData(string Query_)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter dr = new SqlDataAdapter(Query_, ConnectionString);
            dr.Fill(ds);
            object dataum = ds.Tables[0];
            return dataum;

        }
        /// <summary>
        /// نمونه کد
        /// dataGridView1.datasource = ClassObject.ShowDataInGridView("Select * From Student")
        /// </summary>
        /// <param name="Query_"></param>
        /// <returns></returns>
        public static DataTable FillDataTable(string Query_)
        {
            SqlDataAdapter dr = new SqlDataAdapter(Query_, ConnectionString);
            DataTable dt = new DataTable();
            dr.Fill(dt);

            return dt;
        }
        /// <summary>
        /// نمونه کد
        /// "اسم دیتابیس"
        ///<param name="Name_DataBase"></param>
        /// </summary>
        public void BackUp_DB(string Name_DataBase)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.OverwritePrompt = true;
            sfd.Filter = @"SQL BackUp FIles ALL Files (*.*) |*.*| (*.Bak)|*.Bak";
            sfd.DefaultExt = "Bak";
            sfd.FilterIndex = 1;
            sfd.FileName = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            sfd.Title = "BackUp SQL Files";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                OpenConection();
                ExecuteNonQuery(@"BACKUP DATABASE [" + Name_DataBase + "] TO  DISK='" + sfd.FileName + "'", new Dictionary<string, object>());
                CloseConnection();
            }
        }
        /// <summary>
        /// نمونه کد
        /// "اسم دیتابیس"
        ///<param name="Name_DataBase"></param>
        /// </summary>
        public void Restore_DB(string Name_DataBase)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"SQL BackUp FIles ALL Files (*.*) |*.*| (*.Bak)|*.Bak";
            ofd.FilterIndex = 1;
            ofd.Title = "BackUp SQL Files";
            ofd.FileName = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                OpenConection();
                ExecuteNonQuery(@"Alter DATABASE [" + Name_DataBase + "] SET SINGLE_USER with ROLLBACK IMMEDIATE " + "USE master " + " RESTORE DATABASE [" + Name_DataBase + "] FROM DISK =N'" + ofd.FileName + "' with RECOVERY,REPLACE", new Dictionary<string, object>());
                CloseConnection();
            }
        }
    }
}

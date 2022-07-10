
using System;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data.Entity.SqlServer;
using System.Security.Cryptography;
using System.Configuration;
using Log_Book_Application.Config;

namespace Log_Book_Application.Includes
{
    class SQLConfig
    {
        //public string computerName = System.Environment.MachineName;
        //private MySqlConnection con = new MySqlConnection("data source = 192.168.254.110; port = 3306; user id = root ; database = bhms ; password = pass;");
        //private MySqlConnection con = new MySqlConnection("server = localhost; user id = root ; database = bhms ; password = ;");
       
        //SQLiteConnection con;
        //SQLiteCommand cmd;
        //SQLiteDataAdapter da;
        public DataTable dt;
        int result;
        usableFunction funct = new usableFunction();

        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        public void ConnectionString()
        {
            // string a = String.Format(@"Data Source = CommonSql\Database.db;Version=3;New=False;Read Only = False;Compress=True;Journal Mode=Off;providerName=System.Data.SQlite;");
            //USED IN SQL LITE Working
            // string a = String.Format(@"Data Source=EUGELAPTOPWIN10\SQLEXPRESS;Initial Catalog=DB;Integrated Security=True;");
            // con = new SQLiteConnection(a, true);


            string a = String.Format(@"Data Source=EUGELAPTOPWIN10\SQLEXPRESS;Initial Catalog=DB;Integrated Security=True;");
            con = new SqlConnection(a);
        }

        [SQLiteFunction(Name = "Sha1", Arguments = 1, FuncType = FunctionType.Scalar)]
        public class Sha1 : SQLiteFunction
        {

            public override object Invoke(object[] args)
            {
                var buffer = args[0] as byte[];

                if (buffer == null)
                {
                    var s = args[0] as string;

                    if (s != null)
                        buffer = Encoding.Unicode.GetBytes(s);
                }

                if (buffer == null)
                    return null;

                using (var sha1 = SHA1.Create())
                {
                    return sha1.ComputeHash(buffer);
                }
            }
        }


        public void Execute_CUD(string sql, string msg_false, string msg_true)
        {
            ConnectionString();
            //execute query ( command, iffalse, iftrue)
            try
            {
                con.Open();
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show(msg_true);
                }
                else
                {
                    MessageBox.Show(msg_false);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public void Execute_Query(string sql)
        {
            ConnectionString();
            //execute query ( command)
            try
            {
                con.Open();
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public void Load_DTG(string sql, DataGridView dtg)
        {
            ConnectionString();
            //to datagrid
            try
            {
                con.Open();
                cmd = new SqlCommand();
                da = new SqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);
                dtg.DataSource = dt;


                funct.ResponsiveDtg(dtg);
                //dtg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //dtg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }
        public void Load_Datasource(string sql, DataSet dtg)
        {
            ConnectionString();
            //to datagrid
            try
            {
                con.Open();
                cmd = new SqlCommand();
                da = new SqlDataAdapter();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dtg);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }
        public void fiil_CBO(string sql, ComboBox cbo)
        {
            ConnectionString();
            try
            {
                con.Open();
                cmd = new SqlCommand();
                da = new SqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.ValueMember = dt.Columns[0].ColumnName;
                cbo.DisplayMember = dt.Columns[0].ColumnName;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }


        }
        public void singleResult(string sql)
        {
            ConnectionString();
            try
            {
                con.Open();
                cmd = new SqlCommand();
                da = new SqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }

        public void LoadReports(string sql, DataSet ds)

        {
            ConnectionString();
            try
            {
                con.Open();
                cmd = new SqlCommand();
                da = new SqlDataAdapter();
                //DataTable dt = new DataTable();

                using (con)
                {
                    using (da = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
         

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }

        public void autocomplete(string sql, ComboBox txt)
        {
            ConnectionString();
            try
            {

                con.Open();
                cmd = new SqlCommand();
                da = new SqlDataAdapter();
                dt = new DataTable();

                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);
                txt.AutoCompleteCustomSource.Clear();
                txt.AutoCompleteMode = AutoCompleteMode.Suggest;
                txt.AutoCompleteSource = AutoCompleteSource.CustomSource;

                foreach (DataRow r in dt.Rows)
                {
                    txt.AutoCompleteCustomSource.Add(r.Field<string>(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }

      
        public void autonumber(string sql, TextBox txt)
        {
            ConnectionString();
            try
            {
                con.Open();
                cmd = new SqlCommand();
                da = new SqlDataAdapter();
                dt = new DataTable();


                cmd.Connection = con;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                da.Fill(dt);

                txt.Text = dt.Rows[0].Field<string>(0);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
            }
        }
        public void update_Autonumber(string id)
        {
            ConnectionString();
            Execute_Query("UPDATE `tblautonumber` SET `END`=`END`+`INCREMENT` WHERE `DESCRIPTION`='" + id + "'");
        }


        public void New_Autocomplete(string sql, TextBox txt)
        {
            ConnectionString();
            try
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
             
                while (reader.Read())
                {
                    MyCollection.Add(reader.GetString(0));
                  
                }
                txt.AutoCompleteCustomSource = MyCollection;
            

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }


        }
    }
}

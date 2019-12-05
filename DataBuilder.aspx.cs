using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
using System.Web.Caching;
using System.Runtime.Caching;
using Microsoft.Office.Interop.Excel;
using Microsoft.SqlServer.Management.Common;

namespace NSLookup
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        System.Text.StringBuilder DataServer = new System.Text.StringBuilder();
        System.Text.StringBuilder sbExcel1;
        ObjectCache cache = MemoryCache.Default;

        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
            Button3.Enabled = false;
            Label6.Visible = false;
            Label7.Visible = false;
        }

        //This queries the selected table for all its columns and populates CheckBoxList1
        private void database(string PrinterConnectionString)
        {
            CheckBoxList1.Items.Clear();
            string r = RadioButtonList1.SelectedValue;
            string queryString = "SELECT * FROM [dbo].[" + r + "]";

            using (SqlConnection connection = new SqlConnection(
                       PrinterConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        System.Web.UI.WebControls.CheckBox cb = new System.Web.UI.WebControls.CheckBox();

                        cb.Text = " " + reader.GetName(i);

                        CheckBoxList1.Items.Add(cb.Text);
                    }
                }


                finally
                {
                    reader.Close();
                    RadioButtonList1.Enabled = false;
                }

            }
        }

        //This action calls the above "database" function to populate CheckBoxList1 with the selected table's columns
        protected void Button1_Click(object sender, EventArgs e)
        {
            database(@"Data Source=" + TextBox3.Text + ";Initial Catalog=" + RadioButtonList2.SelectedValue + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;uid=" + TextBox1.Text + ";pwd=" + TextBox2.Text + ";");
            Button2.Enabled = true;
        }

        //This queries creates the table representation and sql query copy and paste page from the 
        //selected database, table and table rows user previously selected
        protected void Button2_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sbExcel = new System.Text.StringBuilder();
            
            sbExcel1 = new System.Text.StringBuilder();
            string r = RadioButtonList1.SelectedValue;
            string queryString = "SELECT * FROM [dbo].[" + r + "] ";
            Label1.Text = r;

                using (SqlConnection connection = new SqlConnection(
                           @"Data Source=" + TextBox3.Text + ";Initial Catalog=" + RadioButtonList2.SelectedValue + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;uid=" + TextBox1.Text + ";pwd=" + TextBox2.Text + ";"))
                {
                    SqlCommand command = new SqlCommand(
                        queryString, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {

                        TableHeaderRow thr = new TableHeaderRow();
                        Table1.Rows.Add(thr);
                        
                        for (int n = 0; n < reader.FieldCount; n++)
                        {

                            TableHeaderCell thc = new TableHeaderCell();

                            if (CheckBoxList1.Items[n].Selected)
                            {
                                thc.Text = reader.GetName(n);
                                thr.Cells.Add(thc);
                                sbExcel1.Append(reader.GetName(n) + ",");

                                if (n != CheckBoxList1.SelectedIndex) { sb.AppendLine(", " + "[" + reader.GetName(n) + "]"); }
                                else if (n == CheckBoxList1.SelectedIndex) { sb.AppendLine("SELECT " + "[" + reader.GetName(n) + "]"); }


                            }

                        }
                        sbExcel1.AppendLine(" ");

                        while (reader.Read())
                        {

                            TableRow tr = new TableRow();
                            Table1.Rows.Add(tr);

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                System.Web.UI.WebControls.CheckBox c = new System.Web.UI.WebControls.CheckBox();
                                TableCell tc = new TableCell();

                                if (CheckBoxList1.Items[i].Selected)
                                {
                                    tc.Text = reader[i].ToString();
                                    sbExcel1.Append(reader[i].ToString() + ",");

                                    tr.Cells.Add(tc);
                                }
                            }
                            sbExcel1.AppendLine(" ");
                        }

                    }

                    finally
                    {
                        DataServer.AppendLine(TextBox3.Text);
                        reader.Close();
                        sb.AppendLine("FROM [" + RadioButtonList2.SelectedValue + "].[dbo].[" + r + "]");
                        //sb.AppendLine(TextBox5.Text);
                        TextBox4.Text = sb.ToString();
                        //TextBox5.ReadOnly = true;
                        TextBox1.ReadOnly = false;
                        TextBox2.ReadOnly = false;
                        TextBox3.ReadOnly = false;
                        CheckBoxList1.Enabled = false;
                       
                        string fileContents = cache["filecontents"] as string;

                        if (fileContents == null)
                        {
                            CacheItemPolicy policy = new CacheItemPolicy();

                            List<string> filePaths = new List<string>();
                            filePaths.Add(@"C:\");

                            policy.ChangeMonitors.Add(new
                            HostFileChangeMonitor(filePaths));

                            // Fetch the file contents.
                            fileContents = sbExcel1.ToString();
                            cache.Set("filecontents", fileContents, policy);
                        }
                        
                        try
                        {
                            Label4.Text = "Check Tabs To View Generated Results";
                        }
                        catch
                        {

                        }
                    }

                }

        }

        static bool itemRemoved = false;
        static CacheItemRemovedReason reason;

        public void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
        {
            itemRemoved = true;
            reason = r;
        }

        //This queries the selected database for all its tables and populates RadioButtonList1
        protected void Button3_Click(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(@"Data Source=" + TextBox3.Text + ";Initial Catalog=" + RadioButtonList2.SelectedValue + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;uid=" + TextBox1.Text + ";pwd=" + TextBox2.Text + ";"))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", con))
                {
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        RadioButtonList1.Items.Clear();
                        while (reader.Read())
                        {
                            RadioButtonList1.Items.Add((string)reader["TABLE_NAME"]);
                        }
                    }
                }
                
                Button1.Enabled = true;
                RadioButtonList2.Enabled = false;

            }
        }

        //This connects user to the selected sql server and authenticates them to it
        protected void Button4_Click(object sender, EventArgs e)
        {
            Label1.Text = "Table Not Generated";
            Label4.Text = "";
            RadioButtonList2.Enabled = true;
            RadioButtonList1.Enabled = true;
            CheckBoxList1.Enabled = true;
            RadioButtonList1.Items.Clear();
            RadioButtonList2.Items.Clear();
            CheckBoxList1.Items.Clear();
            TextBox4.Text = "";
            
            cache.Remove("filecontents", null);

                try
                {

                    if (CheckBox1.Checked == true)
                    {
                        System.Data.SqlClient.SqlConnection SqlCon = new System.Data.SqlClient.SqlConnection(@"server=" + TextBox3.Text + ";Integrated Security=SSPI;");
                        SqlCon.Open();
                        System.Data.SqlClient.SqlCommand SqlCom = new System.Data.SqlClient.SqlCommand();
                        SqlCom.Connection = SqlCon;
                        SqlCom.CommandType = CommandType.StoredProcedure;
                        SqlCom.CommandText = "sp_databases";

                        System.Data.SqlClient.SqlDataReader SqlDR;
                        SqlDR = SqlCom.ExecuteReader();

                        while (SqlDR.Read())
                        {
                            RadioButtonList2.Items.Add(SqlDR.GetString(0));
                        }
                        Button3.Enabled = true;
                        TextBox1.ReadOnly = true;
                        TextBox2.ReadOnly = true;
                        TextBox3.ReadOnly = true;
                        Label6.Visible = false;
                        Label7.Visible = false;
                    }
                    else if(CheckBox1.Checked == false)
                    {
                        System.Data.SqlClient.SqlConnection SqlCon = new System.Data.SqlClient.SqlConnection(@"server=" + TextBox3.Text + ";uid=" + TextBox1.Text + ";pwd=" + TextBox2.Text + ";");
                        SqlCon.Open();
                        System.Data.SqlClient.SqlCommand SqlCom = new System.Data.SqlClient.SqlCommand();
                        SqlCom.Connection = SqlCon;
                        SqlCom.CommandType = CommandType.StoredProcedure;
                        SqlCom.CommandText = "sp_databases";

                        System.Data.SqlClient.SqlDataReader SqlDR;
                        SqlDR = SqlCom.ExecuteReader();

                        while (SqlDR.Read())
                        {
                            RadioButtonList2.Items.Add(SqlDR.GetString(0));
                        }
                        Button3.Enabled = true;
                        TextBox1.ReadOnly = true;
                        TextBox2.ReadOnly = true;
                        TextBox3.ReadOnly = true;
                        Label6.Visible = false;
                        Label7.Visible = false;
                    }
                }
                catch
                {

                }
        
            
        }




        protected void Button5_Click(object sender, EventArgs e)
        {
            
        }


        
        private void OnApplicationExit(object sender, EventArgs e)
        {
            Directory.Delete(@"C:\Generated Data");
        }
        //This button saves the user's query to a .csv file
        protected void Button5_Click1(object sender, EventArgs e)
        {
            
            Label6.Visible = true;
            
            try
            {
                
                string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    File.WriteAllText(filepath + "\\Temp.csv", cache.Get("filecontents", null).ToString());
                    Label6.Text = ".csv saved as " + filepath + "\\Temp.csv. MAKE SURE TO SAVE THE GENERATED FILE PERMANENTLY BECAUSE THE TEMP FILE WILL BE OVERWRITTEN.";
                    System.Diagnostics.Process.Start(filepath + "\\Temp.csv");
            }
            catch
            {
                Label6.Text = "File Already Exists";
            }

            try
            { 
            string r = RadioButtonList1.SelectedValue;
            string queryString = "SELECT * FROM [dbo].[" + r + "] ";
            Label1.Text = r;

                using (SqlConnection connection = new SqlConnection(
                           @"Data Source=" + TextBox3.Text + ";Initial Catalog=" + RadioButtonList2.SelectedValue + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;uid=" + TextBox1.Text + ";pwd=" + TextBox2.Text + ";"))
                {
                    SqlCommand command = new SqlCommand(
                        queryString, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {

                        TableHeaderRow thr = new TableHeaderRow();
                        Table1.Rows.Add(thr);
                        
                        for (int n = 0; n < reader.FieldCount; n++)
                        {

                            TableHeaderCell thc = new TableHeaderCell();

                            if (CheckBoxList1.Items[n].Selected)
                            {
                                thc.Text = reader.GetName(n);
                                thr.Cells.Add(thc);


                            }

                        }

                        while (reader.Read())
                        {

                            TableRow tr = new TableRow();
                            Table1.Rows.Add(tr);

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                System.Web.UI.WebControls.CheckBox c = new System.Web.UI.WebControls.CheckBox();
                                TableCell tc = new TableCell();

                                if (CheckBoxList1.Items[i].Selected)
                                {
                                    tc.Text = reader[i].ToString();

                                    tr.Cells.Add(tc);
                                }
                            }
                        }

                    }

                    finally
                    {
                        reader.Close();

                        try
                        {




                        }
                        catch
                        {

                        }
                    }

                }
            }
            catch
            {

            }
        
            
        }

        //This button saves the user's query as a .sql file
        protected void Button8_Click1(object sender, EventArgs e)
        {
            Label7.Visible = true;
            try
            {

                string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    File.WriteAllText(filepath + "\\Temp.sql", TextBox4.Text);
                    Label7.Text = ".sql saved as " + filepath + "\\Temp.sql. MAKE SURE TO SAVE THE GENERATED FILE PERMANENTLY BECAUSE THE TEMP FILE WILL BE OVERWRITTEN.";
                    System.Diagnostics.Process.Start(filepath + "\\Temp.sql");
            }
            catch
            {
                Label7.Text = "File Already Exists";
            }
            
            try
            {
                string r = RadioButtonList1.SelectedValue;
                string queryString = "SELECT * FROM [dbo].[" + r + "] ";
                Label1.Text = r;

                using (SqlConnection connection = new SqlConnection(
                           @"Data Source=" + TextBox3.Text + ";Initial Catalog=" + RadioButtonList2.SelectedValue + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;uid=" + TextBox1.Text + ";pwd=" + TextBox2.Text + ";"))
                {
                    SqlCommand command = new SqlCommand(
                        queryString, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {

                        TableHeaderRow thr = new TableHeaderRow();
                        Table1.Rows.Add(thr);
                        
                        for (int n = 0; n < reader.FieldCount; n++)
                        {

                            TableHeaderCell thc = new TableHeaderCell();

                            if (CheckBoxList1.Items[n].Selected)
                            {
                                thc.Text = reader.GetName(n);
                                thr.Cells.Add(thc);


                            }

                        }

                        while (reader.Read())
                        {

                            TableRow tr = new TableRow();
                            Table1.Rows.Add(tr);

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                System.Web.UI.WebControls.CheckBox c = new System.Web.UI.WebControls.CheckBox();
                                TableCell tc = new TableCell();

                                if (CheckBoxList1.Items[i].Selected)
                                {
                                    tc.Text = reader[i].ToString();

                                    tr.Cells.Add(tc);
                                }
                            }
                        }

                    }

                    finally
                    {
                        reader.Close();

                        try
                        {




                        }
                        catch
                        {

                        }
                    }

                }
            }
            catch
            {

            }

        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


        

    }
}
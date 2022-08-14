using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ADO.NET_HW_WF_Employees
{
    public partial class Form1 : Form
    {
        SqlConnection conn = null;
        SqlDataAdapter dataAdapter = null;
        DataSet dataSet = null;
        string cs = "";
        public Form1()
        {

            InitializeComponent();
            conn = new SqlConnection();
            cs = ConfigurationManager.ConnectionStrings["Company_DB"].ConnectionString;
            conn.ConnectionString = cs;

            dataAdapter = new SqlDataAdapter("select * from Employee;", conn);
            SqlCommandBuilder cmb = new SqlCommandBuilder(dataAdapter);
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Employee");
            dataGridView1.DataSource = dataSet.Tables["Employee"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            int index = dataGridView1.CurrentRow.Index;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Company_DB"].ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(" SELECT * FROM Employee", conn);
                conn.Open();
                dataAdapter.Fill(ds, "Employee");
                conn.Close();
            };
            Form2 form2 = new Form2(ds, index);
            form2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Company_DB"].ConnectionString))
            {
                conn.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter("Select *from [dbo].[Employee]", conn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(Adapter);
                DataSet dataSet = new DataSet();
                Adapter.Fill(dataSet, "Employee");
                DataTable empl = dataSet.Tables["Employee"];
                Adapter.DeleteCommand = new SqlCommand("stp_EmployeeDelete_1", conn);
                Adapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
                Adapter.DeleteCommand.Parameters.AddWithValue("EmployeeID", dataGridView1.CurrentRow.Cells[0].Value);

                Adapter.DeleteCommand.ExecuteNonQuery();
                empl.Clear();
                Adapter.Fill(dataSet, "Employee");
                dataGridView1.DataSource = empl;
                
                conn.Close();
            }
        }
    }
}

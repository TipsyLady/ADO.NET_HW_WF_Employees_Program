using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO.NET_HW_WF_Employees
{
    public partial class Form2 : Form
    {
        DataSet ds;
        int index =-1;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(DataSet ds, int index)
        {
            InitializeComponent();
            this.ds = ds;
            this.index = index;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Company_DB"].ConnectionString))
            {
                conn.Open();
                SqlCommand Add = new SqlCommand("stp_EmployeeAdd", conn);
                Add.CommandType = CommandType.StoredProcedure;
                Add.Parameters.AddWithValue("FirstName", textBox1.Text);
                Add.Parameters.AddWithValue("LastName", textBox2.Text);
                Add.Parameters.AddWithValue("BirthDate", dateTimePicker1.Value);
                Add.Parameters.AddWithValue("PositionID", comboBox1.SelectedItem);
                Add.Parameters.AddWithValue("Salary", textBox3.Text);
                Add.Parameters.AddWithValue("EmployeeID", 1);

                Add.ExecuteNonQuery();
                conn.Close();
                Close();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Company_DB"].ConnectionString))
            {
                conn.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter("Select *from [dbo].[Employee]", conn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(Adapter);
                DataSet dataSet = new DataSet();
                Adapter.Fill(dataSet, "Employee");
                DataTable empl = dataSet.Tables["Employee"];
                Adapter.UpdateCommand = new SqlCommand("stp_EmployeeUpdate", conn);
                Adapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
                Adapter.UpdateCommand.Parameters.AddWithValue("FirstName", textBox1.Text);
                Adapter.UpdateCommand.Parameters.AddWithValue("LastName", textBox2.Text);
                Adapter.UpdateCommand.Parameters.AddWithValue("BirthDate", dateTimePicker1.Value);
                Adapter.UpdateCommand.Parameters.AddWithValue("PositionID", comboBox1.SelectedItem);
                Adapter.UpdateCommand.Parameters.AddWithValue("Salary", textBox3.Text);
                Adapter.UpdateCommand.Parameters.AddWithValue("EmployeeID", ds.Tables[0].Rows[index].Field<int>("EmployeeID"));
                Adapter.UpdateCommand.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;

                Adapter.UpdateCommand.ExecuteNonQuery();
                empl.Clear();
                Adapter.Fill(dataSet, "Employee");
                conn.Close();
                Close();
            }
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            if (index != -1)
            {
                textBox1.Text = ds.Tables[0].Rows[index].Field<string>("FirstName");
                textBox2.Text = ds.Tables[0].Rows[index].Field<string>("LastName");
                dateTimePicker1.Value = ds.Tables[0].Rows[index].Field<DateTime>("BirthDate");
                comboBox1.SelectedIndex = ds.Tables[0].Rows[index].Field<int>("PositionId") - 1;
                textBox3.Text = ds.Tables[0].Rows[index].Field<int>("Salary").ToString();
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

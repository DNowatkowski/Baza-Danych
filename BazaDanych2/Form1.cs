using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BazaDanych2
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dominik\source\repos\BazaDanych2\BazaDanych2\UserDataBase.mdf;Integrated Security=True");
        SqlCommand command;
        SqlDataAdapter adapter;
        SqlDataReader reader;
        string recordCheck = "";
        string SelectedName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayData();
            tableDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (!RecordExists(textBox1.Text))
                {
                    command = new SqlCommand("INSERT INTO [Table] VALUES (@name)", connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@name", textBox1.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record added successfully");
                    textBox1.Text = "";
                }
                else
                    MessageBox.Show("Record already exists!");
            }
            else
                MessageBox.Show("Please provide a valid user name!");
            DisplayData();
        }

        private void DisplayData()
        {
            connection.Open();
            DataTable dt = new DataTable();
            adapter = new SqlDataAdapter("SELECT * FROM [Table]",connection);
            adapter.Fill(dt);
            tableDataGridView.DataSource = dt;
            connection.Close();
        }

        private bool RecordExists(string name)
        {
            connection.Open();
            recordCheck = "";
            command = new SqlCommand("SELECT UserName FROM [Table] WHERE UserName=@name;", connection);
            command.Parameters.AddWithValue("@name", name);
            reader = command.ExecuteReader();
            while (reader.Read())
                recordCheck = reader.GetValue(0).ToString();
            connection.Close();
            if (recordCheck == "")
                return false;
            else
                return true;
        }


        private void tableDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SelectedName = string.Concat(tableDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString().Where(c=>!char.IsWhiteSpace(c)));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedName))
            {
                command = new SqlCommand("DELETE FROM [Table] WHERE UserName=@SelectedName", connection);
                connection.Open();
                command.Parameters.AddWithValue("@SelectedName", SelectedName);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Record deleted successfully!");
                DisplayData();
            }
            
        }
    }
}
